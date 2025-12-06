using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NextFarma.Models
{
    public class CategoriaMedicamento
    {
        public int Id { get; set; }

        [Required]
        public string Lista { get; set; } // Ex: A1, B2, C5

        [Required]
        public string Descricao { get; set; } // Ex: Entorpecentes, Psicotrópicos

        [Required]
        public string TipoReceituario { get; set; } // Ex: Notificação de Receita A

        [Required]
        public string CorReceita { get; set; } // Ex: Amarela, Azul, Branca

        // Relacionamento: Uma categoria tem vários remédios
        public ICollection<Medicamento> Medicamentos { get; set; }
    }
}