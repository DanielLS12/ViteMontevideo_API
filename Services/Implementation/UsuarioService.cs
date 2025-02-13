using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Usuarios;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly string secretKey;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            secretKey = config["JwtSettings:SecretKey"]!;
        }

        public async Task<DataResponse<UsuarioBasicResponseDto>> GetAvailableUsers()
        {
            var usuarios = await _usuarioRepository.GetAvailableUsers();
            int cantidad = usuarios.Count;
            var data = _mapper.Map<List<UsuarioBasicResponseDto>>(usuarios);
            return new DataResponse<UsuarioBasicResponseDto>(cantidad, data);
        }

        public async Task<UsuarioResponseDto> GetByUsername(string username)
        {
            var usuario = await _usuarioRepository.GetByUsername(username)
                ?? throw new NotFoundException("Usuario no encontrado");

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<ApiResponse> Register(UsuarioRequestDto usuario)
        {
            bool existsUsuario = await _usuarioRepository.ExistsUsername(usuario.Nombre);
            if (existsUsuario)
                throw new ConflictException("El usuario que se intento registrar ya existe.");

            var dbUsuario = _usuarioRepository.Register(usuario.Nombre, usuario.Clave)
                ?? throw new BadRequestException("Ha ocurrido un error al intentar guardar el usuario.");

            return ApiResponse.Success("El usuario ha sido creado.", _mapper.Map<UsuarioResponseDto>(dbUsuario));
        }

        public async Task<LoginResponseDto> Login(UsuarioRequestDto usuario)
        {
            var dbUsuario = _usuarioRepository.Login(usuario.Nombre, usuario.Clave)
                ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

            if (!dbUsuario.Estado)
                throw new BadRequestException("Usuario bloqueado.");

            return new LoginResponseDto
            {
                AccessToken = await GenerateToken(usuario)
            };
        }

        private async Task<string> GenerateToken(UsuarioRequestDto usuario)
        {
            var bytesKey = Encoding.ASCII.GetBytes(secretKey);

            var roleName = await _usuarioRepository.GetCargoByUsername(usuario.Nombre);

            if (string.IsNullOrEmpty(roleName))
                roleName = "Expectador";

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
            claims.AddClaim(new Claim(ClaimTypes.Role, roleName));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(18),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytesKey), SecurityAlgorithms.HmacSha256),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
