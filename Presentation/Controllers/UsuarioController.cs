using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Presentation.Dtos.Usuarios;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("auth/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly string secretKey;

        public UsuarioController(EstacionamientoContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            secretKey = config.GetSection("JwtSettings").GetSection("SecretKey").ToString();
        }

        [HttpPost]
        [Route("IniciarSesion")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult IniciarSesion(UsuarioDto usuario)
        {
            var usuarioEncontrado = _dbContext.Usuarios
                            .FromSqlRaw("EXEC dbo.Acceder @Nombre={0}, @Clave={1}", usuario.Nombre, usuario.Clave)
                            .AsEnumerable()
                            .Select(u => new { u.IdUsuario, u.Nombre, u.Estado })
                            .FirstOrDefault() ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

            if (!usuarioEncontrado.Estado)
                throw new ForbiddenException("Usuario bloqueado.");

            var bytesKey = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("usuario", usuario.Nombre));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(18),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytesKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return Ok(tokenCreado);
        }
    }
}
