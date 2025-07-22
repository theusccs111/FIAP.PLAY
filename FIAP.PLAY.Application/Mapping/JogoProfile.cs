using AutoMapper;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;

namespace FIAP.PLAY.Application.Mapping
{
    public class JogoProfile : Profile
    {
        public JogoProfile() 
        {
            CreateMap<Jogo, JogoRequest>();
            CreateMap<JogoRequest, Jogo>();
            CreateMap<Jogo, JogoResponse>();
        }
    }
}
