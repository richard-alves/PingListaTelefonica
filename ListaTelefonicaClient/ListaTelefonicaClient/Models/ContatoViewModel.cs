using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Models
{
    /// <summary>
    /// Informações do contato
    /// </summary>
    public class ContatoViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [Phone]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:(351) ### ### ###}")]
        [RegularExpression("#########")]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Phone]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:(351) ### ### ###}")]
        [RegularExpression("#########")]
        public string Celular { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Nascimento { get; set; }
    }
}
