using System;
using System.ComponentModel.DataAnnotations;

namespace NextFarma.Models
{
    public enum TipoUso 
    { 
        [Display(Name = "Uso Contínuo")]
        Continuo,
        [Display(Name = "Uso 10 Dias")]
        Uso10Dias,
        [Display(Name = "Uso 30 Dias")]
        Uso30Dias,
        [Display(Name = "Uso 90 Dias")]
        Uso90Dias 
    }

    public class Prontuario
    {
        public int Id { get; set; }

        // Dados do Histórico
        [Required]
        public string CID { get; set; } = string.Empty; // Classificação Internacional de Doenças
        
        [Required]
        [Display(Name = "Número Notificação")]
        public string NumeroNotificacao { get; set; } = string.Empty; // Número do bloco da receita

        [Display(Name = "Data Emissão")]
        public DateTime DataEmissao { get; set; }

        // Regra de Negócio: Validade de 30 dias contados a partir do dia seguinte ou dia 0
        public DateTime DataValidade => DataEmissao.AddDays(31);

        [Display(Name = "Tipo de Uso")]
        public TipoUso TipoDeUso { get; set; }

        // Relacionamentos
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; } = null!;

        public int MedicamentoId { get; set; }
        public Medicamento Medicamento { get; set; } = null!;
    }
}