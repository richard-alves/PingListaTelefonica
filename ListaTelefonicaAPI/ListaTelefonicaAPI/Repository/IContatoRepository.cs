using ListaTelefonicaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Repository
{
    /// <summary>
    /// Interface para repositório de contato
    /// </summary>
    public interface IContatoRepository
    {
        /// <summary>
        /// <see cref="ContatoRepository.Add(Contato)"/>
        /// </summary>
        void Add(Contato contato);

        /// <summary>
        /// <see cref="ContatoRepository.GetContatosAsync(string)"/>
        /// </summary>
        Task<List<Contato>> GetContatosAsync(string filtro);

        /// <summary>
        /// <see cref="ContatoRepository.Find(int)"/>
        /// </summary>
        Contato Find(int id);

        /// <summary>
        /// <see cref="ContatoRepository.Delete(int)"/>
        /// </summary>
        void Delete(int id);

        /// <summary>
        /// <see cref="ContatoRepository.Update(Contato)"/>
        /// </summary>
        void Update(Contato contato);
    }
}
