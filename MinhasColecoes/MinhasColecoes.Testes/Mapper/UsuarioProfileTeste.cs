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
	public class UsuarioProfileTeste
	{
		private readonly IMapper mapper;
		private readonly Usuario usuario;

		public UsuarioProfileTeste()
		{
			usuario = new UsuarioFaker(true).Generate();
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
			UsuarioInputModel source = new UsuarioInputFaker().Generate();
			Usuario destiny = mapper.Map<Usuario>(source);

			destiny.Id.Should().Be(0);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Foto.Should().Be(source.Foto);
			destiny.Descricao.Should().Be(source.Descricao);
			destiny.Login.Should().Be(source.Login);
			destiny.Senha.Should().Be(source.Senha);

			destiny.ColecoesDono.Should().BeEmpty();
			destiny.ColecoesParticipa.Should().BeEmpty();
			destiny.ItensDono.Should().BeEmpty();
			destiny.RelacoesItens.Should().BeEmpty();
		}

		[Fact]
		public void EntityParaBasicView()
		{
			UsuarioBasicViewModel destiny = mapper.Map<UsuarioBasicViewModel>(usuario);

			destiny.Id.Should().Be(usuario.Id);
			destiny.Nome.Should().Be(usuario.Nome);
		}

		[Fact]
		public void EntityParaView()
		{
			UsuarioViewModel destiny = mapper.Map<UsuarioViewModel>(usuario);

			destiny.Id.Should().Be(usuario.Id);
			destiny.Nome.Should().Be(usuario.Nome);
			destiny.Descricao.Should().Be(usuario.Descricao);
			destiny.Foto.Should().Be(usuario.Foto);

			destiny.ColecoesDono.Count().Should().Be(usuario.ColecoesDono.Count());
			destiny.ColecoesMembro.Should().BeEmpty();
		}

		[Fact]
		public void EntityParaLoginView()
		{
			UsuarioLoginViewModel destiny = mapper.Map<UsuarioLoginViewModel>(usuario);

			destiny.Id.Should().Be(usuario.Id);
			destiny.Nome.Should().Be(usuario.Nome);
			destiny.Login.Should().Be(usuario.Login);
		}
	}
}
