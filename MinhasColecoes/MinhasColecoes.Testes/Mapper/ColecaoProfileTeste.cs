using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Aplicacao.Profiles;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Testes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MinhasColecoes.Testes.Mapper
{
	public class ColecaoProfileTeste
	{
		private readonly Colecao colecao;
		private readonly IMapper mapper;

		public ColecaoProfileTeste()
		{
			colecao = new ColecaoFaker(true).Generate();
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
		}

		[Fact]
		public void InputParaEntity()
		{
			ColecaoInputModel source = new ColecaoInputFaker().Generate();
			Colecao destiny = mapper.Map<Colecao>(source);

			destiny.Id.Should().Be(0);
			destiny.IdDono.Should().Be(source.IdDono);
			destiny.IdColecaoMaior.Should().Be(source.IdColecaoMaior);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Descricao.Should().Be(source.Descricao);
			destiny.Publica.Should().Be(source.Publica);

			destiny.Colecoes.Should().BeEmpty();
			destiny.UsuariosColecao.Should().HaveCount(1);
			destiny.UsuariosColecao.First().IdUsuario.Should().Be(source.IdDono);
			destiny.UsuariosColecao.First().IdColecao.Should().Be(0);
			destiny.Itens.Should().BeEmpty();
		}

		[Fact]
		public void EntityParaBasicView()
		{
			ColecaoBasicViewModel destiny = mapper.Map<ColecaoBasicViewModel>(colecao);

			destiny.Id.Should().Be(colecao.Id);
			destiny.Nome.Should().Be(colecao.Nome);
			destiny.Descricao.Should().Be(colecao.Descricao);
			destiny.Publica.Should().Be(colecao.Publica);
		}

		[Fact]
		public void EntityParaView()
		{
			ColecaoViewModel destiny = mapper.Map<ColecaoViewModel>(colecao);

			destiny.Id.Should().Be(colecao.Id);
			destiny.Nome.Should().Be(colecao.Nome);
			destiny.Descricao.Should().Be(colecao.Descricao);
			destiny.Publica.Should().Be(colecao.Publica);

			destiny.IdDono.Should().Be(colecao.IdDono);
			destiny.ColecaoMaior.Should()
				.Match<ColecaoBasicViewModel>(c => 
				(c == null && colecao.IdColecaoMaior == null)
				|| (c.Id == colecao.IdColecaoMaior));
			destiny.UsuariosColecao.Should().HaveCount(colecao.UsuariosColecao.Count);
			destiny.Colecoes.Should().HaveCount(colecao.Colecoes.Count());
			destiny.Itens.Should().HaveCount(colecao.Itens.Count());
		}

		[Fact]
		public void EntityParaGenealogiaView()
		{
			ColecaoGenealogiaViewModel destiny = mapper.Map<ColecaoGenealogiaViewModel>(colecao);

			destiny.Id.Should().Be(colecao.Id);
			destiny.Nome.Should().Be(colecao.Nome);
			destiny.Publica.Should().Be(colecao.Publica);
			destiny.IdDono.Should().Be(colecao.IdDono);
			destiny.IdColecaoMaior.Should().Be(colecao.IdColecaoMaior);
		}
	}
}
