using AccesoDatos.Operations;
using AccesoDatos.Models;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/area")]
    [ApiController]
    public class AreaWS : ControllerBase
    {

        AreaDao dao = new AreaDao();

        [HttpGet("listar")]
        public Respuesta mostrar()
        {
            return dao.mostrar();
        }

        [HttpPost("guardar")]
        public Respuesta guardar(string nombreArea)
        {
            return dao.guardar(nombreArea);
        }

        [HttpGet("buscar")]
        public Respuesta buscar(string nombreArea)
        {
            return dao.buscar(nombreArea);
        }

        [HttpPost("actualizar")]
        public Respuesta actualizarArea([FromBody] Area area)
        {
            return dao.actualizarArea(area);
        }

        [HttpPost("eliminar")]
        public Respuesta eliminarArea(int areaId)
        {
            return dao.eliminarArea(areaId);
        }


    }
}
