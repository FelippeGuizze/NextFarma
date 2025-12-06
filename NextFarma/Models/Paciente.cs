using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NextFarma.Models
{
    public enum Sexo { Masculino, Feminino, Outro }

    public class Paciente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Range(0, 150, ErrorMessage = "Idade inválida")]
        public int? Idade { get; set; }

        public string? Endereco { get; set; }

        [Display(Name = "CEP")]
        // Removido RegularExpression para permitir o traço (ex: 00000-000)
        // O Controller vai limpar isso depois
        public string? CEP { get; set; }

        [Display(Name = "RG")]
        // Removido RegularExpression para permitir pontos (ex: 12.345.678-9)
        public string? RG { get; set; }

        [Display(Name = "Telefone")]
        // Removido RegularExpression para permitir parênteses (ex: (11) 99999-9999)
        public string? Telefone { get; set; }

        public Sexo Sexo { get; set; }

        // Histórico do Paciente
        public ICollection<Prontuario> HistoricoProntuarios { get; set; } = new List<Prontuario>();
    }
}