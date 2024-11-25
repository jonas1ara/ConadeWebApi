using AccesoDatos.Models;
using AccesoDatos.Operations;
using ClasesBase.Respuestas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConadeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoInstalacionesWS : ControllerBase
    {
        CatalogoInstalacionesDao dao = new CatalogoInstalacionesDao();

        [HttpPost("Guardar")]
        public Respuesta guardar (string nombreInstalacion)
        {
            return dao.Guardar(nombreInstalacion);
        }

        [HttpGet("ObtenerTodas")]
        public List<CatalogoInstalaciones> obtenerTodas()
        {
            return dao.ObtenerTodas();
        }

        [HttpPost("Actualizar")]
        public Respuesta Actualizar(int id, string nuevoNombre)
        {
            return dao.Actualizar(id, nuevoNombre);
        }

        [HttpDelete("Eliminar")]
        public Respuesta Eliminar(int id)
        {
            return dao.Eliminar(id);
        }
    }
}
