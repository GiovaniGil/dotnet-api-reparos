using Microsoft.EntityFrameworkCore;
using APIReparos.Models;

namespace APIReparos.Context
{
    public class ReparosContext : DbContext
    {
        public ReparosContext(DbContextOptions<ReparosContext> options)
          : base(options)
        { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Reparo> Reparos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Usuario>()
                .HasAlternateKey(c => c.NomeUsuario)
                .HasName("AlternateKey_NomeUsuario");
        }
    }
}