using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Models;

public partial class ShorturlContext : DbContext
{
    public ShorturlContext()
    {
    }

    public ShorturlContext(DbContextOptions<ShorturlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acortador> Acortadors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acortador>(entity =>
        {
            entity.ToTable("ACORTADOR");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Lasttime)
                .HasColumnType("datetime")
                .HasColumnName("lasttime");
            entity.Property(e => e.Link).HasColumnType("text");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Acortadors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserId");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuario");

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
