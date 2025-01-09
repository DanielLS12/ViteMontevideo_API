using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Context;

public partial class EstacionamientoContext : DbContext
{
    public EstacionamientoContext()
    {
    }

    public EstacionamientoContext(DbContextOptions<EstacionamientoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actividad> Actividades { get; set; }

    public virtual DbSet<CajaChica> CajasChicas { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ComercioAdicional> ComerciosAdicionales { get; set; }

    public virtual DbSet<ContratoAbonado> ContratosAbonados { get; set; }

    public virtual DbSet<Egreso> Egresos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Tarifa> Tarifas { get; set; }

    public virtual DbSet<Trabajador> Trabajadores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasKey(e => e.IdActividad).HasName("PK__Activida__DCD34883834EFE8C");

            entity.Property(e => e.IdActividad).HasColumnName("id_actividad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<CajaChica>(entity =>
        {
            entity.HasKey(e => e.IdCaja).HasName("PK__Cajas_Ch__C71E2476BD240E64");

            entity.ToTable("Cajas_Chicas");

            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.Turno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("turno");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaFinal)
                .HasColumnType("date")
                .HasColumnName("fecha_final");
            entity.Property(e => e.HoraFinal).HasColumnName("hora_final");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdTrabajador).HasColumnName("id_trabajador");
            entity.Property(e => e.Observacion)
                .HasMaxLength(600)
                .IsUnicode(false)
                .HasColumnName("observacion");
            entity.Property(e => e.SaldoInicial)
                .HasColumnType("smallmoney")
                .HasColumnName("saldo_inicial");

            entity.Property(e => e.TotalMontoServicios)
                .HasColumnType("money")
                .HasColumnName("total_monto_servicios");

            entity.Property(e => e.TotalMontoAbonados)
                .HasColumnType("money")
                .HasColumnName("total_monto_abonados");

            entity.Property(e => e.TotalMontoComerciosAdicionales)
                .HasColumnType("money")
                .HasColumnName("total_monto_comercios_adicionales");

            entity.Property(e => e.TotalMontoEgresos)
                .HasColumnType("money")
                .HasColumnName("total_monto_egresos");

            entity.HasOne(d => d.Trabajador).WithMany(e => e.CajasChicas)
                .HasForeignKey(e => e.IdTrabajador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Cajas_Chicas_Trabajadores");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargos__D3C09EC5AF4FBE96");

            entity.Property(e => e.IdCargo)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_cargo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A4FD51AE2");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__WD54B5TA4FD54BE5");

            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombres")
                .IsRequired(true);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidos")
                .IsRequired(true);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono")
                .IsRequired(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo")
                .IsRequired(false);
        });

        modelBuilder.Entity<ComercioAdicional>(entity =>
        {
            entity.HasKey(e => e.IdComercioAdicional).HasName("PK__ComercioAdicinal__4202OZRAM3021M");

            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.TipoComercioAdicional)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_comercio_adicional")
                .IsRequired(true);
            entity.Property(e => e.Monto)
                .HasColumnType("money")
                .HasColumnName("monto")
                .IsRequired(true);
            entity.Property(e => e.FechaPago)
                .HasColumnType("date")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.HoraPago).HasColumnName("hora_pago");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_pago");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("observacion");

            entity.HasOne(e => e.Cliente).WithMany(e => e.ComerciosAdicinales)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Fk_ComerciosAdicionales_Clientes");

