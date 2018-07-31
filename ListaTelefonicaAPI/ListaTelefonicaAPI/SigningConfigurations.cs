using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI
{
    /// <summary>
    /// Configurações de login
    /// </summary>
    public class SigningConfigurations
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="SigningConfigurations"/>
        /// </summary>
        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }

        /// <summary>
        /// Chave
        /// </summary>
        public SecurityKey Key { get; }

        /// <summary>
        /// Credenciais
        /// </summary>
        public SigningCredentials SigningCredentials { get; }
    }
}
