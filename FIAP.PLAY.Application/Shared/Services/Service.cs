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
    public abstract class Service<Entity, Request, Response> where Entity : EntidadeBase where Request : RequestBase where Response : ResponseBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        private LoginResponse _usuario;
        protected LoginResponse Usuario { get { return _usuario; } set { _usuario = value; } }
        private readonly IUnityOfWork _uow;
        protected IUnityOfWork Uow { get { return _uow; } }

        private readonly IValidator<Entity> _validator;
        protected IValidator<Entity> Validator { get { return _validator; } }
        public Service(IHttpContextAccessor httpContextAccessor, IUnityOfWork uow, IValidator<Entity> validator)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuario = LoginResponseHelper.ObterLoginResponse(_httpContextAccessor);
            _uow = uow;
            _validator = validator; 
        }

        public virtual Resultado<IEnumerable<Response>> Get()
        {
            var entities = _uow.Repository<Entity>().GetAll();
            return new Resultado<IEnumerable<Response>>(entities);
        }

        public virtual Resultado<Response> GetById(int Id)
        {
            var entity = _uow.Repository<Entity>().GetFirst(e => e.Id == Id);
            return new Resultado<Response>(entity);
        }

        public virtual Resultado<Response> Add(Entity entity)
        {
            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            _uow.Repository<Entity>().Create(entity);
            this.Complete();

            return new Resultado<Response>(entity);
        }

        public virtual Resultado<Response> Update(Entity entity)
        {
            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            if(entity.Id > 0)
                _uow.Repository<Entity>().Update(entity);
            else
                _uow.Repository<Entity>().Create(entity);
            this.Complete();

            return new Resultado<Response>(entity);

        }

        public virtual Resultado<Response> Delete(long id)
        {
            var entity = _uow.Repository<Entity>().GetFirst(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Entidade {nameof(Entity)} com ID {id} não encontrada.");

            _uow.Repository<Entity>().Delete(entity);
            this.Complete();

            return new Resultado<Response>(entity);
        }


        public virtual void Complete()
        {
            Uow.Complete();
        }
    }
}
