using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextFarma.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } // Ex: Zolpidem, Morfina

        [Required]
        public string Concentracao { get; set; } // Ex: 10mg, 50mg

        // Foreign Key para a Regra de Negócio
        public int CategoriaId { get; set; }
        public CategoriaMedicamento Categoria { get; set; }
    }
}