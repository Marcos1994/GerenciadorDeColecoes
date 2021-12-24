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

		public ItemService(IMapper mapper, IItemRepository repositoryItem, IColecaoRepository repositoryColecao)
		{
			this.mapper = mapper;
			this.repositorioItem = repositoryItem;
			this.repositorioColecao = repositoryColecao;
		}

		public ItemViewModel Create(ItemInputModel input)
		{
			Item item = mapper.Map<Item>(input);
			Colecao colecao = repositorioColecao.GetById(input.IdColecao);
			if(colecao.IdDono != input.IdUsuario)
			{
				item.SetOriginal(false);
				item.SetDonoParticular(input.IdUsuario);
			}
			else
			{
				Item itemMesmoCodigo = repositorioItem.GetByCodigo(input.IdColecao, input.Codigo);
				if (itemMesmoCodigo != null)
					throw new Exception($"Já existe um item com o código informado.\nNome do item: {itemMesmoCodigo.Nome}.");
			}

			repositorioItem.Add(item);
			return mapper.Map<ItemViewModel>(item);
		}

		public ItemViewModel Update(int idUsuario, ItemUpdateModel update)
		{
			ItemViewModel itemView;
			Item item = repositorioItem.GetById(update.Id, idUsuario);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono == idUsuario || item.IdDonoParticular == idUsuario)
			{
				Item itemMesmoCodigo = repositorioItem.GetByCodigo(colecao.Id, update.Codigo);
				if (itemMesmoCodigo != null && itemMesmoCodigo.Id != update.Id)
					throw new Exception($"Já existe um item com o código informado.\nNome do item: {itemMesmoCodigo.Nome}.");

				item.Update(update.Nome, update.Codigo, update.Descricao);
				repositorioItem.Update(item);
				itemView = mapper.Map<ItemViewModel>(item);
			}
			else
			{
				ItemInputModel input = mapper.Map<ItemInputModel>(update);
				input.IdUsuario = idUsuario;
				input.IdColecao = item.IdColecao;
				try
				{
					repositorioItem.StartTransaction("CriacaoItemParticular");

					if (item.RelacoesUsuarios.Count() > 0)
						repositorioItem.Delete(item.RelacoesUsuarios.First());

					Item novoItem = repositorioItem.GetById(this.Create(input).Id, idUsuario);
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

		public void Delete(int idUsuario, int idItem)
		{
			Item item = repositorioItem.GetById(idItem);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono == idUsuario || item.IdDonoParticular == idUsuario)
				throw new Exception("O usuário atual não tem permissão para excluir o item.");
			repositorioItem.Delete(item);
		}

		public ItemViewModel GetById(int idUsuario, int idItem)
		{
			return mapper.Map<ItemViewModel>(repositorioItem.GetById(idItem, idUsuario));
		}

		public List<ItemBasicViewModel> GetAll(int idUsuario, int idColecao)
		{
			return mapper.Map<List<ItemBasicViewModel>>(repositorioItem.GetAllPessoais(idColecao, idUsuario));
		}

		public List<ItemBasicViewModel> GetAllOriginais(int idColecao)
		{
			return mapper.Map<List<ItemBasicViewModel>>(repositorioItem.GetAll(idColecao));
		}
	}
}
