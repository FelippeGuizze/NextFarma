using Microsoft.EntityFrameworkCore;
using NextFarma.Models;

namespace NextFarma.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Paciente> Pacientes { get; set; } = null!;
        public DbSet<Medicamento> Medicamentos { get; set; } = null!;
        public DbSet<CategoriaMedicamento> CategoriasMedicamento { get; set; } = null!;
        public DbSet<Prontuario> Prontuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garante que o Enum seja salvo como texto ou int corretamente se preferir
            modelBuilder.Entity<Prontuario>()
                .Property(p => p.TipoDeUso)
                .HasConversion<string>(); // Salva "Continuo" no banco ao invés de 0

            modelBuilder.Entity<Paciente>()
                .Property(p => p.Sexo)
                .HasConversion<string>();
        }
    }
}