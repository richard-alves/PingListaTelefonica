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
    public class Contato
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
        public string Telefone { get; set; }

        [Phone]
        public string Celular { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }
    }
}
