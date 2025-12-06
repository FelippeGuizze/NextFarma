using System;
using System.ComponentModel.DataAnnotations;

namespace NextFarma.Models
{
    public class Usuario
    {
        public int Id { get; set; } 

        [Required]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Senha { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Tipo de Usuário")]
        public PeopleType Type { get; set; }

        public Usuario() { }

        public Usuario(string email, string senha, DateTime birthDate, PeopleType type)
        {
            Email = email;
            Senha = senha;
            BirthDate = birthDate;
            Type = type;
        }
    }
}