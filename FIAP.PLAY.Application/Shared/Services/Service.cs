using AutoMapper;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
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
    public abstract class Service<T,R> where T : EntidadeBase where R : ResourceBase
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

        private readonly IValidator<T> _validator;
        protected IValidator<T> Validator { get { return _validator; } }
        public Service(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<T> validator)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuario = LoginResponseHelper.ObterLoginResponse(_httpContextAccessor);
            _mapper = mapper;
            _uow = uow;
            _config = config;
            _validator = validator; 
        }

        public virtual Resultado<IEnumerable<R>> Get()
        {
            var entities = _uow.Repository<T>().GetAll();
            return new Resultado<IEnumerable<R>>(Mapper.Map<IEnumerable<R>>(entities));
        }

        public virtual Resultado<R> GetById(int Id)
        {
            var entity = _uow.Repository<T>().GetFirst(e => e.Id == Id);
            return new Resultado<R>(Mapper.Map<R>(entity));
        }

        public virtual Resultado<R> Add(R request)
        {
            var entity = Mapper.Map<T>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            _uow.Repository<T>().Create(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));
        }

        public virtual Resultado<R[]> AddMany(R[] request)
        {
            var resultList = new List<R>();

            foreach (var item in request)
            {
                var result = Add(item).Data;
                resultList.Add(result!);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual Resultado<R> Update(R request)
        {
            var entity = Mapper.Map<T>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException(validReturn.Errors.ToList());

            if(entity.Id > 0)
                _uow.Repository<T>().Update(entity);
            else
                _uow.Repository<T>().Create(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));

        }

        public virtual Resultado<R[]> UpdateMany(R[] request)
        {
            var resultList = new List<R>();

            foreach (var item in request)
            {
                var result = Update(item).Data;
                resultList.Add(result!);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual Resultado<R> Delete(long id)
        {
            var entity = _uow.Repository<T>().GetFirst(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Entidade com ID {id} não encontrada.");

            _uow.Repository<T>().Delete(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));
        }


        public virtual Resultado<R[]> DeleteMany(long[] ids)
        {
            var resultList = new List<R>();

            foreach (var item in ids)
            {
                var result = Delete(item).Data;
                resultList.Add(result!);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual void Complete()
        {
            Uow.Complete();
        }
    }
}
