using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Services
{
    /// <summary>
    /// Classe para envio de e-mail para esquecimento de senha
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Enviando o e-mail
        /// </summary>
        /// <param name="email">Endereço de e-mail</param>
        /// <param name="subject">Assunto</param>
        /// <param name="message">Corpo do e-mail</param>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
