using ListaTelefonicaAPI.Models;
using ListaTelefonicaAPI.Models.Autenticacao;
using ListaTelefonicaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Controllers
{
    /// <summary>
    /// Classe para gerenciamento de logins
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="LoginController"/>
        /// </summary>
        /// <param name="userManager">Classe de usuário</param>
        /// <param name="signInManager">gerenciamento de login</param>
        /// <param name="emailSender">Para esquecimento de senha (não implementado)</param>
        /// <param name="logger">Logs</param>
        public LoginController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;

        }

        /// <summary>
        /// Efetuando o login
        /// </summary>
        /// <param name="usuario">Usuário logando</param>
        /// <param name="userManager">Gerenciamento de usuário</param>
        /// <param name="signInManager">Gerenciamento de login</param>
        /// <param name="signingConfigurations">Configurações de login</param>
        /// <param name="tokenConfigurations">Configurações de Token</param>
        /// <returns>Formato json com sucesso ou falha</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<object> Post(
               [FromBody]User usuario,
               [FromServices]UserManager<ApplicationUser> userManager,
               [FromServices]SignInManager<ApplicationUser> signInManager,
               [FromServices]SigningConfigurations signingConfigurations,
               [FromServices]TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = userManager
                    .FindByNameAsync(usuario.UserID).Result;

                var result = _signInManager.PasswordSignInAsync(usuario.UserID, usuario.Password, usuario.RememberMe, lockoutOnFailure: false);
                
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = await signInManager
                        .CheckPasswordSignInAsync(userIdentity, usuario.Password, false);

                    if (resultadoLogin.Succeeded)
                    {
                        // Verifica se o usuário em questão possui a role DEFAULT_ROLE
                        credenciaisValidas = userManager.IsInRoleAsync(
                            userIdentity, Roles.DEFAULT_ROLE).Result;
                    }
                }
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.UserID, "apiKey"
                    ),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                // Gerando token
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });

                var token = handler.WriteToken(securityToken);
                
                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}
