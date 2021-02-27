using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APIReparos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret@jwt123#secret@jwt123#"))
                };
            });
            services.AddDbContext<Context.ReparosContext>(options => options.UseInMemoryDatabase("Reparos"));
            services.AddMvc();
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/error");
            }
            var context = serviceProvider.GetService<Context.ReparosContext>();
            AdicionarDadosTeste(context);
            app.UseAuthentication();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMvc();
        }

        private static void AdicionarDadosTeste(Context.ReparosContext context)
        {
            var testeUsuario1 = new Models.Usuario
            {
                Id = 1,
                NomeUsuario = "admin",
                Email = "admin@testes.com",
                Senha = "1234",
                Role = Models.Role.Adm
            };
            var testeUsuario2 = new Models.Usuario
            {
                Id = 2,
                NomeUsuario = "usr",
                Email = "usr@testes.com",
                Senha = "1234",
                Role = Models.Role.User
            };
            context.Usuarios.Add(testeUsuario1);
            context.Usuarios.Add(testeUsuario2);

            var testeEquip1 = new Models.Equipamento
            {
                Id = 1,
                NomeEquipamento = "Antena 1",
                Identificador = "XABX"
            };
            var testeEquip2 = new Models.Equipamento
            {
                Id = 2,
                NomeEquipamento = "Antena 2",
                Identificador = "XABX2"
            };            
            context.Equipamentos.Add(testeEquip1);
            context.Equipamentos.Add(testeEquip2);



            var reparo1 = new Models.Reparo
            {
                Id = 1,
                UsuarioId = 2,
                EquipamentoId = 1,
                DataInicio = DateTime.Now,
                DataFim = null,
                StatusReparo = Models.Status.Iniciar,
                Observacao = "Lorem ipsum"
            };
            var reapro2 = new Models.Reparo
            {
                Id = 2,
                UsuarioId = 2,
                EquipamentoId = 2,
                DataInicio = DateTime.Now,
                DataFim = null,
                StatusReparo = Models.Status.Concluir,
                Observacao = "Lorem ipsum"                
            };            
            context.Reparos.Add(reparo1);
            context.Reparos.Add(reapro2);
            context.SaveChanges();
        }
    }
}
