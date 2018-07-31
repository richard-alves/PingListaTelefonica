using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Models.Autenticacao
{
    /// <summary>
    /// Configurações de token
    /// </summary>
    public class TokenConfigurations
    {
        /// <summary />
        public string Audience { get; set; }
        /// <summary />
        public string Issuer { get; set; }
        /// <summary />
        public int Seconds { get; set; }
    }
}
