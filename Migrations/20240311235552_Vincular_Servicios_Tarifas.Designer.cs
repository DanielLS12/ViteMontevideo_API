﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ViteMontevideo_API.models;

#nullable disable

namespace ViteMontevideo_API.Migrations
{
    [DbContext(typeof(EstacionamientoContext))]
    [Migration("20240311235552_Vincular_Servicios_Tarifas")]
    partial class Vincular_Servicios_Tarifas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ViteMontevideo_API.models.Actividad", b =>
                {
                    b.Property<short>("IdActividad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id_actividad");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("IdActividad"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdActividad")
                        .HasName("PK__Activida__DCD34883834EFE8C");

                    b.ToTable("Actividades");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.CajaChica", b =>
                {
                    b.Property<int>("IdCaja")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_caja");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCaja"));

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime?>("FechaFinal")
                        .HasColumnType("date")
                        .HasColumnName("fecha_final");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("date")
                        .HasColumnName("fecha_inicio");

                    b.Property<TimeSpan?>("HoraFinal")
                        .HasColumnType("time")
                        .HasColumnName("hora_final");

                    b.Property<TimeSpan>("HoraInicio")
                        .HasColumnType("time")
                        .HasColumnName("hora_inicio");

                    b.Property<short>("IdTrabajador")
                        .HasColumnType("smallint")
                        .HasColumnName("id_trabajador");

                    b.Property<string>("Observacion")
                        .HasMaxLength(600)
                        .IsUnicode(false)
                        .HasColumnType("varchar(600)")
                        .HasColumnName("observacion");

                    b.Property<decimal>("SaldoInicial")
                        .HasColumnType("smallmoney")
                        .HasColumnName("saldo_inicial");

                    b.Property<string>("Turno")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("turno");

                    b.HasKey("IdCaja")
                        .HasName("PK__Cajas_Ch__C71E2476BD240E64");

                    b.HasIndex("IdTrabajador");

