using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Aplicacao.Profiles;
using MinhasColecoes.Aplicacao.Services;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MinhasColecoes.Testes.Services
{
	public class ColecaoServiceTeste
	{
		private readonly ColecaoService service;
		private readonly IMapper mapper;
		private readonly IColecaoRepository repositorioColecao;
		private readonly IItemRepository repositorioItem;

		public ColecaoServiceTeste()
		{
			if (mapper == null)
			{
				var mappingConfig = new MapperConfiguration(mc =>
				{
					mc.AddProfile(new ColecaoProfile());
					mc.AddProfile(new ItemProfile());
					mc.AddProfile(new UsuarioProfile());
				});
				mapper = mappingConfig.CreateMapper();
			}

			repositorioColecao = Substitute.For<IColecaoRepository>();
			repositorioItem = Substitute.For<IItemRepository>();

			service = new ColecaoService(mapper, repositorioColecao, repositorioItem);
		}

		[Fact]
		public void Create()
		{
			repositorioColecao.GetAll(0).Returns(
				new List<Colecao>
				{
					new Colecao(null, 1, "Colecao Teste", "", "", false)
				});
			ColecaoInputModel input = new ColecaoInputModel
			{
				IdDono = 1,
				Descricao = "Primeira colecao de teste",
				IdColecaoMaior = 0,
				Nome = "Colecao Teste",
				Publica = true
			};
			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.Nome.Should().Be("Colecao Teste");
			colecaoNova.ColecaoMaior.Should().BeNull();
		}
	}
}
