﻿using System.Text.Json.Serialization;

namespace ViteMontevideo_API.Persistence.Models;
public partial class Vehiculo
{
    public int IdVehiculo { get; set; }
    public short IdTarifa { get; set; }
    public int? IdCliente { get; set; }
    public string Placa { get; set; } = null!;
    public virtual Tarifa Tarifa { get; set; } = null!;
    public virtual Cliente? Cliente { get; set; }
    [JsonIgnore]
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    [JsonIgnore]
    public virtual ICollection<ContratoAbonado> ContratosAbonados { get; set; } = new List<ContratoAbonado>();
}
