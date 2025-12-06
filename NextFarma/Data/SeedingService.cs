using NextFarma.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

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
            // Ensure database created
            _context.Database.EnsureCreated();

            // 0) Admin user: create if missing
            if (!_context.Usuarios.Any())
            {
                Usuario admin = new Usuario
                {
                    Email = "admin@nextfarma.com",
                    Senha = "1234",
                    BirthDate = new DateTime(1990, 1, 1),
                    Type = PeopleType.Adm
                };

                var hasher = new PasswordHasher<Usuario>();
                admin.Senha = hasher.HashPassword(admin, admin.Senha);

                _context.Usuarios.Add(admin);
                _context.SaveChanges();
            }

            // 1) Categorias
            if (!_context.CategoriasMedicamento.Any())
            {
                var categorias = new CategoriaMedicamento[]
                {
                    new CategoriaMedicamento { Lista = "A1", Descricao = "Entorpecentes", TipoReceituario = "Notificação de Receita A", CorReceita = "Amarela" },
                    new CategoriaMedicamento { Lista = "A2", Descricao = "Entorpecentes", TipoReceituario = "Notificação de Receita A", CorReceita = "Amarela" },
                    new CategoriaMedicamento { Lista = "A3", Descricao = "Psicotrópicos", TipoReceituario = "Notificação de Receita A", CorReceita = "Amarela" },
                    new CategoriaMedicamento { Lista = "B1", Descricao = "Psicotrópicos", TipoReceituario = "Receita de Controle Especial em duas vias", CorReceita = "Branca" },
                    new CategoriaMedicamento { Lista = "B2", Descricao = "Psicotrópicos Anorexígenos", TipoReceituario = "Notificação de Receita B2", CorReceita = "Azul" },
                    new CategoriaMedicamento { Lista = "C1", Descricao = "Outras Substâncias", TipoReceituario = "Receita de Controle Especial em duas vias", CorReceita = "Branca" },
                    new CategoriaMedicamento { Lista = "C5", Descricao = "Anabolizantes", TipoReceituario = "Receita de Controle Especial em duas vias", CorReceita = "Branca" }
                };
                _context.CategoriasMedicamento.AddRange(categorias);
                _context.SaveChanges();
            }

            // 2) Medicamentos
            if (!_context.Medicamentos.Any())
            {
                // reload categories
                var categorias = _context.CategoriasMedicamento.ToList();
                var medicamentos = new Medicamento[]
                {
                    new Medicamento { Nome = "Morfina", Concentracao = "10mg", CategoriaId = categorias.First(c => c.Lista == "A1").Id },
                    new Medicamento { Nome = "Metadona", Concentracao = "5mg", CategoriaId = categorias.First(c => c.Lista == "A1").Id },
                    new Medicamento { Nome = "Ritalina (Metilfenidato)", Concentracao = "10mg", CategoriaId = categorias.First(c => c.Lista == "A3").Id },
                    new Medicamento { Nome = "Zolpidem", Concentracao = "10mg", CategoriaId = categorias.First(c => c.Lista == "B1").Id },
                    new Medicamento { Nome = "Alprazolam", Concentracao = "0.5mg", CategoriaId = categorias.First(c => c.Lista == "B1").Id },
                    new Medicamento { Nome = "Sibutramina", Concentracao = "15mg", CategoriaId = categorias.First(c => c.Lista == "B2").Id },
                    new Medicamento { Nome = "Durateston", Concentracao = "250mg", CategoriaId = categorias.First(c => c.Lista == "C5").Id }
                };

                _context.Medicamentos.AddRange(medicamentos);
                _context.SaveChanges();
            }

            // 3) Pacientes
            if (!_context.Pacientes.Any())
            {
                var pacientes = new Paciente[]
                {
                    new Paciente { Nome = "João da Silva", Idade = 45, Endereco = "Rua das Flores, 123", CEP = "01001-000", RG = "12.345.678-9", Telefone = "(11) 99999-1111", Sexo = Sexo.Masculino },
                    new Paciente { Nome = "Maria Oliveira", Idade = 32, Endereco = "Av. Paulista, 2000", CEP = "01310-200", RG = "98.765.432-1", Telefone = "(11) 98888-2222", Sexo = Sexo.Feminino },
                    new Paciente { Nome = "Carlos Pereira", Idade = 60, Endereco = "Rua Augusta, 500", CEP = "01305-000", RG = "11.222.333-4", Telefone = "(11) 97777-3333", Sexo = Sexo.Masculino },
                    new Paciente { Nome = "Ana Santos", Idade = 25, Endereco = "Rua Vergueiro, 100", CEP = "04101-000", RG = "55.666.777-8", Telefone = "(11) 96666-4444", Sexo = Sexo.Feminino }
                };
                _context.Pacientes.AddRange(pacientes);
                _context.SaveChanges();
            }

            // 4) Prontuarios
            if (!_context.Prontuarios.Any())
            {
                var rand = new Random();
                var pacientes = _context.Pacientes.ToList();
                var medicamentos = _context.Medicamentos.ToList();

                if (pacientes.Any() && medicamentos.Any())
                {
                    var prontuarios = new Prontuario[]
                    {
                        new Prontuario
                        {
                            PacienteId = pacientes[0].Id,
                            MedicamentoId = medicamentos.First(m => m.Nome.Contains("Zolpidem")).Id,
                            CID = "F51.0",
                            NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                            DataEmissao = DateTime.Now.AddDays(-10),
                            TipoDeUso = TipoUso.Uso30Dias
                        },
                        new Prontuario
                        {
                            PacienteId = pacientes[1].Id,
                            MedicamentoId = medicamentos.First(m => m.Nome.Contains("Sibutramina")).Id,
                            CID = "E66",
                            NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                            DataEmissao = DateTime.Now.AddDays(-5),
                            TipoDeUso = TipoUso.Continuo
                        },
                        new Prontuario
                        {
                            PacienteId = pacientes[2].Id,
                            MedicamentoId = medicamentos.First(m => m.Nome.Contains("Morfina")).Id,
                            CID = "R52",
                            NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                            DataEmissao = DateTime.Now.Date,
                            TipoDeUso = TipoUso.Uso10Dias
                        }
                    };
                    _context.Prontuarios.AddRange(prontuarios);
                    _context.SaveChanges();
                }
            }
        }
    }
}