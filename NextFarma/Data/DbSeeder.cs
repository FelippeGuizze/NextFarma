using NextFarma.Models;
using System;
using System.Linq;

namespace NextFarma.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Verifica se o banco já tem dados
            if (context.CategoriasMedicamento.Any())
            {
                return; // O banco já foi populado
            }

            // 1. Criar Categorias (Regras de Negócio Exatas)
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
            context.CategoriasMedicamento.AddRange(categorias);
            context.SaveChanges();

            // 2. Criar Medicamentos vinculados às categorias
            var medicamentos = new Medicamento[]
            {
                new Medicamento { Nome = "Morfina", Concentracao = "10mg", Categoria = categorias.First(c => c.Lista == "A1") },
                new Medicamento { Nome = "Metadona", Concentracao = "5mg", Categoria = categorias.First(c => c.Lista == "A1") },
                new Medicamento { Nome = "Ritalina (Metilfenidato)", Concentracao = "10mg", Categoria = categorias.First(c => c.Lista == "A3") },
                new Medicamento { Nome = "Zolpidem", Concentracao = "10mg", Categoria = categorias.First(c => c.Lista == "B1") },
                new Medicamento { Nome = "Alprazolam", Concentracao = "0.5mg", Categoria = categorias.First(c => c.Lista == "B1") },
                new Medicamento { Nome = "Sibutramina", Concentracao = "15mg", Categoria = categorias.First(c => c.Lista == "B2") },
                new Medicamento { Nome = "Durateston", Concentracao = "250mg", Categoria = categorias.First(c => c.Lista == "C5") }
            };
            context.Medicamentos.AddRange(medicamentos);
            context.SaveChanges();

            // 3. Criar Pacientes Aleatórios
            var pacientes = new Paciente[]
            {
                new Paciente { Nome = "João da Silva", Idade = 45, Endereco = "Rua das Flores, 123", CEP = "01001-000", RG = "12.345.678-9", Telefone = "(11) 99999-1111", Sexo = Sexo.Masculino },
                new Paciente { Nome = "Maria Oliveira", Idade = 32, Endereco = "Av. Paulista, 2000", CEP = "01310-200", RG = "98.765.432-1", Telefone = "(11) 98888-2222", Sexo = Sexo.Feminino },
                new Paciente { Nome = "Carlos Pereira", Idade = 60, Endereco = "Rua Augusta, 500", CEP = "01305-000", RG = "11.222.333-4", Telefone = "(11) 97777-3333", Sexo = Sexo.Masculino },
                new Paciente { Nome = "Ana Santos", Idade = 25, Endereco = "Rua Vergueiro, 100", CEP = "04101-000", RG = "55.666.777-8", Telefone = "(11) 96666-4444", Sexo = Sexo.Feminino }
            };
            context.Pacientes.AddRange(pacientes);
            context.SaveChanges();

            // 4. Criar Prontuários (Histórico) simulando prescrições
            var rand = new Random();
            var prontuarios = new Prontuario[]
            {
                new Prontuario 
                { 
                    Paciente = pacientes[0], 
                    Medicamento = medicamentos.First(m => m.Nome == "Zolpidem"), 
                    CID = "F51.0", // Insônia
                    NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                    DataEmissao = DateTime.Now.AddDays(-10), // Emitida há 10 dias
                    TipoDeUso = TipoUso.Uso30Dias
                },
                new Prontuario 
                { 
                    Paciente = pacientes[1], 
                    Medicamento = medicamentos.First(m => m.Nome == "Sibutramina"), 
                    CID = "E66", // Obesidade
                    NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    TipoDeUso = TipoUso.Continuo
                },
                 new Prontuario 
                { 
                    Paciente = pacientes[2], 
                    Medicamento = medicamentos.First(m => m.Nome == "Morfina"), 
                    CID = "R52", // Dor Crônica
                    NumeroNotificacao = "SP" + rand.Next(100000, 999999),
                    DataEmissao = DateTime.Now.Date, // Hoje
                    TipoDeUso = TipoUso.Uso10Dias
                }
            };
            context.Prontuarios.AddRange(prontuarios);
            context.SaveChanges();
        }
    }
}