            entity.HasOne(e => e.CajaChica).WithMany(e => e.ComerciosAdicionales)
                .HasForeignKey(e => e.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_ComerciosAdicionales_CajasChicas");
        });

        modelBuilder.Entity<ComercioAdicional>()
            .ToTable("Comercios_Adicionales", ca => ca.HasTrigger("after_update_recalcular_comercios_adicionales"));

        modelBuilder.Entity<ComercioAdicional>()
            .ToTable("Comercios_Adicionales", ca => ca.HasTrigger("after_delete_recalcular_comercios_adicionales"));

        modelBuilder.Entity<ContratoAbonado>(entity =>
        {
            entity.HasKey(e => e.IdContratoAbonado).HasName("PK__Contrato__6E37623D12EBB370");

            entity.ToTable("Contratos_Abonados");

            entity.Property(e => e.IdContratoAbonado).HasColumnName("id_contrato_abonado");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.EstadoPago)
                .HasDefaultValueSql("((0))")
                .HasColumnName("estado_pago")
                .IsRequired();
            entity.Property(e => e.FechaFinal)
                .HasColumnType("date")
                .HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaPago)
                .HasColumnType("date")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.HoraPago)
                .HasColumnType("time")
                .HasColumnName("hora_pago");
            entity.Property(e => e.HoraFinal).HasColumnName("hora_final");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");
            entity.Property(e => e.Monto)
                .HasColumnType("money")
                .HasColumnName("monto");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_pago");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("observacion");

            entity.HasOne(d => d.Vehiculo).WithMany(e => e.ContratosAbonados)
                .HasForeignKey(e => e.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Contratos_Abonados_Vehiculos");

            entity.HasOne(e => e.CajaChica).WithMany(e => e.ContratosAbonados)
                .HasForeignKey(e => e.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Contratos_Abonados_Cajas_Chicas");
        });

        modelBuilder.Entity<ContratoAbonado>()
            .ToTable("Contratos_Abonados", ca => ca.HasTrigger("after_update_recalcular_abonados"));

        modelBuilder.Entity<ContratoAbonado>()
            .ToTable("Contratos_Abonados", ca => ca.HasTrigger("after_delete_recalcular_abonados"));

        modelBuilder.Entity<Egreso>(entity =>
        {
            entity.HasKey(e => e.IdEgreso).HasName("PK__Egresos__6B30851B5799EDD1");

            entity.Property(e => e.IdEgreso).HasColumnName("id_egreso");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Monto)
                .HasColumnType("money")
                .HasColumnName("monto");
            entity.Property(e => e.Motivo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("motivo");

            entity.HasOne(e => e.CajaChica).WithMany(e => e.Egresos)
                .HasForeignKey(e => e.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Egresos_Cajas_Chicas");
        });

        modelBuilder.Entity<Egreso>()
            .ToTable("Egresos", s => s.HasTrigger("after_insert_recalcular_egresos"));

        modelBuilder.Entity<Egreso>()
            .ToTable("Egresos", s => s.HasTrigger("after_update_recalcular_egresos"));

        modelBuilder.Entity<Egreso>()
            .ToTable("Egresos", s => s.HasTrigger("after_delete_recalcular_egresos"));
         
        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__6FD07FDC9CAEE091");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");
            entity.Property(e => e.Descuento)
                .HasColumnType("money")
                .HasColumnName("descuento");
            entity.Property(e => e.EstadoPago)
                .IsRequired()
                .HasDefaultValueSql("((0))")
                .HasColumnName("estado_pago");
            entity.Property(e => e.FechaEntrada)
                .HasColumnType("date")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("date")
                .HasColumnName("fecha_salida");
            entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");
            entity.Property(e => e.HoraSalida).HasColumnName("hora_salida");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_pago");
            entity.Property(e => e.Monto)
                .HasColumnType("money")
                .HasColumnName("monto");
            entity.Property(e => e.Observacion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("observacion");

            entity.HasOne(e => e.CajaChica).WithMany(e => e.Servicios)
                .HasForeignKey(d => d.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Servicios_Cajas_Chicas");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Servicios_Vehiculos");

            entity.HasOne(d => d.Tarifa).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTarifa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Servicios_Tarifas");
        });

        modelBuilder.Entity<Servicio>()
            .ToTable("Servicios", s => s.HasTrigger("after_update_recalcular_servicios"));

        modelBuilder.Entity<Servicio>()
            .ToTable("Servicios", s => s.HasTrigger("after_delete_recalcular_servicios"));

        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.HasKey(e => e.IdTarifa).HasName("PK__Tarifas__747D038947649B04");

            entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");
            entity.Property(e => e.IdActividad).HasColumnName("id_actividad");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.PrecioDia)
                .HasColumnType("smallmoney")
                .HasColumnName("precio_dia");
            entity.Property(e => e.PrecioNoche)
                .HasColumnType("smallmoney")
                .HasColumnName("precio_noche");
            entity.Property(e => e.Tolerancia).HasColumnName("tolerancia");

            entity.HasOne(d => d.Actividad).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Tarifas_Actividades");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Tarifas_Categoria");
        });

        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__Trabajad__767D20B21C672845");

            entity.HasIndex(e => e.Dni, "UQ__Trabajad__D87608A7B28B04C7").IsUnique();

            entity.Property(e => e.IdTrabajador).HasColumnName("id_trabajador");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido_materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido_paterno");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dni");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.IdCargo).HasColumnName("id_cargo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Cargo).WithMany(p => p.Trabajadores)
                .HasForeignKey(d => d.IdCargo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Trabajadores_Cargos");

            entity.HasOne(d => d.OUsuario).WithMany(p => p.Trabajadores)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("Fk_Trabajadores_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__4E3E04ADB8386F2A");

            entity.HasIndex(e => e.Nombre, "UQ__Usuarios__72AFBCC6F3526FE2").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(8000)
                .HasColumnName("clave");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__F5DC0F3958169D5F");

            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");
            entity.Property(e => e.IdTarifa).HasColumnName("id_tarifa");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Placa)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("placa");

            entity.HasOne(e => e.Tarifa).WithMany(p => p.Vehiculos)
                .HasForeignKey(e => e.IdTarifa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Vehiculos_Tarifas");

            entity.HasOne(e => e.Cliente).WithMany(p => p.Vehiculos)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Vehiculos_Clientes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
