using System.ComponentModel.DataAnnotations; 

namespace NextFarma.Models
{
    public enum PeopleType
    {
        [Display(Name = "Administrador")]
        Adm = 0,

        [Display(Name = "Professor")]
        Teacher = 1,

        [Display(Name = "Aluno")]
        Student = 2
    }
}