                    b.ToTable("Cajas_Chicas", (string)null);
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Cargo", b =>
                {
                    b.Property<byte>("IdCargo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasColumnName("id_cargo");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("IdCargo"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdCargo")
                        .HasName("PK__Cargos__D3C09EC5AF4FBE96");

                    b.ToTable("Cargos");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Categoria", b =>
                {
                    b.Property<short>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id_categoria");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("IdCategoria"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdCategoria")
                        .HasName("PK__Categori__CD54BC5A4FD51AE2");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.ContratosAbonado", b =>
                {
                    b.Property<int>("IdContratoAbonado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_contrato_abonado");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContratoAbonado"));

                    b.Property<bool>("EstadoPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("estado_pago")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("FechaFinal")
                        .HasColumnType("date")
                        .HasColumnName("fecha_final");

                    b.Property<DateTime?>("FechaInicio")
                        .HasColumnType("date")
                        .HasColumnName("fecha_inicio");

                    b.Property<DateTime?>("FechaPago")
                        .HasColumnType("date")
                        .HasColumnName("fecha_pago");

                    b.Property<TimeSpan?>("HoraFinal")
                        .HasColumnType("time")
                        .HasColumnName("hora_final");

                    b.Property<TimeSpan?>("HoraInicio")
                        .HasColumnType("time")
                        .HasColumnName("hora_inicio");

                    b.Property<TimeSpan?>("HoraPago")
                        .HasColumnType("time")
                        .HasColumnName("hora_pago");

                    b.Property<int?>("IdCaja")
                        .HasColumnType("int")
                        .HasColumnName("id_caja");

                    b.Property<int?>("IdVehiculo")
                        .HasColumnType("int")
                        .HasColumnName("id_vehiculo");

                    b.Property<decimal>("Monto")
                        .HasColumnType("money")
                        .HasColumnName("monto");

                    b.HasKey("IdContratoAbonado")
                        .HasName("PK__Contrato__6E37623D12EBB370");

                    b.HasIndex("IdCaja");

                    b.HasIndex("IdVehiculo");

                    b.ToTable("Contratos_Abonados", (string)null);
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Egreso", b =>
                {
                    b.Property<int>("IdEgreso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_egreso");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEgreso"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time")
                        .HasColumnName("hora");

                    b.Property<int?>("IdCaja")
                        .HasColumnType("int")
                        .HasColumnName("id_caja");

                    b.Property<decimal>("Monto")
                        .HasColumnType("money")
                        .HasColumnName("monto");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("motivo");

                    b.HasKey("IdEgreso")
                        .HasName("PK__Egresos__6B30851B5799EDD1");

                    b.HasIndex("IdCaja");

                    b.ToTable("Egresos");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Servicio", b =>
                {
                    b.Property<int>("IdServicio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_servicio");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdServicio"));

                    b.Property<decimal>("Descuento")
                        .HasColumnType("money")
                        .HasColumnName("descuento");

                    b.Property<bool>("EstadoPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("estado_pago")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime>("FechaEntrada")
                        .HasColumnType("date")
                        .HasColumnName("fecha_entrada");

                    b.Property<DateTime?>("FechaSalida")
                        .HasColumnType("date")
                        .HasColumnName("fecha_salida");

                    b.Property<TimeSpan>("HoraEntrada")
                        .HasColumnType("time")
                        .HasColumnName("hora_entrada");

                    b.Property<TimeSpan?>("HoraSalida")
                        .HasColumnType("time")
                        .HasColumnName("hora_salida");

                    b.Property<int?>("IdCaja")
                        .HasColumnType("int")
                        .HasColumnName("id_caja");

                    b.Property<short?>("IdTarifa")
                        .HasColumnType("smallint")
                        .HasColumnName("id_tarifa");

                    b.Property<int>("IdVehiculo")
                        .HasColumnType("int")
                        .HasColumnName("id_vehiculo");

                    b.Property<decimal>("Monto")
                        .HasColumnType("money")
                        .HasColumnName("monto");

                    b.Property<string>("Observacion")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)")
                        .HasColumnName("observacion");

                    b.Property<string>("TipoPago")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("tipo_pago");

                    b.HasKey("IdServicio")
                        .HasName("PK__Servicio__6FD07FDC9CAEE091");

                    b.HasIndex("IdCaja");

                    b.HasIndex("IdTarifa");

                    b.HasIndex("IdVehiculo");

                    b.ToTable("Servicios");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Tarifa", b =>
                {
                    b.Property<short>("IdTarifa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id_tarifa");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("IdTarifa"));

                    b.Property<TimeSpan?>("HoraDia")
                        .HasColumnType("time")
                        .HasColumnName("hora_dia");

                    b.Property<TimeSpan?>("HoraNoche")
                        .HasColumnType("time")
                        .HasColumnName("hora_noche");

                    b.Property<short>("IdActividad")
                        .HasColumnType("smallint")
                        .HasColumnName("id_actividad");

                    b.Property<short>("IdCategoria")
                        .HasColumnType("smallint")
                        .HasColumnName("id_categoria");

                    b.Property<decimal>("PrecioDia")
                        .HasColumnType("smallmoney")
                        .HasColumnName("precio_dia");

                    b.Property<decimal>("PrecioNoche")
                        .HasColumnType("smallmoney")
                        .HasColumnName("precio_noche");

                    b.Property<TimeSpan>("Tolerancia")
                        .HasColumnType("time")
                        .HasColumnName("tolerancia");

                    b.HasKey("IdTarifa")
                        .HasName("PK__Tarifas__747D038947649B04");

                    b.HasIndex("IdActividad");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Tarifas");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Trabajador", b =>
                {
                    b.Property<short>("IdTrabajador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id_trabajador");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("IdTrabajador"));

                    b.Property<string>("ApellidoMaterno")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("apellido_materno");

                    b.Property<string>("ApellidoPaterno")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("apellido_paterno");

                    b.Property<string>("Correo")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("correo");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasMaxLength(8)
                        .IsUnicode(false)
                        .HasColumnType("char(8)")
                        .HasColumnName("dni")
                        .IsFixedLength();

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("((1))");

                    b.Property<byte?>("IdCargo")
                        .HasColumnType("tinyint")
                        .HasColumnName("id_cargo");

                    b.Property<short?>("IdUsuario")
                        .HasColumnType("smallint")
                        .HasColumnName("id_usuario");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.Property<string>("Telefono")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("telefono");

                    b.HasKey("IdTrabajador")
                        .HasName("PK__Trabajad__767D20B21C672845");

                    b.HasIndex("IdCargo");

                    b.HasIndex("IdUsuario");

                    b.HasIndex(new[] { "Telefono" }, "UQ__Trabajad__2A16D94509C064B4");

                    b.HasIndex(new[] { "Correo" }, "UQ__Trabajad__2A586E0BFAA6BA1B");

                    b.HasIndex(new[] { "Dni" }, "UQ__Trabajad__D87608A7B28B04C7")
                        .IsUnique();

                    b.ToTable("Trabajadores");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Usuario", b =>
                {
                    b.Property<short>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasColumnName("id_usuario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("IdUsuario"));

                    b.Property<byte[]>("Clave")
                        .IsRequired()
                        .HasMaxLength(8000)
                        .HasColumnType("varbinary(8000)")
                        .HasColumnName("clave");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("nombre");

                    b.HasKey("IdUsuario")
                        .HasName("PK__Usuarios__4E3E04ADB8386F2A");

                    b.HasIndex(new[] { "Nombre" }, "UQ__Usuarios__72AFBCC6F3526FE2")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Vehiculo", b =>
                {
                    b.Property<int>("IdVehiculo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_vehiculo");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdVehiculo"));

                    b.Property<short?>("IdTarifa")
                        .HasColumnType("smallint")
                        .HasColumnName("id_tarifa");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("char(6)")
                        .HasColumnName("placa")
                        .IsFixedLength();

                    b.HasKey("IdVehiculo")
                        .HasName("PK__Vehiculo__F5DC0F3958169D5F");

                    b.HasIndex("IdTarifa");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.CajaChica", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.Trabajador", "trabajador")
                        .WithMany("CajasChicas")
                        .HasForeignKey("IdTrabajador")
                        .IsRequired()
                        .HasConstraintName("Fk_Cajas_Chicas_Trabajadores");

                    b.Navigation("trabajador");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.ContratosAbonado", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.CajaChica", "oCajaChica")
                        .WithMany("ContratosAbonados")
                        .HasForeignKey("IdCaja")
                        .HasConstraintName("Fk_Contratos_Abonados_Cajas_Chicas");

                    b.HasOne("ViteMontevideo_API.models.Vehiculo", "oVehiculo")
                        .WithMany("ContratosAbonados")
                        .HasForeignKey("IdVehiculo")
                        .HasConstraintName("Fk_Contratos_Abonados_Vehiculos");

                    b.Navigation("oCajaChica");

                    b.Navigation("oVehiculo");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Egreso", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.CajaChica", "oCajaChica")
                        .WithMany("Egresos")
                        .HasForeignKey("IdCaja")
                        .HasConstraintName("Fk_Egresos_Cajas_Chicas");

                    b.Navigation("oCajaChica");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Servicio", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.CajaChica", "oCajaChica")
                        .WithMany("Servicios")
                        .HasForeignKey("IdCaja")
                        .HasConstraintName("Fk_Servicios_Cajas_Chicas");

                    b.HasOne("ViteMontevideo_API.models.Tarifa", "oTarifa")
                        .WithMany("Servicios")
                        .HasForeignKey("IdTarifa")
                        .HasConstraintName("Fk_Servicios_Tarifas");

                    b.HasOne("ViteMontevideo_API.models.Vehiculo", "oVehiculo")
                        .WithMany("Servicios")
                        .HasForeignKey("IdVehiculo")
                        .IsRequired()
                        .HasConstraintName("Fk_Servicios_Vehiculos");

                    b.Navigation("oCajaChica");

                    b.Navigation("oTarifa");

                    b.Navigation("oVehiculo");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Tarifa", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.Actividad", "oActividad")
                        .WithMany("Tarifas")
                        .HasForeignKey("IdActividad")
                        .IsRequired()
                        .HasConstraintName("Fk_Tarifas_Actividades");

                    b.HasOne("ViteMontevideo_API.models.Categoria", "oCategoria")
                        .WithMany("Tarifas")
                        .HasForeignKey("IdCategoria")
                        .IsRequired()
                        .HasConstraintName("Fk_Tarifas_Categoria");

                    b.Navigation("oActividad");

                    b.Navigation("oCategoria");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Trabajador", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.Cargo", "oCargo")
                        .WithMany("Trabajadores")
                        .HasForeignKey("IdCargo")
                        .HasConstraintName("Fk_Trabajadores_Cargos");

                    b.HasOne("ViteMontevideo_API.models.Usuario", "oUsuario")
                        .WithMany("Trabajadores")
                        .HasForeignKey("IdUsuario")
                        .HasConstraintName("Fk_Trabajadores_Usuarios");

                    b.Navigation("oCargo");

                    b.Navigation("oUsuario");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Vehiculo", b =>
                {
                    b.HasOne("ViteMontevideo_API.models.Tarifa", "oTarifa")
                        .WithMany("Vehiculos")
                        .HasForeignKey("IdTarifa")
                        .HasConstraintName("Fk_Vehiculos_Tarifas");

                    b.Navigation("oTarifa");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Actividad", b =>
                {
                    b.Navigation("Tarifas");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.CajaChica", b =>
                {
                    b.Navigation("ContratosAbonados");

                    b.Navigation("Egresos");

                    b.Navigation("Servicios");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Cargo", b =>
                {
                    b.Navigation("Trabajadores");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Categoria", b =>
                {
                    b.Navigation("Tarifas");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Tarifa", b =>
                {
                    b.Navigation("Servicios");

                    b.Navigation("Vehiculos");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Trabajador", b =>
                {
                    b.Navigation("CajasChicas");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Usuario", b =>
                {
                    b.Navigation("Trabajadores");
                });

            modelBuilder.Entity("ViteMontevideo_API.models.Vehiculo", b =>
                {
                    b.Navigation("ContratosAbonados");

                    b.Navigation("Servicios");
                });
#pragma warning restore 612, 618
        }
    }
}
