using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Models
{
    /// <summary>
    /// Informações de token de acesso
    /// </summary>
    public class Token
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
    }
}
