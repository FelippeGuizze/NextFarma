using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NextFarma.Models
{
    public enum Sexo { Masculino, Feminino, Outro }

    public class Paciente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Range(0,150, ErrorMessage = "Idade inválida")]
        public int? Idade { get; set; }

        public string? Endereco { get; set; }

        [Display(Name = "CEP")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos numéricos.")]
        public string? CEP { get; set; }

        [Display(Name = "RG")]
        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "RG deve conter entre 7 e 9 dígitos numéricos.")]
        public string? RG { get; set; }

        [Display(Name = "Telefone")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone deve conter 10 ou 11 dígitos numéricos (DDD + número).")]
        public string? Telefone { get; set; }

        public Sexo Sexo { get; set; }

        // Histórico do Paciente
        public ICollection<Prontuario> HistoricoProntuarios { get; set; } = new List<Prontuario>();
    }
}