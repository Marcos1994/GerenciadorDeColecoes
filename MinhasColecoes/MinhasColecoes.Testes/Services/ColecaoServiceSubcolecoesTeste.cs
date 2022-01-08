using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
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
using Xunit;

namespace MinhasColecoes.Testes.Services
{
	public class ColecaoServiceSubcolecoesTeste
	{
		private readonly ColecaoService service;
		private readonly IMapper mapper;
		private readonly IColecaoRepository repositorioColecao;
		private readonly IItemRepository repositorioItem;

		public ColecaoServiceSubcolecoesTeste()
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
		public void SucessoVerificarGenealogia()
		{
			Colecao colecaoRaiz = new ColecaoFaker().Generate();
			Colecao colecaoFilha = new ColecaoFaker(colecaoRaiz.Id).Generate();
			Colecao colecaoNeta = new ColecaoFaker(colecaoFilha.Id).Generate();

			repositorioColecao.GetById(colecaoRaiz.Id).Returns(colecaoRaiz);
			repositorioColecao.GetById(colecaoFilha.Id).Returns(colecaoFilha);
			repositorioColecao.GetById(colecaoNeta.Id).Returns(colecaoNeta);

			ColecaoGenealogiaViewModel genealogia = service.GetAllSupercolecoes(colecaoNeta.Id);

			service.VerificarGenealogia(genealogia, colecaoRaiz.Id).Should().BeTrue();
			service.VerificarGenealogia(genealogia, 0).Should().BeFalse();
		}

		[Fact]
		public void SucessoAdicionar()
		{
			Colecao super = new ColecaoFaker().Generate();
			Colecao subcolecao = new ColecaoFaker().Generate();
			int idUsuario = subcolecao.IdDono;

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome).Returns(new List<Colecao>());
			repositorioColecao.GetById(super.Id).Returns(super);

			service.AdicionarSupercolecao(idUsuario, subcolecao.Id, super.Id);

			subcolecao.IdColecaoMaior.Should().NotBeNull();
			subcolecao.IdColecaoMaior.Should().Be(super.Id);
		}

		[Fact]
		public void SucessoRemover()
		{
			Colecao subcolecao = new ColecaoFaker(2).Generate();
			int idSupercolecao = (int)subcolecao.IdColecaoMaior;
			int idUsuario = subcolecao.IdDono;

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome).Returns(new List<Colecao>());

			service.AdicionarSupercolecao(idUsuario, subcolecao.Id, null);

			subcolecao.IdColecaoMaior.Should().BeNull();
		}

		[Fact]
		public void FalhaUsuarioNaoEDono()
		{
			Colecao subcolecao = new ColecaoFaker(2).Generate();
			int idSupercolecao = (int) subcolecao.IdColecaoMaior;
			int idUsuario = 0;

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome).Returns(new List<Colecao>());

			Action act = () => service.AdicionarSupercolecao(idUsuario, subcolecao.Id, null);
			act.Should().ThrowExactly<UsuarioNaoAutorizadoException>();

			subcolecao.IdColecaoMaior.Should().Be(idSupercolecao);
		}

		[Fact]
		public void SucessoAdicionarComNomeRepetidoPrivadoNoMesmoPai()
		{
			Colecao subcolecao = new ColecaoFaker().Generate();
			int idUsuario = subcolecao.IdDono;

			Colecao super = new ColecaoFaker().Generate();
			Colecao mesmoNome = new ColecaoFaker(super.Id).Generate();
			mesmoNome.Update(subcolecao.Nome, mesmoNome.Descricao, mesmoNome.Foto, false);

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome)
				.Returns(new List<Colecao>() { mesmoNome });

			Action act = () => service.AdicionarSupercolecao(idUsuario, subcolecao.Id, super.Id);
			act.Should().ThrowExactly<ObjetoDuplicadoException>();

			subcolecao.IdColecaoMaior.Should().BeNull();
		}

		[Fact]
		public void FalhaAdicionarComNomeRepetidoNoMesmoPai()
		{
			Colecao subcolecao = new ColecaoFaker().Generate();
			int idUsuario = subcolecao.IdDono;

			Colecao super = new ColecaoFaker().Generate();
			Colecao mesmoNome = new ColecaoFaker(super.Id).Generate();
			mesmoNome.Update(subcolecao.Nome, mesmoNome.Descricao, mesmoNome.Foto, true);

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome)
				.Returns(new List<Colecao>() { mesmoNome });

			Action act = () => service.AdicionarSupercolecao(idUsuario, subcolecao.Id, super.Id);
			act.Should().ThrowExactly<ObjetoDuplicadoException>();

			subcolecao.IdColecaoMaior.Should().BeNull();
		}

		[Fact]
		public void FalhaRemoverComNomeRepetido()
		{
			Colecao subcolecao = new ColecaoFaker(2).Generate();
			int idSupercolecao = (int)subcolecao.IdColecaoMaior;
			int idUsuario = subcolecao.IdDono;

			Colecao mesmoNome = new ColecaoFaker().Generate();
			mesmoNome.Update(subcolecao.Nome, mesmoNome.Descricao, mesmoNome.Foto, true);

			repositorioColecao.GetById(subcolecao.Id).Returns(subcolecao);
			repositorioColecao.GetAll(idUsuario, subcolecao.Nome)
				.Returns(new List<Colecao>() { mesmoNome });

			Action act = () => service.AdicionarSupercolecao(idUsuario, subcolecao.Id, null);
			act.Should().ThrowExactly<FalhaDeValidacaoException>();

			subcolecao.IdColecaoMaior.Should().Be(idSupercolecao);
		}

		[Fact]
		public void FalhaAdicionarComReferenciaCiclica()
		{
			Colecao colecaoRaiz = new ColecaoFaker().Generate();
			Colecao colecaoFilha = new ColecaoFaker(colecaoRaiz.Id).Generate();
			Colecao colecaoNeta = new ColecaoFaker(colecaoFilha.Id).Generate();

			int idUsuario = colecaoRaiz.IdDono;

			repositorioColecao.GetById(colecaoRaiz.Id).Returns(colecaoRaiz);
			repositorioColecao.GetAll(idUsuario, colecaoRaiz.Nome).Returns(new List<Colecao>());
			repositorioColecao.GetById(colecaoNeta.Id).Returns(colecaoNeta);

			repositorioColecao.GetById(colecaoRaiz.Id).Returns(colecaoRaiz);
			repositorioColecao.GetById(colecaoFilha.Id).Returns(colecaoFilha);
			repositorioColecao.GetById(colecaoNeta.Id).Returns(colecaoNeta);

			Action act = () => service.AdicionarSupercolecao(idUsuario, colecaoRaiz.Id, colecaoNeta.Id);
			act.Should().ThrowExactly<FalhaDeValidacaoException>();

			colecaoRaiz.IdColecaoMaior.Should().BeNull();
		}
	}
}
