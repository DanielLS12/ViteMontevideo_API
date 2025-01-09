using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses;
using ViteMontevideo_API.Services.Dtos.Egresos.Responses;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class CajaChicaRepository : BaseRepository<int, CajaChica>, ICajaChicaRepository
    {
        public CajaChicaRepository(EstacionamientoContext context, ILogger<BaseRepository<int, CajaChica>> logger) : base(context, logger)
        {
        }

        public override async Task<CajaChica?> GetById(int id) =>
            await _context.CajasChicas
                .AsNoTracking()
                .Include(cc => cc.Trabajador)
                .FirstOrDefaultAsync(cc => cc.IdCaja == id);

        public async Task<CajaChica?> GetOpenCajaChica() =>
            await _context.CajasChicas.FirstOrDefaultAsync(cc => cc.Estado);

        public async Task<List<InformeCajaChica>> GetAllInformes(DateTime fecha) =>
            await _context.CajasChicas
                .AsNoTracking()
                .Where(cc => cc.FechaInicio == fecha)
                .Select(cc => new InformeCajaChica
                {
                    Cajero = $"{cc.Trabajador.Nombre} {cc.Trabajador.ApellidoPaterno} {cc.Trabajador.ApellidoMaterno}",
                    MParticulares = cc.Servicios.Where(s => s.Tarifa!.Actividad.Nombre == "Particular").Sum(s => s.Monto),
                    MTurnos = cc.Servicios.Where(s => s.Tarifa!.HoraDia != null && s.Tarifa.Actividad.Nombre != "EsSalud").Sum(s => s.Monto),
                    MEsSalud = cc.Servicios.Where(s => s.Tarifa!.Actividad.Nombre == "EsSalud").Sum(s => s.Monto),
                    MEfectivo = SumarPorTipoPagoServicio(cc.Servicios, TipoPago.Efectivo.ToString()) +
                            SumarPorTipoPagoAbonados(cc.ContratosAbonados, TipoPago.Efectivo.ToString()) +
                            SumarPorTipoPagoComerciosAdicionales(cc.ComerciosAdicionales, TipoPago.Efectivo.ToString()) -
                            cc.Egresos.Sum(e => e.Monto),
                    MYape = SumarPorTipoPagoServicio(cc.Servicios, TipoPago.Yape.ToString()) +
                        SumarPorTipoPagoAbonados(cc.ContratosAbonados, TipoPago.Yape.ToString()) +
                        SumarPorTipoPagoComerciosAdicionales(cc.ComerciosAdicionales, TipoPago.Yape.ToString()),
                    MOtros = SumarPorTipoPagoServicio(cc.Servicios, TipoPago.Otros.ToString()) +
                         SumarPorTipoPagoAbonados(cc.ContratosAbonados, TipoPago.Otros.ToString()) +
                         SumarPorTipoPagoComerciosAdicionales(cc.ComerciosAdicionales, TipoPago.Otros.ToString()),
                    MServicios = cc.TotalMontoServicios,
                    MContratosAbonados = cc.TotalMontoAbonados,
                    MComerciosAdicionales = cc.TotalMontoComerciosAdicionales,
                    MEgresos = cc.TotalMontoEgresos,
                    ComerciosAdicionales = cc.ComerciosAdicionales.Select(ca => new ComercioAdicionalInfoResponseDto
                    {
                        Monto = ca.Monto,
                        Cliente = new()
                        {
                            Nombres = ca.Cliente.Nombres,
                            Apellidos = ca.Cliente.Apellidos
                        }
                    }).ToList(),
                    Egresos = cc.Egresos.Select(e => new EgresoInfoResponseDto() 
                    { 
                        Motivo = e.Motivo,
                        Monto = e.Monto
                    }).ToList(),
                    Abonados = cc.ContratosAbonados.Select(ca => new ContratoAbonadoInfoResponseDto() {
                        Monto = ca.Monto,
                        Vehiculo = new()
                        {
                            Placa = ca.Vehiculo.Placa
                        }
                    }).ToList(),
                }).ToListAsync();

        public async Task<bool> ExistsOpenCajaChica() =>
            await _context.CajasChicas.AnyAsync(cc => cc.Estado);

        public async Task<bool> IsCajaChicaClosedById(int id) =>
            await _context.CajasChicas.AnyAsync(cc => cc.IdCaja == id && !cc.Estado);

        public async Task<bool> HasContratosAbonadosOrEgresosOrServicios(int id) =>
            await _context.CajasChicas.AnyAsync(cc => cc.IdCaja == id && (cc.ContratosAbonados.Any() || cc.Egresos.Any() || cc.ComerciosAdicionales.Any()));

        private static decimal SumarPorTipoPagoServicio(IEnumerable<Servicio> servicios, string tipoPago)
        {
            decimal montoTotal = servicios.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }

        private static decimal SumarPorTipoPagoAbonados(IEnumerable<ContratoAbonado> abonados, string tipoPago)
        {
            decimal montoTotal = abonados.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }

        private static decimal SumarPorTipoPagoComerciosAdicionales(IEnumerable<ComercioAdicional> ca, string tipoPago)
        {
            decimal montoTotal = ca.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }
    }
}
