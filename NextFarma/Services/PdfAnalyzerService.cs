using NextFarma.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;

namespace NextFarma.Services
{
    public class PdfAnalyzerService
    {
        public AnaliseReceitaViewModel AnalisarPdf(Stream pdfStream)
        {
            var resultado = new AnaliseReceitaViewModel();

            using (var document = PdfDocument.Open(pdfStream))
            {
                var page = document.GetPage(1);

                // =========================================================
                // 1. COORDENADAS REFINADAS (V3)
                // =========================================================

                // --- AMARELO (Medicamento) ---
                var rectMed_Canhoto = new PdfRectangle(350, 530, 500, 580);
                var rectMed_Via1 = new PdfRectangle(10, 280, 350, 350);
                var rectMed_Via2 = new PdfRectangle(440, 280, 750, 350);

                // --- VERMELHO (Data) ---
                var rectData_Canhoto = new PdfRectangle(10, 490, 160, 530);
                // Via 1: Ajustado para garantir leitura da data acima da assinatura
                var rectData_Via1 = new PdfRectangle(50, 100, 250, 150);
                var rectData_Via2 = new PdfRectangle(480, 100, 700, 150);

                // --- VERDE (Paciente) ---
                // CORREÇÃO CRÍTICA: Abaixei o Topo de 530 para 520 para não pegar o CEP de cima
                var rectPac_Canhoto = new PdfRectangle(180, 480, 450, 520);
                var rectPac_Via1 = new PdfRectangle(50, 420, 350, 465);
                var rectPac_Via2 = new PdfRectangle(480, 420, 750, 465);

                // --- LARANJA (Unidade) ---
                var rectUnit_Topo = new PdfRectangle(200, 540, 380, 580);
                var rectUnit_Dir = new PdfRectangle(650, 490, 800, 530);

                // --- PÊSSEGO (Número) ---
                var rectNumero = new PdfRectangle(40, 530, 150, 570);

                // --- ROXO (Assinaturas) ---
                var rectAss_Via1 = new PdfRectangle(50, 150, 350, 250);
                var rectAss_Via2 = new PdfRectangle(480, 150, 750, 250);
                var rectAss_Canhoto = new PdfRectangle(20, 530, 100, 550);


                // =========================================================
                // 2. EXTRAÇÃO E NORMALIZAÇÃO
                // =========================================================

                var txtMed1 = LimparTexto(ExtrairTextoOrdenado(page, rectMed_Canhoto));
                var txtMed2 = LimparTexto(ExtrairTextoOrdenado(page, rectMed_Via1));
                var txtMed3 = LimparTexto(ExtrairTextoOrdenado(page, rectMed_Via2));

                resultado.Remedio = new GrupoAnalise
                {
                    NomeGrupo = "Medicamento (Amarelo)",
                    ValoresEncontrados = new List<string> { txtMed1, txtMed2, txtMed3 },
                    // "ZOLPIDEM" está contido em "ZOLPIDEM 10MG..."? SIM.
                    Consistente = CompararSimilaridade(txtMed1, txtMed2, txtMed3)
                };

                var txtData1 = ExtrairApenasNumeros(ExtrairTextoOrdenado(page, rectData_Canhoto));
                var txtData2 = ExtrairApenasNumeros(ExtrairTextoOrdenado(page, rectData_Via1));
                var txtData3 = ExtrairApenasNumeros(ExtrairTextoOrdenado(page, rectData_Via2));

                resultado.Data = new GrupoAnalise
                {
                    NomeGrupo = "Data (Vermelho)",
                    ValoresEncontrados = new List<string> { txtData1, txtData2, txtData3 },
                    // Lógica especial para 25 vs 2025
                    Consistente = CompararDatasFlexivel(txtData1, txtData2, txtData3)
                };

                var txtPac1 = LimparTexto(ExtrairTextoOrdenado(page, rectPac_Canhoto));
                var txtPac2 = LimparTexto(ExtrairTextoOrdenado(page, rectPac_Via1));
                var txtPac3 = LimparTexto(ExtrairTextoOrdenado(page, rectPac_Via2));

                resultado.Paciente = new GrupoAnalise
                {
                    NomeGrupo = "Paciente/Endereço (Verde)",
                    ValoresEncontrados = new List<string> { txtPac1, txtPac2, txtPac3 },
                    // Se o endereço (Via 1) estiver dentro do Nome+Endereço (Via 2), dá OK
                    Consistente = CompararSimilaridade(txtPac1, txtPac3) || CompararSimilaridade(txtPac2, txtPac3)
                };

                var txtUnit1 = LimparTexto(ExtrairTextoOrdenado(page, rectUnit_Topo));
                var txtUnit2 = LimparTexto(ExtrairTextoOrdenado(page, rectUnit_Dir));

                resultado.Unidade = new GrupoAnalise
                {
                    NomeGrupo = "Unidade (Laranja)",
                    ValoresEncontrados = new List<string> { txtUnit1, txtUnit2 },
                    Consistente = CompararSimilaridade(txtUnit1, txtUnit2)
                };

                var txtNum = ExtrairTextoOrdenado(page, rectNumero);
                resultado.Numero = new GrupoAnalise
                {
                    NomeGrupo = "Número (Pêssego)",
                    ValoresEncontrados = new List<string> { txtNum },
                    Consistente = !string.IsNullOrWhiteSpace(txtNum)
                };

                resultado.Assinaturas = new StatusAssinatura
                {
                    AssinaturaCanhoto = DetectarPreenchimento(page, rectAss_Canhoto),
                    AssinaturaVia1 = DetectarPreenchimento(page, rectAss_Via1),
                    AssinaturaVia2 = DetectarPreenchimento(page, rectAss_Via2)
                };

                resultado.AprovadoGeral = resultado.Remedio.Consistente &&
                                          resultado.Data.Consistente &&
                                          resultado.Paciente.Consistente &&
                                          resultado.Assinaturas.AssinaturaVia1 &&
                                          resultado.Assinaturas.AssinaturaVia2;
            }

            return resultado;
        }

