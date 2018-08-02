using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ListaTelefonicaAPI.Data;
using ListaTelefonicaAPI.Models;
using ListaTelefonicaAPI.Models.Autenticacao;
using ListaTelefonicaAPI.Repository;
using ListaTelefonicaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace ListaTelefonicaAPI
{
    /// <summary>
    /// Configurando serviços
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration">Parâmetros de configuração</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Instância de configuração recebida 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Método para adicionar serviços
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Conexão
            services.AddDbContext<ListaTelefonicaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Autenticação
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ListaTelefonicaContext>();

            // Repositórios utilizados nos requests
            services.AddTransient<IContatoRepository, ContatoRepository>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configurando informações de criação de login
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });


            #region Configurações do JWT Bearer e Token
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);
            
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = false;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = false;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            })
            .AddCookie(o =>
            {
                o.LoginPath = "/api/login";
                o.LogoutPath = "/api/logout";
                // additional config options here
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            #endregion

            // Adicionando Swagger para doc
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Lista Telefônica",
                        Version = "v2.0",
                        Description = "Contatos de uma lista",
                        Contact = new Contact
                        {
                            Name = "Richard",
                            Email = "rick.aclive@hotmail.com"
                        }
                    });

                s.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Autenticação baseada em Json Web Token (JWT)",
                        Name = "Authorization",
                        Type = "apiKey"
                    });

                string caminhoAplicacao = AppContext.BaseDirectory;
                string assemblyName =  System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");

                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{assemblyName}.xml");

                s.IncludeXmlComments(caminhoXmlDoc);
            });
        }

        /// <summary>
        /// Configurando o pipeline
        /// </summary>
        /// <param name="app">Applicação</param>
        /// <param name="env">Ambiente</param>
        /// <param name="context">Contexto</param>
        /// <param name="userManager">Gerenciamento de contas de usuário</param>
        /// <param name="roleManager">gerenciamento de acessos</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ListaTelefonicaContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            new IdentityInitializer(context, userManager, roleManager)
                .Initialize();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "GetContato",
                    template: "Contato/{action}",
                    defaults: new { Controller = "Contato", Action = "GetById" }
                    );

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            // Rota 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lista Telefônica");
            });

            app.UseMvc();
        }
    }
}
