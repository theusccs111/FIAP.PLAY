using AutoMapper;
using FIAP.PLAY.Domain.Shared.Resource.Request;
using FIAP.PLAY.Domain.Shared.Resource.Response;
using FIAP.PLAY.Domain.UserAccess.Entities;

namespace FIAP.PLAY.Domain
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            var entityAssemplyEntity = typeof(Usuario).Assembly;
            var entityAssemplyRequest = typeof(UsuarioRequest).Assembly.ExportedTypes.ToList();
            var entityAssemplyResponse = typeof(UsuarioResponse).Assembly.ExportedTypes.ToList();

            entityAssemplyEntity.ExportedTypes.ToList().ForEach(s =>
            {
                var formattedRequestModelName = string.Format("{0}Request", s.Name);
                var requestModelName = entityAssemplyRequest.FirstOrDefault(s => s.Name == formattedRequestModelName);
                if (requestModelName != null)
                {
                    this.CreateMap(s, requestModelName).ReverseMap();
                }

                var formattedResponseName = string.Format("{0}Response", s.Name);
                var responseModelName = entityAssemplyResponse.FirstOrDefault(s => s.Name == formattedResponseName);
                if (responseModelName != null)
                {
                    var map = this.CreateMap(s, responseModelName).ReverseMap();
                   
                }
            });

        }
    }
}
