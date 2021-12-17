using AutoMapper;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Services
{
	public class ItemService : IItemService
	{
		private readonly IMapper mapper;
		private readonly IItemRepository repositorioItem;
		private readonly IColecaoRepository repositorioColecao;
		private int? _idUsuario;

		public ItemService(IMapper mapper, IItemRepository repositoryItem, IColecaoRepository repositoryColecao)
		{
			this.mapper = mapper;
			this.repositorioItem = repositoryItem;
			this.repositorioColecao = repositoryColecao;
		}

		public void SetUsuario(int idUsuario)
		{
			_idUsuario = idUsuario;
		}

		public ItemViewModel Criar(ItemInputModel input)
		{
			input.IdUsuario = (int)_idUsuario;
			Item item = mapper.Map<Item>(input);
			Colecao colecao = repositorioColecao.GetById(input.IdColecao);
			if(colecao.IdDono != _idUsuario)
			{
				item.SetOriginal(false);
				item.SetDonoParticular(_idUsuario);
			}

			repositorioItem.Add(item);
			return mapper.Map<ItemViewModel>(item);
		}

		public void Atualizar(ItemUpdateModel update)
		{
			throw new NotImplementedException();
		}

		public void DefinirRelacoes(List<RelacaoItemUsuarioInputModel> relacoesInput)
		{
			throw new NotImplementedException();
		}

		public void Excluir(int idItem)
		{
			throw new NotImplementedException();
		}

		public ItemViewModel GetById(int idItem)
		{
			throw new NotImplementedException();
		}

		public List<ItemBasicViewModel> GetAll(int idColecao)
		{
			throw new NotImplementedException();
		}

		public List<ItemBasicViewModel> GetAllOriginais(int idColecao)
		{
			throw new NotImplementedException();
		}
	}
}
