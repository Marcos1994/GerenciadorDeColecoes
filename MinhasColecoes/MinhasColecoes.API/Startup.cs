using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.API.Services;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Validators;
using MinhasColecoes.Aplicacao.Profiles;
using MinhasColecoes.Aplicacao.Services;
using MinhasColecoes.Persistencia.Context;
using MinhasColecoes.Persistencia.Interfaces;
using MinhasColecoes.Persistencia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.API
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
			services.AddSingleton<IJWTService, JWTService>();
			byte[] chave = Encoding.ASCII.GetBytes(Configuration.GetSection("JWT:Secret").Value);
			services.AddAuthentication(a =>
				{
					a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
			.AddJwtBearer(a =>
			{
				a.RequireHttpsMetadata = false;
				a.SaveToken = true;
				a.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(chave),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true
				};
			});

			services.AddAutoMapper(typeof(UsuarioProfile));
			services.AddAutoMapper(typeof(ColecaoProfile));
			services.AddAutoMapper(typeof(ItemProfile));

			services.AddScoped<IUsuarioService, UsuarioService>();
			services.AddScoped<IUsuarioRepository, UsuarioRepository>();

			services.AddScoped<IColecaoService, ColecaoService>();
			services.AddScoped<IColecaoRepository, ColecaoRepository>();

			services.AddScoped<IItemService, ItemService>();
			services.AddScoped<IItemRepository, ItemRepository>();

			services.AddDbContext<MinhasColecoesDbContext>(o => o.UseInMemoryDatabase("MinhasColecoesCs"));
			//services.AddDbContext<MinhasColecoesDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MinhasColecoesCs")));
			services.AddScoped<MinhasColecoesDbContext>();

			services.AddControllers()
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UsuarioInputValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UsuarioLoginInputValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UsuarioUpdateValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UsuarioSenhaUpdateValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ColecaoInputValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ColecaoUpdateValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ItemInputValidator>())
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ItemUpdateValidator>());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinhasColecoes.API", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Insira o token",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference= new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id ="Bearer"
							}
						},
						new string[]{ }
					}
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinhasColecoes.API v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
