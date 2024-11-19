﻿using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class VehiculoRepository : BaseRepository<int, Vehiculo>, IVehiculoRepository
    {
        public VehiculoRepository(EstacionamientoContext context, ILogger<BaseRepository<int, Vehiculo>> logger) : base(context, logger)
        {
        }

        public IQueryable<Vehiculo> ApplyPageCursor(IQueryable<Vehiculo> query, int cursor, int count, int MaxRegisters)
        {
            if (cursor > 0)
                query = query.Where(v => v.IdVehiculo < cursor);

            return query.Take(count > MaxRegisters ? MaxRegisters : count);
        }

        public async Task<bool> ExistsByIdAndPlaca(int id, string placa) =>
            await _context.Vehiculos.AnyAsync(v => v.Placa == placa.ToUpper() && v.IdVehiculo != id);

        public async Task<bool> ExistsByPlaca(string placa) =>
            await _context.Vehiculos.AnyAsync(v => v.Placa == placa.ToUpper());

        public override async Task<Vehiculo> GetById(int id) =>
            await _context.Vehiculos
                .Include(v => v.Tarifa)
                    .ThenInclude(t => t.Categoria)
                .Include(v => v.Tarifa)
                    .ThenInclude(t => t.Actividad)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.IdVehiculo == id) 
            ?? throw new NotFoundException("Vehículo no encontrado");

        public async Task<Vehiculo> GetByPlaca(string placa) =>
            await _context.Vehiculos
                .Include(v => v.Tarifa)
                    .ThenInclude(t => t.Categoria)
                .Include(v => v.Tarifa)
                    .ThenInclude(t => t.Actividad)
                .Include(v => v.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Placa == placa.ToUpper())
            ?? throw new NotFoundException("Vehículo no encontrado.");

        public async Task<bool> HasAbonadosById(int id) =>
            await _context.Vehiculos.AnyAsync(v => v.IdVehiculo == id && v.ContratosAbonados.Any());

        public async Task<bool> HasServiciosById(int id) =>
            await _context.Vehiculos.AnyAsync(v => v.IdVehiculo == id && v.Servicios.Any());

        public IQueryable<Vehiculo> Query() => _context.Vehiculos.AsNoTracking().AsQueryable();
    }
}