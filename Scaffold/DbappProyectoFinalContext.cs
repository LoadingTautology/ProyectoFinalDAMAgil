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

    public virtual DbSet<Asignatura> Asignaturas { get; set; }

    public virtual DbSet<Asignaturascicloformativo> Asignaturascicloformativos { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Centroeducativo> Centroeducativos { get; set; }

    public virtual DbSet<Cicloformativo> Cicloformativos { get; set; }

    public virtual DbSet<Correoelectronico> Correoelectronicos { get; set; }

    public virtual DbSet<Diasemana> Diasemanas { get; set; }

    public virtual DbSet<Franjahorarium> Franjahoraria { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuarioscentroeducativo> Usuarioscentroeducativos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

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
                .HasConstraintName("administrador_ibfk_1");
        });

        modelBuilder.Entity<Asignatura>(entity =>
        {
            entity.HasKey(e => e.IdAsignatura).HasName("PRIMARY");

            entity.ToTable("asignatura");

            entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");
            entity.Property(e => e.Curso).HasColumnType("int(11)");
            entity.Property(e => e.NombreAsignatura)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Asignatura'");
        });

        modelBuilder.Entity<Asignaturascicloformativo>(entity =>
        {
            entity.HasKey(e => e.IdAsignaturasCicloFormativo).HasName("PRIMARY");

            entity.ToTable("asignaturascicloformativo");

            entity.HasIndex(e => new { e.IdAsignatura, e.IdCiclo }, "IdAsignatura").IsUnique();

            entity.HasIndex(e => e.IdCiclo, "IdCiclo");

            entity.Property(e => e.IdAsignaturasCicloFormativo).HasColumnType("int(11)");
            entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");
            entity.Property(e => e.IdCiclo).HasColumnType("int(11)");

            entity.HasOne(d => d.IdAsignaturaNavigation).WithMany(p => p.Asignaturascicloformativos)
                .HasForeignKey(d => d.IdAsignatura)
                .HasConstraintName("asignaturascicloformativo_ibfk_2");

            entity.HasOne(d => d.IdCicloNavigation).WithMany(p => p.Asignaturascicloformativos)
                .HasForeignKey(d => d.IdCiclo)
                .HasConstraintName("asignaturascicloformativo_ibfk_1");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.IdAula).HasName("PRIMARY");

            entity.ToTable("aula");

            entity.Property(e => e.IdAula).HasColumnType("int(11)");
            entity.Property(e => e.AforoMax).HasColumnType("int(11)");
            entity.Property(e => e.NombreAula)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Aula'");
            entity.Property(e => e.NumeroAula).HasColumnType("int(11)");
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
                .HasConstraintName("centroeducativo_ibfk_1");
        });

        modelBuilder.Entity<Cicloformativo>(entity =>
        {
            entity.HasKey(e => e.IdCiclo).HasName("PRIMARY");

            entity.ToTable("cicloformativo");

            entity.HasIndex(e => e.IdCentro, "IdCentro");

            entity.Property(e => e.IdCiclo).HasColumnType("int(11)");
            entity.Property(e => e.Acronimo)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Ciclo'");
            entity.Property(e => e.IdCentro).HasColumnType("int(11)");
            entity.Property(e => e.NombreCiclo)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Ciclo'");

            entity.HasOne(d => d.IdCentroNavigation).WithMany(p => p.Cicloformativos)
                .HasForeignKey(d => d.IdCentro)
                .HasConstraintName("cicloformativo_ibfk_1");
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

        modelBuilder.Entity<Diasemana>(entity =>
        {
            entity.HasKey(e => e.Dia).HasName("PRIMARY");

            entity.ToTable("diasemana");

            entity.Property(e => e.Dia).HasMaxLength(10);
        });

        modelBuilder.Entity<Franjahorarium>(entity =>
        {
            entity.HasKey(e => new { e.HoraMinInicio, e.HoraMinFinal })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("franjahoraria");

            entity.Property(e => e.HoraMinInicio).HasColumnType("time");
            entity.Property(e => e.HoraMinFinal).HasColumnType("time");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PRIMARY");

            entity.ToTable("horario");

            entity.HasIndex(e => e.Dia, "Dia");

            entity.HasIndex(e => new { e.HoraMinInicio, e.HoraMinFinal }, "HoraMinInicio");

            entity.HasIndex(e => new { e.IdAsignatura, e.IdCiclo }, "IdAsignatura");

            entity.HasIndex(e => new { e.IdAula, e.HoraMinInicio, e.HoraMinFinal, e.Dia }, "IdAula").IsUnique();

            entity.Property(e => e.IdHorario).HasColumnType("int(11)");
            entity.Property(e => e.Dia).HasMaxLength(10);
            entity.Property(e => e.HoraMinFinal).HasColumnType("time");
            entity.Property(e => e.HoraMinInicio).HasColumnType("time");
            entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");
            entity.Property(e => e.IdAula).HasColumnType("int(11)");
            entity.Property(e => e.IdCiclo).HasColumnType("int(11)");

            entity.HasOne(d => d.DiaNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.Dia)
                .HasConstraintName("horario_ibfk_4");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdAula)
                .HasConstraintName("horario_ibfk_2");

            entity.HasOne(d => d.Franjahorarium).WithMany(p => p.Horarios)
                .HasForeignKey(d => new { d.HoraMinInicio, d.HoraMinFinal })
                .HasConstraintName("horario_ibfk_3");

            entity.HasOne(d => d.Asignaturascicloformativo).WithMany(p => p.Horarios)
                .HasPrincipalKey(p => new { p.IdAsignatura, p.IdCiclo })
                .HasForeignKey(d => new { d.IdAsignatura, d.IdCiclo })
                .HasConstraintName("horario_ibfk_1");
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
                .HasConstraintName("usuarioscentroeducativo_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Usuarioscentroeducativos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("usuarioscentroeducativo_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
