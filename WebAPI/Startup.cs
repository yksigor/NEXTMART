using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository.Data;
using Domain.Security;
using Services;
using Services.Contratos;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //if (env.IsProduction() || env.IsStaging())
            //{
            //    var builder = new ConfigurationBuilder()
            //        .SetBasePath(env.ContentRootPath)
            //        //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //        .AddEnvironmentVariables();

            //    Configuration = builder.Build();
            //} else
            Configuration = configuration;
            environment = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<LojaDBContext>(option => option
                .UseSqlServer(Configuration.GetSection($"ConnectionStrings:{environment.EnvironmentName}").Value));
            //services.AddDbContext<LojaDBContext>(options => options.UseInMemoryDatabase("Banco"));

            services.AddScoped<ILoginServicesJWT, LoginServicesJWT>();
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();
            services.AddScoped<IPessoaServices, PessoaServices>();
            services.AddScoped<IProdutoServices, ProdutoServices>();
            services.AddScoped<IItemServices,ItemServices>();
            services.AddScoped<IVendaServices, VendaServices>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddScoped<LoginServices>();

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(signingConfigurations, tokenConfigurations);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
