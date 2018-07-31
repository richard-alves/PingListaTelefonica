using ListaTelefonicaAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaAPI.Data
{
    /// <summary>
    /// DBContext
    /// </summary>
    public class ListaTelefonicaContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Inicializa uma nova instância de DbContext <see cref="ListaTelefonicaContext"/>
        /// </summary>
        /// <param name="options">opções</param>
        public ListaTelefonicaContext(DbContextOptions<ListaTelefonicaContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Permite sobrescrever/complementar comportamento padrão
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// DbSet de contatos
        /// </summary>
        public DbSet<Contato> Contatos { get; set; }
    }
}
