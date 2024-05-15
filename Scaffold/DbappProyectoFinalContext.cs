using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class DbappProyectoFinalContext : DbContext
{
    public DbappProyectoFinalContext(DbContextOptions<DbappProyectoFinalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Centroeducativo> Centroeducativos { get; set; }

    public virtual DbSet<Correoelectronico> Correoelectronicos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuarioscentroeducativo> Usuarioscentroeducativos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.IdAdministrador).HasName("PRIMARY");

            entity.ToTable("administrador");

            entity.Property(e => e.IdAdministrador)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasColumnName("DNI");

            entity.HasOne(d => d.IdAdministradorNavigation).WithOne(p => p.Administrador)
                .HasForeignKey<Administrador>(d => d.IdAdministrador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("administrador_ibfk_1");
        });

        modelBuilder.Entity<Centroeducativo>(entity =>
        {
            entity.HasKey(e => e.IdCentro).HasName("PRIMARY");

            entity.ToTable("centroeducativo");

            entity.HasIndex(e => e.IdAdministrador, "IdAdministrador");

            entity.HasIndex(e => new { e.NombreCentro, e.Direccion }, "NombreCentro").IsUnique();

            entity.Property(e => e.IdCentro).HasColumnType("int(11)");
            entity.Property(e => e.Direccion).HasDefaultValueSql("''");
            entity.Property(e => e.IdAdministrador).HasColumnType("int(11)");
            entity.Property(e => e.NombreCentro)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");

            entity.HasOne(d => d.IdAdministradorNavigation).WithMany(p => p.Centroeducativos)
                .HasForeignKey(d => d.IdAdministrador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("centroeducativo_ibfk_1");
        });

        modelBuilder.Entity<Correoelectronico>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PRIMARY");

            entity.ToTable("correoelectronico");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");
            entity.Property(e => e.ApellidosUsuario)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ALUMNO'");

            entity.HasOne(d => d.EmailNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_ibfk_1");
        });

        modelBuilder.Entity<Usuarioscentroeducativo>(entity =>
        {
            entity.HasKey(e => e.IdUsuariosCentroEducativo).HasName("PRIMARY");

            entity.ToTable("usuarioscentroeducativo");

            entity.HasIndex(e => new { e.IdCentro, e.IdUsuario }, "IdCentro").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.IdUsuariosCentroEducativo).HasColumnType("int(11)");
            entity.Property(e => e.IdCentro).HasColumnType("int(11)");
            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");

            entity.HasOne(d => d.IdCentroNavigation).WithMany(p => p.Usuarioscentroeducativos)
                .HasForeignKey(d => d.IdCentro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarioscentroeducativo_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Usuarioscentroeducativos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarioscentroeducativo_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
