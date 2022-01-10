using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Aplicacao.Profiles;
using MinhasColecoes.Aplicacao.Services;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using MinhasColecoes.Testes.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace MinhasColecoes.Testes.Services
{
	public class ColecaoServiceCreateTeste
	{
		private readonly ColecaoService service;
		private readonly IMapper mapper;
		private readonly IColecaoRepository repositorioColecao;
		private readonly IItemRepository repositorioItem;

		public ColecaoServiceCreateTeste()
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
		public void SucessoCreate()
		{
			ColecaoInputModel input = new ColecaoInputFaker().Generate();
			Colecao colecaoComNomeParecido = new ColecaoFaker().Generate();
			colecaoComNomeParecido.Update(input.Nome + " parecido", input.Descricao, input.Foto, true);
			input.IdColecaoMaior = colecaoComNomeParecido.IdColecaoMaior;

			repositorioColecao.GetAllSubcolecoes(input.IdDono, input.IdColecaoMaior, input.Nome)
				.Returns(new List<Colecao>() { colecaoComNomeParecido });

			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.IdDono.Should().Be(input.IdDono);
			colecaoNova.Nome.Should().Be(input.Nome);
			colecaoNova.Descricao.Should().Be(input.Descricao);
			colecaoNova.Publica.Should().Be(input.Publica);
			colecaoNova.UsuariosColecao.Should().HaveCount(1);
			colecaoNova.Colecoes.Should().BeEmpty();
			colecaoNova.Itens.Should().BeEmpty();
		}

		[Fact]
		public void SucessoCreateSemColecaoMaior()
		{
			ColecaoInputModel input = new ColecaoInputFaker().Generate();
			input.IdColecaoMaior = null;

			repositorioColecao.GetAllSubcolecoes(input.IdDono, input.IdColecaoMaior, input.Nome)
				.Returns(new List<Colecao>());

			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.IdColecaoMaior.Should().BeNull();
			colecaoNova.ColecaoMaior.Should().BeNull();
		}

		[Fact]
		public void SucessoCreateComColecaoMaior()
		{
			ColecaoInputModel input = new ColecaoInputFaker(true).Generate();

			repositorioColecao.GetAllSubcolecoes(input.IdDono, input.IdColecaoMaior, input.Nome)
				.Returns(new List<Colecao>());
			repositorioColecao.GetById((int) input.IdColecaoMaior)
				.Returns(new ColecaoFaker().Generate());

			ColecaoViewModel colecaoNova = service.Create(input);
			colecaoNova.IdColecaoMaior.Should().Be((int)input.IdColecaoMaior);
			colecaoNova.ColecaoMaior.Should().BeNull();
		}

		[Fact]
		public void FalhaCreateComNomeRepetidoDoMesmoPai()
		{
			ColecaoInputModel input = new ColecaoInputFaker(true).Generate();
			ColecaoInputModel chara = new ColecaoInputFaker().Generate();
			chara.Nome = input.Nome;
			chara.IdColecaoMaior = input.IdColecaoMaior;

			repositorioColecao.GetAllSubcolecoes(input.IdDono, input.IdColecaoMaior, input.Nome)
				.Returns(new List<Colecao>() { mapper.Map<Colecao>(chara) });

			Action act = () => service.Create(input);
			act.Should().ThrowExactly<ObjetoDuplicadoException>();
		}
	}
}
