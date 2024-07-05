using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Clientes;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public ClienteController(EstacionamientoContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var clientes = _dbContext.Clientes
                .AsNoTracking()
                .OrderByDescending(c => c.IdCliente)
                .ProjectTo<ClienteResponseDto>(_mapper.ConfigurationProvider)
                .ToList();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var cliente = _dbContext.Clientes
                .AsNoTracking()
                .ProjectTo<ClienteResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(c => c.IdCliente == id) ?? throw new NotFoundException("Cliente no encontrado.");
            return Ok(cliente);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(ClienteRequestDto clienteDto)
        {
            clienteDto = LimpiarDatos(clienteDto);

            var cliente = _mapper.Map<Cliente>(clienteDto);

            _dbContext.Clientes.Add(cliente);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El cliente ha sido agregado.");

            return CreatedAtAction(nameof(Obtener), new { id = cliente.IdCliente }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] ClienteRequestDto clienteDto)
        {
            var dbCliente = _dbContext.Clientes.Find(id) ?? throw new NotFoundException("Cliente no encontrado.");

            clienteDto = LimpiarDatos(clienteDto);

            dbCliente.Nombres = clienteDto.Nombres.Trim();
            dbCliente.Apellidos = clienteDto.Apellidos.Trim();
            dbCliente.Telefono = string.IsNullOrWhiteSpace(clienteDto.Telefono) ? null : clienteDto.Telefono.Trim();
            dbCliente.Correo = string.IsNullOrWhiteSpace(clienteDto.Correo) ? null : clienteDto.Correo.Trim();

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El cliente ha sido actualizado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var cliente = _dbContext.Clientes.Find(id) ?? throw new NotFoundException("Cliente no encontrado.");

            var tieneComerciosAdicionales = _dbContext.ComerciosAdicionales.Any(ca => ca.IdCliente == id);

            if (tieneComerciosAdicionales)
                throw new BadRequestException("No se puede eliminar este cliente porque tiene comercio(s) adicional(es).");

            _dbContext.Clientes.Remove(cliente);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El cliente ha sido eliminado.");

            return Ok(response);
        }

        private static ClienteRequestDto LimpiarDatos(ClienteRequestDto cliente)
        {
            cliente.Nombres = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Nombres, @"\s+", " ").Trim());
            cliente.Apellidos = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Apellidos, @"\s+", " ").Trim());
            if (cliente.Telefono != null) cliente.Telefono = Regex.Replace(cliente.Telefono, @"\s+", " ").Trim();
            if (cliente.Correo != null) cliente.Correo = Regex.Replace(cliente.Correo, @"\s+", "");
            return cliente;
        }
    }
}
