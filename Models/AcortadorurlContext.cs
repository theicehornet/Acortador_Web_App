using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Acortador_Web_App.Models;

public partial class AcortadorurlContext : DbContext
{
    public AcortadorurlContext()
    {
    }

    public AcortadorurlContext(DbContextOptions<AcortadorurlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acortador> Acortadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acortador>(entity =>
        {
            entity.ToTable("ACORTADOR");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Link).HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
