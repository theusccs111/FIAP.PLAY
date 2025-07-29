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
        private readonly IMapper _mapper;
        protected IMapper Mapper { get { return _mapper; } }
        private readonly IConfiguration _config;
        protected IConfiguration Config { get { return _config; } }

        private readonly IValidator<Entity> _validator;
        protected IValidator<Entity> Validator { get { return _validator; } }
        public Service(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Entity> validator)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuario = LoginResponseHelper.ObterLoginResponse(_httpContextAccessor);
            _mapper = mapper;
            _uow = uow;
            _config = config;
            _validator = validator; 
        }

        public virtual Resultado<IEnumerable<Response>> Get()
        {
            var entities = _uow.Repository<Entity>().GetAll();
            return new Resultado<IEnumerable<Response>>(Mapper.Map<IEnumerable<Response>>(entities));
        }

        public virtual Resultado<Response> GetById(int Id)
        {
            var entity = _uow.Repository<Entity>().GetFirst(e => e.Id == Id);
            return new Resultado<Response>(Mapper.Map<Response>(entity));
        }

        public virtual Resultado<Response> Add(Request request)
        {
            var entity = Mapper.Map<Entity>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            _uow.Repository<Entity>().Create(entity);
            this.Complete();

            return new Resultado<Response>(Mapper.Map<Response>(entity));
        }

        public virtual Resultado<Response[]> AddMany(Request[] request)
        {
            var resultList = new List<Response>();

            foreach (var item in request)
            {
                var result = Add(item).Data;
                resultList.Add(result!);
            }
            this.Complete();

            return new Resultado<Response[]>(resultList.ToArray());
        }

        public virtual Resultado<Response> Update(Request request)
        {
            var entity = Mapper.Map<Entity>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            if(entity.Id > 0)
                _uow.Repository<Entity>().Update(entity);
            else
                _uow.Repository<Entity>().Create(entity);
            this.Complete();

            return new Resultado<Response>(Mapper.Map<Response>(entity));

        }

        public virtual Resultado<Response[]> UpdateMany(Request[] request)
        {
            var resultList = new List<Response>();

            foreach (var item in request)
            {
                var result = Update(item).Data;
                resultList.Add(result!);
            }
            this.Complete();

            return new Resultado<Response[]>(resultList.ToArray());
        }

        public virtual Resultado<Response> Delete(long id)
        {
            var entity = _uow.Repository<Entity>().GetFirst(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Entidade {nameof(Entity)} com ID {id} não encontrada.");

            _uow.Repository<Entity>().Delete(entity);
            this.Complete();

            return new Resultado<Response>(Mapper.Map<Response>(entity));
        }


        public virtual Resultado<Response[]> DeleteMany(long[] ids)
        {
            var resultList = new List<Response>();

            foreach (var item in ids)
            {
                var result = Delete(item).Data;
                resultList.Add(result!);
            }
            this.Complete();

            return new Resultado<Response[]>(resultList.ToArray());
        }

        public virtual void Complete()
        {
            Uow.Complete();
        }
    }
}
