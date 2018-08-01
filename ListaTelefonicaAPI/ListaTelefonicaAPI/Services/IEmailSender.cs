using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Services
{
    /// <summary>
    /// <see cref="EmailSender"/>
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// <see cref="EmailSender.SendEmailAsync(string, string, string)"/>
        /// </summary>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
