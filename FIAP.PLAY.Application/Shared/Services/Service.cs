using AutoMapper;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Helpers;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FIAP.PLAY.Application.Shared.Services
{
    public abstract class Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        private LoginResponse _usuario;
        protected LoginResponse Usuario { get { return _usuario; } set { _usuario = value; } }
        public Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuario = LoginResponseHelper.ObterLoginResponse(_httpContextAccessor);
        }

       
    }
}
