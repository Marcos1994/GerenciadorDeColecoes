using AutoMapper;
using FluentAssertions;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
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
	public class ItemProfileTeste
	{
		private readonly IMapper mapper;

		public ItemProfileTeste()
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
		}

		[Fact]
		public void UpdateParaInput()
		{
			ItemUpdateModel source = new ItemUpdateFaker().Generate();
			ItemInputModel destiny = mapper.Map<ItemInputModel>(source);

			destiny.IdColecao.Should().Be(0);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Codigo.Should().Be(source.Codigo);
			destiny.Descricao.Should().Be(source.Descricao);
		}

		[Fact]
		public void InputParaEntity()
		{
			ItemInputModel source = new ItemInputFaker(EnumRelacaoUsuarioItem.NaoPossuo).Generate();
			Item destiny = mapper.Map<Item>(source);

			destiny.IdColecao.Should().Be(source.IdColecao);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Codigo.Should().Be(source.Codigo);
			destiny.Descricao.Should().Be(source.Descricao);

			//Sem relação
			destiny.RelacoesUsuarios.Should().BeEmpty();

			//Com relação
			source.Relacao = EnumRelacaoUsuarioItem.Possuo;
			destiny = mapper.Map<Item>(source);

			destiny.RelacoesUsuarios.Should().HaveCount(1);
			destiny.RelacoesUsuarios.Single().IdUsuario.Should().Be(source.IdUsuario);
			destiny.RelacoesUsuarios.Single().Relacao.Should().Be((int)source.Relacao);
			destiny.RelacoesUsuarios.Single().Comentario.Should().Be(source.Comentario);
		}

		[Fact]
		public void EntityParaBasicView()
		{
			//Item Original Sem relação
			Item source = new ItemFaker(1, 1, EnumRelacaoUsuarioItem.NaoPossuo, true).Generate();
			ItemBasicViewModel destiny = mapper.Map<ItemBasicViewModel>(source);

			destiny.Id.Should().Be(source.Id);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Codigo.Should().Be(source.Codigo);
			destiny.Descricao.Should().Be(source.Descricao);
			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().BeNull();
			destiny.ItemOriginal.Should().BeNull();
			destiny.Relacao.Should().Be(EnumRelacaoUsuarioItem.NaoPossuo);

			//Item Particular Com Original Com relação
			source = new ItemFaker(1, 1, EnumRelacaoUsuarioItem.Possuo, false).Generate();
			destiny = mapper.Map<ItemBasicViewModel>(source);

			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().Be(source.IdOriginal);
			destiny.ItemOriginal.Should().NotBeNull();
			destiny.ItemOriginal.Id.Should().Be(source.IdOriginal);
			destiny.Relacao.Should().Be(source.RelacoesUsuarios.Single().Relacao);
			destiny.Comentario.Should().Be(source.RelacoesUsuarios.Single().Comentario);

			//Item Particular Sem Original
			source = new ItemFaker(1, null).Generate();
			destiny = mapper.Map<ItemBasicViewModel>(source);

			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().BeNull();
			destiny.ItemOriginal.Should().BeNull();
		}

		[Fact]
		public void EntityParaView()
		{
			//Item Original Sem relação
			Item source = new ItemFaker(1, 1, EnumRelacaoUsuarioItem.NaoPossuo, true).Generate();
			ItemViewModel destiny = mapper.Map<ItemViewModel>(source);

			destiny.Id.Should().Be(source.Id);
			destiny.Nome.Should().Be(source.Nome);
			destiny.Codigo.Should().Be(source.Codigo);
			destiny.Descricao.Should().Be(source.Descricao);
			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().BeNull();
			destiny.ItemOriginal.Should().BeNull();
			destiny.Relacao.Should().Be(EnumRelacaoUsuarioItem.NaoPossuo);

			//Item Particular Com Original Com relação
			source = new ItemFaker(1, 1, EnumRelacaoUsuarioItem.Possuo, false).Generate();
			destiny = mapper.Map<ItemViewModel>(source);

			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().Be(source.IdOriginal);
			destiny.ItemOriginal.Should().NotBeNull();
			destiny.ItemOriginal.Id.Should().Be(source.IdOriginal);
			destiny.Relacao.Should().Be(source.RelacoesUsuarios.Single().Relacao);
			destiny.Comentario.Should().Be(source.RelacoesUsuarios.Single().Comentario);

			//Item Particular Sem Original Com relação
			source = new ItemFaker(1, null).Generate();
			destiny = mapper.Map<ItemViewModel>(source);

			destiny.Original.Should().Be(source.Original);
			destiny.IdOriginal.Should().BeNull();
			destiny.ItemOriginal.Should().BeNull();
		}
	}
}
