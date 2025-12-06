using System.Collections.Generic;

namespace NextFarma.Models
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Paciente> Pacientes { get; set; } = new List<Paciente>();
        public IEnumerable<Prontuario> Prontuarios { get; set; } = new List<Prontuario>();
        public IEnumerable<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}