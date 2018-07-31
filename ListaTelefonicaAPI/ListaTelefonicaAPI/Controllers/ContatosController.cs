using ListaTelefonicaAPI.Models;
using ListaTelefonicaAPI.Repository;
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
    /// Controlador de contatos
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : Controller
    {
        private readonly IContatoRepository _contatoRepository;
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContatosController"/>
        /// </summary>
        /// <param name="contatoRepository">Repository para trabalhar com o gerenciamento dos contatos</param>
        public ContatosController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        /// <summary>
        /// Obtendo contatos
        /// </summary>
        /// <param name="filtro">Filtro por nome. Ex.: "nome ilike %tes%" </param>
        /// <returns>Lista de contatos</returns>
        [HttpGet]
        //GET: api/Contatos?filtro=
        public async Task<IEnumerable<Contato>> GetContatosAsync(string filtro)
        {
            return await _contatoRepository.GetContatosAsync(filtro);
        }

        /// <summary>
        /// Obtém contato por código
        /// </summary>
        /// <param name="id">Código do contato</param>
        /// <returns>Instância de <see cref="Contato"/> do código informado</returns>
        [HttpGet("{id}", Name="GetContato")]
        public ActionResult<Contato> GetById(int id)
        {
            var contato = _contatoRepository.Find(id);

            if (contato == null) return NotFound();

            return contato;
        }

        /// <summary>
        /// Adicionando um novo contato
        /// </summary>
        /// <param name="item">Contato a ser adicionado</param>
        /// <returns>Rota GetContato</returns>
        [HttpPost]
        public IActionResult Create(Contato item)
        {
            if (item == null) return BadRequest();

            _contatoRepository.Add(item);
            
            return CreatedAtRoute("GetContato", new { codigo = item.Codigo}, item);
        }

        /// <summary>
        /// Atualizando um contato existente
        /// </summary>
        /// <param name="id">Código do contato a ser atualizado</param>
        /// <param name="item">Contato, se existir</param>
        /// <returns>NoContentResult (OK)</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, Contato item)
        {
            if (item == null || item.Codigo != id) return BadRequest();

            var _contato = _contatoRepository.Find(id);

            if (_contato == null) return NotFound();

            _contato.Nome = item.Nome;
            _contato.Telefone = item.Telefone;
            _contato.Celular = item.Celular;
            _contato.Email = item.Email;
            _contato.Nascimento = item.Nascimento;

            _contatoRepository.Update(_contato);

            return new NoContentResult();

        }

        /// <summary>
        /// Deleta um contato
        /// </summary>
        /// <param name="id">Código do contato a ser removido</param>
        /// <returns>NoContentResult (OK)</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contato = _contatoRepository.Find(id);

            if (contato == null) return NotFound();

            _contatoRepository.Delete(id);
            return new NoContentResult();
        }
    }
}
