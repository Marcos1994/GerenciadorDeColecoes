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
			item.SetOriginal(colecao.IdDono == input.IdUsuario);
			if (item.Original)
			{
				Item itemMesmoCodigo = repositorioItem.GetByCodigo(input.IdColecao, input.Codigo);
				if (itemMesmoCodigo != null)
					throw new Exception($"Já existe um item com o código informado.\nNome do item: {itemMesmoCodigo.Nome}.");
			}
			else
			{
				item.SetDonoParticular(input.IdUsuario);
			}

			repositorioItem.Add(item);
			return mapper.Map<ItemViewModel>(item);
		}

		public void Oficializar(int idUsuario, int idItemParticular)
		{
			Item itemParticular = repositorioItem.GetById(idItemParticular);

			Colecao colecao = repositorioColecao.GetById(itemParticular.IdColecao);
			if (colecao.IdDono != idUsuario)
				throw new Exception("O usuário não tem permissão para oficializar o item.");

			Item itemMesmoCodigo = repositorioItem.GetByCodigo(colecao.Id, itemParticular.Codigo);
			Item itemOficial = (itemParticular.IdOriginal != null)
				? repositorioItem.GetById((int)itemParticular.IdOriginal)
				: null;

			//Verifica se existe algum item com o novo código que não seja o item original.
			if (itemMesmoCodigo != null && (itemOficial == null || itemMesmoCodigo.Id != itemOficial.Id))
				throw new Exception($"Já existe um item com o código informado.\nNome do item: {itemMesmoCodigo.Nome}.");

			if (itemOficial == null) //Item novo não oficial
			{
				itemParticular.SetOriginal(true);
				itemParticular.SetDonoParticular(null);
				repositorioItem.Update(itemParticular);
			}
			else //Versão não oficial de um item
			{
				try
				{
					repositorioItem.StartTransaction("OficializacaoItem");

					//Exclui alguma possível relação entre o dono do item particular com o item oficial.
					ItemUsuario relacao = repositorioItem.GetByKey((int)itemParticular.IdDonoParticular, itemOficial.Id);
					if(relacao != null)
						repositorioItem.Delete(relacao);

					//Transfere a relação do item particular para o item oficial.
					relacao = repositorioItem.GetByKey((int)itemParticular.IdDonoParticular, itemParticular.Id);
					if (relacao != null)
						itemOficial.RelacoesUsuarios.Add(new ItemUsuario(relacao.IdUsuario, itemOficial.Id, relacao.Relacao));

					//Atualiza o item oficial e exclui o item particular
					itemOficial.Update(itemParticular.Nome, itemParticular.Codigo, itemParticular.Descricao);
					repositorioItem.Update(itemOficial);
					repositorioItem.Delete(itemParticular);

					repositorioItem.FinishTransaction();
				}
				catch (Exception ex)
				{
					repositorioItem.RollbackTransaction("OficializacaoItem");
					throw new Exception($"Não foi possível tornar o item {itemParticular.Nome} oficial.\n{ex.Message}");
				}

				itemParticular = itemOficial;
			}
		}

		public ItemViewModel Update(int idUsuario, ItemUpdateModel update)
		{
			ItemViewModel itemView;
			Item item = repositorioItem.GetById(update.Id, idUsuario);

			if (item.IdDonoParticular != null && item.IdDonoParticular != idUsuario)
				throw new Exception("O usuário não tem permissão para editar o item.");

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

		public void DefinirRelacoes(RelacaoItemUsuarioInputModel relacaoInput)
		{
			ItemUsuario relacao = mapper.Map<ItemUsuario>(relacaoInput);
			ItemUsuario relacaoExistente = repositorioItem.GetByKey(relacaoInput.IdItem, relacaoInput.IdUsuario);

			if(relacao == null)
			{
				if (relacaoExistente != null)
					repositorioItem.Delete(relacaoExistente);
			}
			else
			{
				if (relacaoExistente == null)
					repositorioItem.Add(relacao);
				else
					repositorioItem.Update(relacao);
			}
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

		public IEnumerable<ItemBasicViewModel> GetAll(int idUsuario, int idColecao)
		{
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAllPessoais(idColecao, idUsuario));
		}

		public IEnumerable<ItemBasicViewModel> GetAllOriginais(int idColecao)
		{
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAll(idColecao));
		}

		public IEnumerable<ItemBasicViewModel> GetAllParticularesColecao(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (colecao.IdDono != idUsuario)
				throw new Exception("Apenas o dono da coleção pode ter acesso aos itens particulares dela.");
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAllParticularesColecao(idColecao));
		}
		public IEnumerable<ItemBasicViewModel> GetAllParticularesItem(int idUsuario, int idItemOficial)
		{
			Item item = repositorioItem.GetById(idItemOficial, idUsuario);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono != idUsuario)
				throw new Exception("Apenas o dono da coleção pode ter acesso aos itens particulares do item.");
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAllParticularesItem(idItemOficial));
		}
	}
}
