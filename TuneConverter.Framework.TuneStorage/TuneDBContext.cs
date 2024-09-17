using Emgu.CV.Ocl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace TuneConverter.Framework.TuneStorage;

internal class TuneDBContext : DbContext
{
    public DbSet<TuneRecord> TuneRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("TuneServer");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TuneRecord>()
            .Property(m => m.Name)
            .HasMaxLength(100); // Optional: set maximum length for Name

        modelBuilder.Entity<TuneRecord>()
            .Property(m => m.Type)
            .HasMaxLength(50); // Optional: set maximum length for Type

        modelBuilder.Entity<TuneRecord>()
            .Property(m => m.Key)
            .HasMaxLength(50); // Optional: set maximum length for Key

        modelBuilder.Entity<TuneRecord>()
            .Property(m => m.Tune) ; // Optional: set maximum length for Tune
    }
}
