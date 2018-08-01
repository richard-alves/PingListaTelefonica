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
        Task<IEnumerable<ContatoViewModel>> GetContatosAsync();
        Task<List<ContatoViewModel>> GetContatosAsync(string filtro);
        Task<ContatoViewModel> GetContatoAsync(int id);
        Task<HttpResponseMessage> Create(ContatoViewModel contato);
        Task<HttpResponseMessage> Update(ContatoViewModel contato);
        Task<HttpResponseMessage> Delete(int id);
    }
}
