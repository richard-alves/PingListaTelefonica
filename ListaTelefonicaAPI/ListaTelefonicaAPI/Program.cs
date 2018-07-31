using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ListaTelefonicaAPI.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ListaTelefonicaAPI
{
    /// <summary>
    /// Onde tudo começa
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Classe primeira
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ListaTelefonicaContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Erro ao tentar criar DB");
                }
            }

            host.Run();
        }

        /// <summary>
        /// Construindo o WebHost
        /// </summary>
        /// <param name="args">parâmetros de inicialização</param>
        /// <returns>IWebHost</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .Build();
    }
}