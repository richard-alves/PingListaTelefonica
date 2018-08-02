using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI
{
    /// <summary>
    /// Classe de validação
    /// </summary>
    [ValidateModel]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Ao executar ação, verifica se tem erros no modelo
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
