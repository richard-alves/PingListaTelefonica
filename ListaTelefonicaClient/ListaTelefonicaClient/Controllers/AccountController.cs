using ListaTelefonicaClient.Models;
using ListaTelefonicaClient.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            await Logout();

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
            ViewData["ReturnUrl"] = returnUrl;

            // Retorno do login
            var response = await _rep.Login(model.Email, model.Password, model.RememberMe);

            // Se não deu certo retorna BadRequest
            if (!response.httpResponse.IsSuccessStatusCode) return BadRequest(response.httpResponse);

            // Se foi autenticado retorna página anterior
            if (response.token.Authenticated)
            {
                Startup.Autenticado = true;
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "O usuário e/ou senha informado estão incorretos");
            }

            // Qualquer coisa ficamos na tela de login
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            var res = await _rep.Logout();

            if (res.IsSuccessStatusCode)
            {
                Startup.Autenticado = false;
                return RedirectToAction("Login");
            }
            else return BadRequest();
        }

        public async Task<IActionResult> Register(string returnUrl)
        {
            await Logout();

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,Password,ConfirmPassword")] RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;

                var res = await _rep.Register(model);

                IActionResult ret = View(model);

                if (res.IsSuccessStatusCode)
                {
                    await Login(new LoginViewModel { Email = model.Email, Password = model.Password }, returnUrl)
                        .ContinueWith(a => { if (Startup.Autenticado) ret = RedirectToLocal(returnUrl); });
                }
                else ModelState.AddModelError("Password", "Não foi possível registrar o usuário. A senha deve conter entre 8 e 12 caracteres e deve conter letras e números");

                return ret;
            }
            else return BadRequest(ModelState);
        }
        /// <summary>
        /// Página que estava acessando ou Home
        /// </summary>
        /// <param name="returnUrl">Página que estava acessando</param>
        /// <returns>Home se returnUrl for null</returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) || (returnUrl?.StartsWith("https://localhost:44391")).GetValueOrDefault())
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}