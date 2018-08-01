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
        [EmailAddress]
        public string UserID { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Manter logado?
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
