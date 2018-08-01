using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Models
{
    /// <summary>
    /// Model utilizado no login
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuário")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Manter conectado")]
        public bool RememberMe { get; set; }
        
        public Token token { get; set; }
    }
}