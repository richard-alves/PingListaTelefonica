using ListaTelefonicaClient.Models;
using ListaTelefonicaClient.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Controllers
{
    /// <summary>
    /// Página de contatos
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Repositório para gerenciar contatos
        /// </summary>
        private readonly IContatosRepository _rep;

        /// <summary>
        /// Inicializa uma nova instância de<see cref="HomeController"/>
        /// </summary>
        /// <param name="rep">Repositório para gerenciar contatos</param>
        /// <param name="token">Token de acesso</param>
        public HomeController(IContatosRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// Abre página para adição de novo contato
        /// </summary>
        // GET: ContatoViewModels/Create
        public IActionResult Create()
        {

            return View();
        }

        /// <summary>
        /// Inserção do usuário
        /// </summary>
        /// <param name="contato">Contato a ser inserido</param>
        // POST: ContatoViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,Telefone,Celular,Email,Nascimento")] Contato contato)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _rep.Create(contato);
                }
                catch (NotAuthorizedException)
                {
                    return RedirectToRoute("Login");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Abre tela para confirmação de deleção
        /// </summary>
        /// <param name="id">Código do usuário a ser deletado</param>
        // GET: Contatos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 0) return NotFound();

            var contato = await _rep.GetContatoAsync(id.GetValueOrDefault());
            if (contato == null) return NotFound();

            return View(contato);
        }

        /// <summary>
        /// Confirmando deleção, usuário será removido
        /// </summary>
        /// <param name="id">Código do usuário deletado</param> 
        // POST: Contatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _rep.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (NotAuthorizedException)
            {
                return RedirectToRoute("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exibe em formato de form as informações do usuário
        /// </summary>
        /// <param name="id">Código do usuário</param>
        /// <returns></returns>
        // GET: Contatos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var contato = await _rep.GetContatoAsync(id.GetValueOrDefault());
                if (contato == null) return NotFound();

                return View(contato);
            }
            catch (NotAuthorizedException)
            {
                return RedirectToRoute("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Abre tela edição das informações
        /// </summary>
        /// <param name="id">Código do usuário que será editado</param>
        // GET: Contatos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) return NotFound();

            try
            {
                var contato = await _rep.GetContatoAsync(id.GetValueOrDefault());
                if (contato == null) return NotFound();

                return View(contato);
            }
            catch (NotAuthorizedException)
            {
                return RedirectToRoute("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Concluindo a edição
        /// </summary>
        /// <param name="id">Código do usuário que foi editado</param>
        /// <param name="contato">Instância do contato e informação de Bind</param>
        // POST: Contatos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Telefone,Celular,Email,Nascimento")] Contato contato)
        {
            if (id != contato.Codigo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _rep.Update(contato);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ContatoExists(contato.Codigo)) return NotFound();
                    else throw;
                }
                catch (NotAuthorizedException)
                {
                    return RedirectToRoute("Login");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(contato);
        }

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Lista de contatos (Página inicial)
        /// </summary>
        /// <param name="id">Código do contato</param>
        /// <param name="filtro">Filtro a ser realizado(ilike)</param>
        /// <param name="sortOrder">Campo de ordenação</param>
        /// <param name="currentFilter">Filtro atual</param>
        /// <returns>Página</returns>
        public async Task<IActionResult> Index(int id, string filtro, string sortOrder, string currentFilter)
        {

            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["ordemNome"] = String.IsNullOrEmpty(sortOrder) ? "nome_desc" : "";
                ViewData["ordemEmail"] = sortOrder == "email" ? "email_desc" : "email";
                ViewData["ordemNasc"] = sortOrder == "nasc" ? "nasc_desc" : "nasc";

                List<Contato> contatos = new List<Contato>();

                if (filtro == null)
                    filtro = currentFilter;

                ViewData["filtro"] = filtro;

                if (id > 0)
                {
                    var contato = await _rep.GetContatoAsync(id);

                    if (contato == null) return NotFound();
                    contatos.Add(contato);
                }
                else if (!string.IsNullOrWhiteSpace(filtro))
                {
                    contatos = (await _rep.GetContatosAsync(filtro))?.ToList();
                }
                else
                {
                    contatos = (await _rep.GetContatosAsync())?.ToList();
                }

                contatos = Ordernar(contatos, sortOrder)?.ToList();

                return View(contatos);
            }
            catch (NotAuthorizedException)
            {
                return RedirectToRoute("Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary />
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Verificando se o contato existe
        /// </summary>
        /// <param name="id">Código do contato</param>
        private async Task<bool> ContatoExists(int id)
        {
            return await _rep.GetContatoAsync(id) != null;
        }

        /// <summary>
        /// Ordernando lista na página
        /// </summary>
        /// <param name="contatos">Lista de contatos</param>
        /// <param name="ordem">campo a ser ordernado</param>
        /// <returns>Lista de contatos ordenada</returns>
        private IEnumerable<Contato> Ordernar(IEnumerable<Contato> contatos, string ordem)
        {
            // O padrão é ordernar pr nome
            if (ordem == null) ordem = "nome";

            // Verificando qual campo e asc ou desc
            switch (ordem)
            {
                case "nome":
                    contatos = contatos.OrderBy(c => c.Nome);
                    break;

                case "nome_desc":
                    contatos = contatos.OrderByDescending(s => s.Nome);
                    break;

                case "email":
                    contatos = contatos.OrderBy(s => s.Email);
                    break;

                case "email_desc":
                    contatos = contatos.OrderByDescending(s => s.Email);
                    break;

                case "nasc":
                    contatos = contatos.OrderBy(s => s.Nascimento);
                    break;

                case "nasc_desc":
                    contatos = contatos.OrderByDescending(s => s.Nascimento);
                    break;

                default:
                    contatos = contatos.OrderBy(s => s.Nome);
                    break;
            }

            return contatos?.ToList();
        }
    }
}