using ListaTelefonicaClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ListaTelefonicaClient.Repository
{
    /// <summary>
    /// Interface de repositório de contatos <see cref="ContatosRepository"/>
    /// </summary>
    public interface IContatosRepository
    {
        Task<IEnumerable<Contato>> GetContatosAsync();
        Task<List<Contato>> GetContatosAsync(string filtro);
        Task<Contato> GetContatoAsync(int id);
        Task<HttpResponseMessage> Create(Contato contato);
        Task Update(Contato contato);
        Task Delete(int id);
    }
}
