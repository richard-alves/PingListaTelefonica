using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Controllers
{
    /// <summary>
    /// Responsável pelo logout
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        /// <summary>
        /// Fazendo o logout
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public object Get()
        {
            return new
            {
                authenticated = false,
                message = "Falha ao autenticar"
            };
        }
    }
}