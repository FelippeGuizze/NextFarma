// Models/AnaliseReceitaViewModel.cs
using System.Collections.Generic;

namespace NextFarma.Models
{
    public class AnaliseReceitaViewModel
    {
        public bool AprovadoGeral { get; set; }

        // Seus grupos de verificação baseados nas cores
        public GrupoAnalise Remedio { get; set; }     // Amarelo
        public GrupoAnalise Data { get; set; }        // Vermelho
        public GrupoAnalise Paciente { get; set; }    // Verde
        public GrupoAnalise Unidade { get; set; }     // Laranja
        public GrupoAnalise Numero { get; set; }      // Pêssego

        // Assinaturas são booleanas (Tem ou Não Tem)
        public StatusAssinatura Assinaturas { get; set; } // Roxo
    }

    public class GrupoAnalise
    {
        public string NomeGrupo { get; set; } // Ex: "Medicamento (Amarelo)"
        public List<string> ValoresEncontrados { get; set; } = new List<string>();
        public bool Consistente { get; set; } // True se todos os valores baterem
    }

    public class StatusAssinatura
    {
        public bool AssinaturaCanhoto { get; set; }
        public bool AssinaturaVia1 { get; set; }
        public bool AssinaturaVia2 { get; set; }
        public bool Completo => AssinaturaCanhoto && AssinaturaVia1 && AssinaturaVia2;
    }
}