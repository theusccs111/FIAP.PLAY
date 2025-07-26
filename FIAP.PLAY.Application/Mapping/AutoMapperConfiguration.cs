using AutoMapper;
using FIAP.PLAY.Domain.Shared.Entities;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Pega todos os tipos exportados (public) de todos os assemblies carregados
        var allExportedTypes = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

        // Obtém todas as entidades do domínio (por convenção, que herdam de EntidadeBase)
        var entidades = allExportedTypes
            .Where(t => t.IsClass && !t.IsAbstract && typeof(EntidadeBase).IsAssignableFrom(t))
            .ToList();

        foreach (var entidade in entidades)
        {
            var nomeEntidade = entidade.Name;

            var tipoRequest = allExportedTypes.FirstOrDefault(x => x.Name == $"{nomeEntidade}Request");
            if (tipoRequest != null)
            {
                CreateMap(entidade, tipoRequest).ReverseMap();
            }

            var tipoResponse = allExportedTypes.FirstOrDefault(x => x.Name == $"{nomeEntidade}Response");
            if (tipoResponse != null)
            {
                CreateMap(entidade, tipoResponse).ReverseMap();
            }
        }
    }
}
