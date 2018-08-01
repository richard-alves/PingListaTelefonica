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
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registrando novo usuário
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ApplicationUser> Post(
            [FromBody]User usuario
            //string userName, string password, string confirmPassword
            )
        {
            var user = new ApplicationUser { UserName = usuario.UserID, Email = usuario.UserID };
            var result = await _userManager.CreateAsync(user, usuario.Password);

            if (result.Succeeded)
            {
                if (!String.IsNullOrWhiteSpace(Roles.DEFAULT_ROLE)) _userManager.AddToRoleAsync(user, Roles.DEFAULT_ROLE).Wait();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return user;
            }
            else
            {
                foreach (var err in result.Errors)
                    throw new Exception("Erro ao tentar registrar usuário:" + string.Join(Environment.NewLine, result.Errors));
            }

            throw new Exception("Erro ao tentar registrar usuário.");
        }
    }
}
