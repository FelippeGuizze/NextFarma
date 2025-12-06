using NextFarma.Models;

namespace NextFarma.Data
{
    public class SeedingService
    {
        private readonly AppDbContext _context;

        public SeedingService(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Se já existir algum usuário, não faz nada
            if (_context.Usuarios.Any())
            {
                return;
            }

            // Cria o admin padrão
            Usuario admin = new Usuario
            {
                Email = "admin@nextfarma.com",
                Senha = "1234",
                BirthDate = new DateTime(1990, 1, 1),
                Type = PeopleType.Adm
            };

            _context.Usuarios.Add(admin);
            _context.SaveChanges();
        }
    }
}