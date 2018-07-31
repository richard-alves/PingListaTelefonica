using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListaTelefonicaAPI.Data;
using ListaTelefonicaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaTelefonicaAPI.Repository
{
    /// <summary>
    /// Repositório de Contatos
    /// </summary>
    public class ContatoRepository : IContatoRepository
    {
        /// <summary>
        /// DbContexts
        /// </summary>
        private readonly ListaTelefonicaContext _context;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ContatoRepository"/>
        /// </summary>
        /// <param name="context">DbContext</param>
        public ContatoRepository(ListaTelefonicaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserindo novo contato
        /// </summary>
        /// <param name="contato">Contato a ser inserido</param>
        public void Add(Contato contato)
        {
            _context.Contatos.Add(contato);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletando contato existente
        /// </summary>
        /// <param name="id">Código do contato</param>
        public void Delete(int id)
        {
            var contato = Find(id);
            _context.Contatos.Remove(contato);
            _context.SaveChanges();
        }

        /// <summary>
        /// Encontrando contatos
        /// </summary>
        /// <param name="id">Código do contato</param>
        /// <returns>Contato<see cref="Contato"/></returns>
        public Contato Find(int id)
        {
            return _context.Contatos.Find(id);
        }

        /// <summary>
        /// Obtém vários contatos
        /// </summary>
        /// <param name="filtro">Filtro a ser aplicado no campo nome</param>
        /// <returns>Lista de contatos</returns>
        public async Task<List<Contato>> GetContatosAsync(string filtro)
        {
            var contatos = from x in _context.Contatos
                           select x;

            if (!string.IsNullOrWhiteSpace(filtro))
                contatos = contatos.Where(a => a.Nome.Contains(filtro));

            return await contatos.ToListAsync();
        }

        /// <summary>
        /// Atualizando um contato existente
        /// </summary>
        /// <param name="contato">Contato a ser atualizado</param>
        public void Update(Contato contato)
        {
            _context.Contatos.Update(contato);
            _context.SaveChanges();
        }
    }
}
