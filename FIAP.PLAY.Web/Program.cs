using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Services;
using FIAP.PLAY.Application.Library.Validations;
using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Services;
using FIAP.PLAY.Application.UserAccess.Validations;
using FIAP.PLAY.Infrastructure;
using FIAP.PLAY.Infrastructure.Data;
using FIAP.PLAY.Infrastructure.Logging;
using FIAP.PLAY.Infrastructure.Logging.Correlation;
using FIAP.PLAY.Web.Filters.Shared;
using FIAP.PLAY.Web.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FiapPlayContext>(options =>
    options.UseSqlServer(connectionString));

// CORS
builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    }));

// JWT
var key = builder.Configuration["JwtSecurityToken:Key"];
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    var keys = Encoding.ASCII.GetBytes(key);
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keys)
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/Messaging") || path.StartsWithSegments("/Comments")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RequestValidationFilter>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// MVC + FluentValidation
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new CustomExceptionFilterAttribute());
    })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<UserRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<GameRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<LibraryRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<GameLibraryRequestValidator>();
    });

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EspecificacaoAnalise",
        Version = "v1",
        Description = "Management System"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Middlewares e serviços customizados
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddTransient<ICorrelationIdGenerator, CorrelationIdGenerator>();
builder.Services.AddTransient(typeof(ILoggerManager<>), typeof(LoggerManager<>));
builder.Services.AddTransient(typeof(BaseLogger<>));

builder.Services.AddScoped<IJWTService, JWTService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IGameLibraryService, GameLibraryService>();

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IPromotionGameService, PromotionGameService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

var app = builder.Build();

// Seeding do banco
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FiapPlayContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu erro no metodo Seeding.");
    }
}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<CorrelationMiddleware>();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var swaggerEndpoint = app.Environment.IsDevelopment()
        ? "/swagger/v1/swagger.json"
        : "/ea/swagger/v1/swagger.json";
    c.SwaggerEndpoint(swaggerEndpoint, "FiapPlay");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

try
{
    Log.Information("Iniciando aplicação FIAP.PLAY.Web");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação terminou inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}
