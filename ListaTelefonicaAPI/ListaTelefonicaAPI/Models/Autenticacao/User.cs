using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Models.Autenticacao
{
    /// <summary>
    /// Informações do usuário ao logar
    /// </summary>
    public class User
    {
        /// <summary>
        /// Nome de usuário
        /// </summary>
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string UserID { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MaxLength(16)]
        public string Password { get; set; }
        /// <summary>
        /// Manter logado?
        /// </summary>
        public bool RememberMe => false;
    }
}
