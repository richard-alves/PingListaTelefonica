using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Models
{
    /// <summary>
    /// Classe com informações dos contatos
    /// </summary>
    public class Contato
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        /// <summary>
        /// Telefone Principal
        /// </summary>
        [Required]
        [Phone]
        public string Telefone { get; set; }

        /// <summary>
        /// Celular
        /// </summary>
        [Phone]
        public string Celular { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }
    }
}
