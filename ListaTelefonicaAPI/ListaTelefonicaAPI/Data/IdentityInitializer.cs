using ListaTelefonicaAPI.Models;
using ListaTelefonicaAPI.Models.Autenticacao;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Data
{
    /// <summary>
    /// Classe para inicializar tabelas e informação de identidade
    /// </summary>
    public class IdentityInitializer
    {
        private readonly ListaTelefonicaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="IdentityInitializer"/>
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="userManager">Gerenciamento de usuário</param>
        /// <param name="roleManager">Gerenciamento de acessos</param>
        public IdentityInitializer(
            ListaTelefonicaContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Inicializando
        /// </summary>
        public void Initialize()
        {
            // Criando a base se não existir
            if (_context.Database.EnsureCreated())
            {
                // Criando a role padrão
                if (!_roleManager.RoleExistsAsync(Roles.DEFAULT_ROLE).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.DEFAULT_ROLE)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {Roles.DEFAULT_ROLE}.");
                    }
                }

                // Criando usuário padrão
                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "admin@teste.com.br",
                        Email = "admin@teste.com.br",
                        EmailConfirmed = true
                    }, "admin123", Roles.DEFAULT_ROLE);
            }
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="user">Nome de usuário</param>
        /// <param name="password">Senha</param>
        /// <param name="initialRole">Role</param>
        private void CreateUser(
            ApplicationUser user,
            string password,
            string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (resultado.Succeeded) { 
                    if ( !String.IsNullOrWhiteSpace(initialRole)) _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
                else
                {
                    var erros = string.Empty;

                    foreach (var res in resultado.Errors)
                        erros = res.Description + Environment.NewLine;

                    throw new Exception(erros);
                }
            }
        }
    }
}