        // --- HELPERS ---

        // Restaura o Debug para funcionar a visualização dos quadrados
        public PdfDebugViewModel ExtractWordsWithCoords(Stream pdfStream)
        {
            var debug = new PdfDebugViewModel();
            using (var document = PdfDocument.Open(pdfStream))
            {
                var page = document.GetPage(1);
                foreach (var w in page.GetWords())
                {
                    debug.Words.Add(new PdfWordInfo
                    {
                        Text = w.Text,
                        Left = w.BoundingBox.Left,
                        Bottom = w.BoundingBox.Bottom,
                        Right = w.BoundingBox.Right,
                        Top = w.BoundingBox.Top
                    });
                }
            }
            return debug;
        }

        private string ExtrairTextoOrdenado(Page page, PdfRectangle rect)
        {
            var words = page.GetWords()
                .Where(w => rect.IntersectsWith(w.BoundingBox))
                // Agrupa linhas com tolerância de 5px e ordena da esquerda pra direita
                .OrderByDescending(w => Math.Round(w.BoundingBox.Bottom / 5) * 5)
                .ThenBy(w => w.BoundingBox.Left)
                .Select(w => w.Text);

            return string.Join(" ", words).Trim();
        }

        private string LimparTexto(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string limpo = input.Replace("Medicamento", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("Substancia", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("Uso Oral", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("R.", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("Nome:", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("Endereço:", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("Posologia", "", StringComparison.OrdinalIgnoreCase);

            limpo = Regex.Replace(limpo, @"[.,:;-]", " ");
            return Regex.Replace(limpo, @"\s+", " ").Trim().ToUpper();
        }

        private string ExtrairApenasNumeros(string input)
        {
            return Regex.Replace(input, @"[^\d]", "");
        }

        // Lógica: ZOLPIDEM (curto) está dentro de ZOLPIDEM 10MG (longo)? Sim -> True
        private bool CompararSimilaridade(params string[] valores)
        {
            var validos = valores.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (validos.Count < 2) return false;

            // Ordena pelo tamanho para pegar a string mais curta (ex: "Zolpidem")
            string referencia = validos.OrderBy(x => x.Length).First();

            bool todosCompativeis = true;
            foreach (var val in validos)
            {
                // Verifica se A contem B OU B contem A
                if (!val.Contains(referencia) && !referencia.Contains(val))
                {
                    // Tenta ver se pelo menos 50% da string bate (para endereços parciais)
                    if (!FuzzyMatch(referencia, val))
                    {
                        todosCompativeis = false;
                        break;
                    }
                }
            }
            return todosCompativeis;
        }

        private bool FuzzyMatch(string s1, string s2)
        {
            if (s1.Length < 4 || s2.Length < 4) return false;
            // Se as primeiras 5 letras batem, consideramos OK (Ex: "Rua Anne" vs "Rua Annemberg")
            string sub1 = s1.Substring(0, Math.Min(s1.Length, 5));
            string sub2 = s2.Substring(0, Math.Min(s2.Length, 5));
            return sub1 == sub2;
        }

        // Resolve o problema 061225 vs 06122025
        private bool CompararDatasFlexivel(params string[] datas)
        {
            var validas = datas.Where(d => !string.IsNullOrWhiteSpace(d)).ToList();
            if (validas.Count < 2) return false;

            string baseData = validas[0]; // Pega a primeira (ex: 061225)

            foreach (var d in validas)
            {
                // Se forem identicas, ótimo
                if (d == baseData) continue;

                // Se uma tem 6 digitos (dmyy) e a outra 8 (dmyyyy)
                if (Math.Abs(d.Length - baseData.Length) == 2)
                {
                    string curta = d.Length == 6 ? d : baseData;
                    string longa = d.Length == 8 ? d : baseData;

                    // Curta: 06 12 25 -> Dia:06, Mes:12, Ano:25
                    // Longa: 06 12 2025 -> Dia:06, Mes:12, Ano:2025

                    // Verifica Dia e Mes (4 primeiros digitos)
                    if (curta.Substring(0, 4) != longa.Substring(0, 4)) return false;

                    // Verifica final do ano (25 vs 2025)
                    if (!longa.EndsWith(curta.Substring(4, 2))) return false;
                }
                else
                {
                    return false; // Tamanhos incompativeis que nao sao data
                }
            }
            return true;
        }

        private bool DetectarPreenchimento(Page page, PdfRectangle rect)
        {
            bool temImagem = page.GetImages().Any(img => rect.IntersectsWith(img.Bounds));
            int letrasNaArea = page.Letters.Count(l => rect.IntersectsWith(l.GlyphRectangle));
            return temImagem || letrasNaArea > 3;
        }
    }
}