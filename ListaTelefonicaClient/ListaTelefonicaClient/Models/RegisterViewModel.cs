using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Usuário")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
