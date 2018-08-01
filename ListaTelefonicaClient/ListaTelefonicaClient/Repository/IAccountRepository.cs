using ListaTelefonicaClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Repository
{
    /// <summary>
    /// Interface para repositório de accounts
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// <see cref="AccountRepository.Login(string, string, bool)"/>
        /// </summary>
        Task<(HttpResponseMessage httpResponse, Token token)> Login(string userName, string pass, bool rememberMe);
        Task<HttpResponseMessage> Logout();
    }
}
