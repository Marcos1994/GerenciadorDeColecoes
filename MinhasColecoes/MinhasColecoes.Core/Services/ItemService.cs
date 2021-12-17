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

		public ItemViewModel Atualizar(ItemUpdateModel update)
		{
			ItemViewModel itemView;
			Item item = repositorioItem.GetById(update.Id, (int)_idUsuario);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono == _idUsuario || item.IdDonoParticular == _idUsuario)
			{
				item.Update(update.Nome, update.Codigo, update.Descricao);
				repositorioItem.Update(item);
				itemView = mapper.Map<ItemViewModel>(item);
			}
			else
			{
				ItemInputModel input = mapper.Map<ItemInputModel>(update);
				input.IdColecao = item.IdColecao;
				try
				{
					repositorioItem.StartTransaction("CriacaoItemParticular");

					if (item.RelacoesUsuarios.Count() > 0)
						repositorioItem.Delete(item.RelacoesUsuarios.First());

					Item novoItem = repositorioItem.GetById(this.Criar(input).Id, (int)_idUsuario);
					novoItem.SetItemOriginal(item);
					novoItem.SetOriginal(false);
					repositorioItem.Update(novoItem);

					repositorioItem.FinishTransaction();
					itemView = mapper.Map<ItemViewModel>(novoItem);
				}
				catch (Exception ex)
				{
					repositorioItem.RollbackTransaction("CriacaoItemParticular");
					throw new Exception($"Não foi possível atualizar o item {item.Nome}.\n{ex.Message}");
				}
			}
			return itemView;
		}

		public void DefinirRelacoes(List<RelacaoItemUsuarioInputModel> relacoesInput)
		{
			throw new NotImplementedException();
		}

		public void Excluir(int idItem)
		{
			Item item = repositorioItem.GetById(idItem);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono == _idUsuario || item.IdDonoParticular == _idUsuario)
				throw new Exception("O usuário atual não tem permissão para excluir o item.");
			repositorioItem.Delete(item);
		}

		public ItemViewModel GetById(int idItem)
		{
			return mapper.Map<ItemViewModel>(repositorioItem.GetById(idItem, (int)_idUsuario));
		}

		public List<ItemBasicViewModel> GetAll(int idColecao)
		{
			return mapper.Map<List<ItemBasicViewModel>>(repositorioItem.GetAllPessoais(idColecao, (int)_idUsuario));
		}

		public List<ItemBasicViewModel> GetAllOriginais(int idColecao)
		{
			return mapper.Map<List<ItemBasicViewModel>>(repositorioItem.GetAll(idColecao));
		}
	}
}
