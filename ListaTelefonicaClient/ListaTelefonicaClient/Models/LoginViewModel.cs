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
        //[EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Token gerado após login
        /// </summary>
        public Token token { get; set; }
    }
}