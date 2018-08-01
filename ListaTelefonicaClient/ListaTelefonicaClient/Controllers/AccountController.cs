using ListaTelefonicaClient.Models;
using ListaTelefonicaClient.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Controllers
{
    /// <summary>
    /// Controlador para login
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Repositório para gerenciamento de login
        /// </summary>
        private readonly IAccountRepository _rep;
        
        /// <summary>
        /// Inicializa uma nova instância de <see cref="AccountController"/>
        /// </summary>
        /// <param name="rep">Repositório para gerenciamento de login</param>
        /// <param name="token">Token de autenticação</param>
        public AccountController(IAccountRepository rep, Token token)
        {
            _rep = rep;
        }

        /// <summary>
        /// Abrindo página de login
        /// </summary>
        /// <param name="returnUrl">Url para retornar após login</param>
        /// <returns>Página que estava acessando</returns>
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        
        /// <summary>
        /// Efetuando login
        /// </summary>
        /// <param name="model">Model com informações de usuário</param>
        /// <param name="returnUrl">URL a retornar</param>
        /// <returns>Página que estava acessando</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            // Retorno do login
            var response = await _rep.Login(model.Email, model.Password, model.RememberMe);
            
            // Se não deu certo retorna BadRequest
            if (!response.httpResponse.IsSuccessStatusCode) return BadRequest(response.httpResponse);

            // Se foi autenticado retorna página anterior
            if (response.token.Authenticated) return RedirectToLocal(returnUrl);

            // Qualquer coisa ficamos na tela de login
            return View(model);
        }

        /// <summary>
        /// Página que estava acessando ou Home
        /// </summary>
        /// <param name="returnUrl">Página que estava acessando</param>
        /// <returns>Home se returnUrl for null</returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
