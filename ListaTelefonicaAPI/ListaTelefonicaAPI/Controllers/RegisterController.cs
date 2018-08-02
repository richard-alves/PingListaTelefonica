using ListaTelefonicaAPI.Models;
using ListaTelefonicaAPI.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Controllers
{
    /// <summary>
    /// Para registrar novo usuário
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ValidateModel]
    public class RegisterController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="RegisterController"/>
        /// </summary>
        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registrando novo usuário
        /// </summary>
        /// <param name="usuario">Informações do usuário sendo registrado</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Post([FromBody]User usuario)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.FindByNameAsync(usuario.UserID).Result == null)
                {
                    var user = new ApplicationUser { UserName = usuario.UserID, Email = usuario.UserID };
                    var result = await _userManager.CreateAsync(user, usuario.Password);

                    if (result.Succeeded)
                    {
                        if (!String.IsNullOrWhiteSpace(Roles.DEFAULT_ROLE)) _userManager.AddToRoleAsync(user, Roles.DEFAULT_ROLE).Wait();
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        //return user;
                        return Ok(user);
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                            ModelState.AddModelError(err.Code, err.Description);

                        return BadRequest(string.Join(Environment.NewLine, result.Errors));
                        //throw new Exception("Erro ao tentar registrar usuário:" + string.Join(Environment.NewLine, result.Errors));
                    }
                }
                else ModelState.AddModelError(string.Empty, "O usuário já está cadastrado");
            }

            return BadRequest(ModelState);
        }
    }
}
