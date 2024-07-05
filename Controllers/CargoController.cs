using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CargoController : ControllerBase
    {
        public readonly EstacionamientoContext _dbContext;

        public CargoController(EstacionamientoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Listar() 
        {
            var data = _dbContext.Cargos
                .AsNoTracking()
                .ToList();
            var cantidad = data.Count;

            return Ok(new DataResponse<Cargo>(cantidad, data));
        }
    }
}
