using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListaTelefonicaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ListaTelefonicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoControllerOld : ControllerBase
    {
        public readonly ListaTelefonicaContext _context;

        public ContatoControllerOld(ListaTelefonicaContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contato>>> Get()
        {
            return _context.Contatos.ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name="GetContato")]
        public ActionResult<Contato> Get(int id)
        {
            var item = _context.Contatos.Find(id);

            if (item == null) return NotFound();

            return item;
        }

        public ActionResult<IEnumerable<Contato>> Get(string nome)
        {
            var items = from c in _context.Contatos
                       select c;

            if (!string.IsNullOrWhiteSpace(nome))
                items = items.Where(a => a.Nome.Contains(nome));

            return items.ToList();
        }

        [HttpPost]
        public IActionResult Create(Contato item)
        {
            _context.Contatos.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetContato", new { codigo = item.Codigo, nome = item.Nome,  }, item);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
