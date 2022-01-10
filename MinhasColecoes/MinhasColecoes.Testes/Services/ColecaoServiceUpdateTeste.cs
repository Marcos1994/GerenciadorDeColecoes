using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Models.Update;
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
	public class ColecaoServiceUpdateTeste
	{
		private readonly ColecaoService service;
		private readonly IMapper mapper;
		private readonly IColecaoRepository repositorioColecao;
		private readonly IItemRepository repositorioItem;

		public ColecaoServiceUpdateTeste()
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
		public void SucessoUpdate()
		{
			Colecao colecao = new ColecaoFaker().Generate();
			Colecao colecaoComNomeParecido = new ColecaoFaker().Generate();
			colecaoComNomeParecido.Update(colecao.Nome + " parecido", colecao.Descricao, colecao.Foto, true);
			ColecaoUpdateModel update = new ColecaoUpdateFaker(colecao.Id).Generate();
			int idUsuario = colecao.IdDono;

			repositorioColecao.GetById(update.Id).Returns(colecao);
			repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome)
				.Returns(new List<Colecao>() { colecaoComNomeParecido });
			//repositorioColecao.When(r => r.Update(colecao))
			//	.Do(c => colecao.Update(update.Nome, update.Descricao, update.Foto, update.Publica));

			service.Update(idUsuario, update);

			colecao.Id.Should().Be(colecao.Id);
			colecao.IdDono.Should().Be(colecao.IdDono);
			colecao.IdColecaoMaior.Should().Be(colecao.IdColecaoMaior);

			colecao.Nome.Should().Be(update.Nome);
			colecao.Descricao.Should().Be(update.Descricao);
			colecao.Foto.Should().Be(update.Foto);
			colecao.Publica.Should().Be(update.Publica);
		}

		[Fact]
		public void FalhaUpdateUsuarioNaoEDonoDaColecao()
		{
			Colecao colecao = new ColecaoFaker().Generate();
			ColecaoUpdateModel update = new ColecaoUpdateFaker(colecao.Id).Generate();
			int idUsuario = 0;

			repositorioColecao.GetById(update.Id).Returns(colecao);
			repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome).Returns(new List<Colecao>());

			Action act = () => service.Update(idUsuario, update);
			act.Should().ThrowExactly<UsuarioNaoAutorizadoException>();

			colecao.Nome.Should().Be(colecao.Nome);
			colecao.Descricao.Should().Be(colecao.Descricao);
			colecao.Foto.Should().Be(colecao.Foto);
			colecao.Publica.Should().Be(colecao.Publica);
		}

		[Fact]
		public void FalhaUpdateComColecaoPublicaComNomeRepetidoDeMesmoPai()
		{
			Colecao colecao = new ColecaoFaker(2).Generate();
			colecao.Update(colecao.Nome, colecao.Descricao, colecao.Foto, false);
			int idUsuario = colecao.IdDono;

			Colecao colecaoMesmoNome = new ColecaoFaker(2).Generate();
			colecaoMesmoNome.Update(colecaoMesmoNome.Nome, colecaoMesmoNome.Descricao, colecaoMesmoNome.Foto, true);

			ColecaoUpdateModel update = new ColecaoUpdateFaker(colecao.Id).Generate();
			update.Nome = colecaoMesmoNome.Nome;
			update.Publica = true;

			repositorioColecao.GetById(update.Id).Returns(colecao);
			repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome)
				.Returns(new List<Colecao>() { colecaoMesmoNome });

			Action act = () => service.Update(idUsuario, update);
			act.Should().ThrowExactly<FalhaDeValidacaoException>();

			colecao.Nome.Should().Be(colecao.Nome);
			colecao.Descricao.Should().Be(colecao.Descricao);
			colecao.Foto.Should().Be(colecao.Foto);
			colecao.Publica.Should().Be(colecao.Publica);
		}

		[Fact]
		public void FalhaUpdateComNomeRepetidoDeMesmoPai()
		{
			Colecao colecao = new ColecaoFaker(2).Generate();
			colecao.Update(colecao.Nome, colecao.Descricao, colecao.Foto, true);
			int idUsuario = colecao.IdDono;

			Colecao colecaoMesmoNome = new ColecaoFaker(2).Generate();
			colecaoMesmoNome.Update(colecaoMesmoNome.Nome, colecaoMesmoNome.Descricao, colecaoMesmoNome.Foto, true);

			ColecaoUpdateModel update = new ColecaoUpdateFaker(colecao.Id).Generate();
			update.Nome = colecaoMesmoNome.Nome;
			update.Publica = true;

			repositorioColecao.GetById(update.Id).Returns(colecao);
			repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome)
				.Returns(new List<Colecao>() { colecaoMesmoNome });

			Action act = () => service.Update(idUsuario, update);
			act.Should().ThrowExactly<ObjetoDuplicadoException>();

			colecao.Nome.Should().Be(colecao.Nome);
			colecao.Descricao.Should().Be(colecao.Descricao);
			colecao.Foto.Should().Be(colecao.Foto);
			colecao.Publica.Should().Be(colecao.Publica);
		}

		[Fact]
		public void FalhaUpdatePublicaParaPrivadaComOutrosMembros()
		{
			Colecao colecao = new ColecaoFaker().Generate();
			colecao.Update(colecao.Nome, colecao.Descricao, colecao.Foto, true);
			int idUsuario = colecao.IdDono;

			ColecaoUpdateModel update = new ColecaoUpdateFaker(colecao.Id).Generate();
			update.Publica = false;

			repositorioColecao.GetById(update.Id).Returns(colecao);
			repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome)
				.Returns(new List<Colecao>());
			repositorioColecao.GetMembros(update.Id)
				.Returns(new UsuarioFaker().Generate(3));

			Action act = () => service.Update(idUsuario, update);
			act.Should().ThrowExactly<FalhaDeValidacaoException>();

			colecao.Nome.Should().Be(colecao.Nome);
			colecao.Descricao.Should().Be(colecao.Descricao);
			colecao.Foto.Should().Be(colecao.Foto);
			colecao.Publica.Should().Be(colecao.Publica);
		}
	}
}
