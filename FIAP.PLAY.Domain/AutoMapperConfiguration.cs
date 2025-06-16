using AutoMapper;
using FIAP.PLAY.Domain.Entities;
using FIAP.PLAY.Domain.Resource.Request;
using FIAP.PLAY.Domain.Resource.Response;

namespace FIAP.PLAY.Domain
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            var entityAssemplyEntity = typeof(User).Assembly;
            var entityAssemplyRequest = typeof(UserRequest).Assembly.ExportedTypes.ToList();
            var entityAssemplyResponse = typeof(UserResponse).Assembly.ExportedTypes.ToList();

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
