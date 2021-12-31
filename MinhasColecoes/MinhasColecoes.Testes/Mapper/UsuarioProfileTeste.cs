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
			UsuarioInputModel input = new UsuarioInputFaker().Generate();
			Usuario entity = mapper.Map<Usuario>(input);

			entity.Id.Should().Be(0);
			entity.Nome.Should().Be(input.Nome);
			entity.Foto.Should().Be(input.Foto);
			entity.Descricao.Should().Be(input.Descricao);
			entity.Login.Should().Be(input.Login);
			entity.Senha.Should().Be(input.Senha);

			entity.ColecoesDono.Count().Should().Be(0);
			entity.ColecoesParticipa.Count().Should().Be(0);
			entity.ItensDono.Count().Should().Be(0);
			entity.RelacoesItens.Count().Should().Be(0);
		}

		[Fact]
		public void EntityParaBasicView()
		{
			UsuarioBasicViewModel basicView = mapper.Map<UsuarioBasicViewModel>(usuario);

			basicView.Id.Should().Be(usuario.Id);
			basicView.Nome.Should().Be(usuario.Nome);
		}

		[Fact]
		public void EntityParaView()
		{
			UsuarioViewModel basicView = mapper.Map<UsuarioViewModel>(usuario);

			basicView.Id.Should().Be(usuario.Id);
			basicView.Nome.Should().Be(usuario.Nome);
			basicView.Descricao.Should().Be(usuario.Descricao);
			basicView.Foto.Should().Be(usuario.Foto);

			basicView.ColecoesDono.Count().Should().Be(usuario.ColecoesDono.Count());
			basicView.ColecoesMembro.Count().Should().Be(0);
		}

		[Fact]
		public void EntityParaLoginView()
		{
			UsuarioLoginViewModel loginView = mapper.Map<UsuarioLoginViewModel>(usuario);

			loginView.Id.Should().Be(usuario.Id);
			loginView.Nome.Should().Be(usuario.Nome);
			loginView.Login.Should().Be(usuario.Login);
		}
	}
}
