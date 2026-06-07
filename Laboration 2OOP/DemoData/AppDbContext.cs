using Laboration_2OOP.Domän;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Laboration_2OOP.DemoData
{
    public class AppDbContext : DbContext
    {
        public DbSet<Medlem> Medlemmar { get; set; }
        public DbSet<Spel> Spel { get; set; }
        public DbSet<Spelträff> Träffar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=Laboration2OOPDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medlem>().HasKey(m => m.MedlemsId);
            modelBuilder.Entity<Spel>().HasKey(s => s.SpelId);
            modelBuilder.Entity<Spelträff>().HasKey(t => t.TräffId);
        }
    }
}


