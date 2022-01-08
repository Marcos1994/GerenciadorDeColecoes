using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Aplicacao.Profiles;
using MinhasColecoes.Aplicacao.Services;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Exceptions;
using MinhasColecoes.Persistencia.Interfaces;
using MinhasColecoes.Testes.Data;
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
		public void CreateSemColecaoMaior()
		{
			ColecaoInputModel input = new ColecaoInputFaker().Generate();
			input.IdColecaoMaior = null;

			repositorioColecao.GetAll(0).Returns(new List<Colecao>());

			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.IdDono.Should().Be(input.IdDono);
			colecaoNova.Nome.Should().Be(input.Nome);
			colecaoNova.Descricao.Should().Be(input.Descricao);
			colecaoNova.Publica.Should().Be(input.Publica);
			colecaoNova.ColecaoMaior.Should().BeNull();

			colecaoNova.UsuariosColecao.Should().HaveCount(1);
			colecaoNova.Colecoes.Should().BeEmpty();
			colecaoNova.Itens.Should().BeEmpty();
		}

		[Fact]
		public void CreateComColecaoMaior()
		{
			ColecaoInputModel input = new ColecaoInputFaker(true).Generate();

			repositorioColecao.GetAll(0).Returns(new List<Colecao>());
			repositorioColecao.GetById((int) input.IdColecaoMaior)
				.Returns(new ColecaoFaker().Generate());

			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.IdDono.Should().Be(input.IdDono);
			colecaoNova.Nome.Should().Be(input.Nome);
			colecaoNova.Descricao.Should().Be(input.Descricao);
			colecaoNova.Publica.Should().Be(input.Publica);
			colecaoNova.IdColecaoMaior.Should().Be((int)input.IdColecaoMaior);

			colecaoNova.UsuariosColecao.Should().HaveCount(1);
			colecaoNova.Colecoes.Should().BeEmpty();
			colecaoNova.Itens.Should().BeEmpty();
		}

		[Fact]
		public void CreateComNomeRepetido()
		{
			ColecaoInputModel input = new ColecaoInputFaker().Generate();

			repositorioColecao.GetAll(input.IdDono, input.Nome)
				.Returns(new List<Colecao>() { mapper.Map<Colecao>(input) });

			Action act = () => service.Create(input);
			act.Should().ThrowExactly<ObjetoDuplicadoException>();
		}
	}
}
