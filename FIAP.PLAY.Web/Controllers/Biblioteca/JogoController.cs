using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController(IJogoService _jogoService) : ControllerBase
    {       

        [HttpGet]
        public IActionResult Get() 
            => Ok( _jogoService.Get());
       
        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
            => Ok(_jogoService.GetById(id));

        [HttpPost]
        public IActionResult Post([FromBody] JogoRequest request)
            => Created("api/Jogo", _jogoService.Add(request));

        [HttpPut]
        public IActionResult Put(JogoRequest request)
            => Ok(_jogoService.Update(request));

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _jogoService.Delete(id);           
            return NoContent();
        }
    }
}
