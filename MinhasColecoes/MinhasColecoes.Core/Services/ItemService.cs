using AutoMapper;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Aplicacao.Exceptions;
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
					//throw new FalhaDeValidacaoException("Item", "Código", $"Nome do item: {itemMesmoCodigo.Nome}.");
					throw new FalhaDeValidacaoException(
						"Código", $"O código já pertence ao item {itemMesmoCodigo.Nome}");
			}
			else
			{
				item.SetDonoParticular(input.IdUsuario);
			}

			repositorioItem.Add(item);
			if(item.RelacoesUsuarios.Count > 0)
			{
				ColecaoUsuario relacao = repositorioColecao.GetRelacao(input.IdUsuario, input.IdColecao);
				if(relacao == null)
					repositorioColecao.Add(new ColecaoUsuario(input.IdUsuario, input.IdColecao));
			}
			return mapper.Map<ItemViewModel>(item);
		}

		public void Oficializar(int idUsuario, int idItemParticular)
		{
			Item itemParticular = repositorioItem.GetById(idItemParticular);

			Colecao colecao = repositorioColecao.GetById(itemParticular.IdColecao);
			if (colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("oficializar", "item");

			Item itemMesmoCodigo = repositorioItem.GetByCodigo(colecao.Id, itemParticular.Codigo);
			Item itemOficial = (itemParticular.IdOriginal != null)
				? repositorioItem.GetById((int)itemParticular.IdOriginal)
				: null;

			//Verifica se existe algum item com o novo código que não seja o item original.
			if (itemMesmoCodigo != null && (itemOficial == null || itemMesmoCodigo.Id != itemOficial.Id))
				throw new FalhaDeValidacaoException(
					"Código", $"Já existe um item oficial com este código: {itemMesmoCodigo.Nome}");

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
						itemOficial.RelacoesUsuarios.Add(new ItemUsuario(relacao.IdUsuario, itemOficial.Id, relacao.Relacao, relacao.Comentario));

					//Atualiza o item oficial e exclui o item particular
					itemOficial.Update(itemParticular.Nome, itemParticular.Codigo, itemParticular.Descricao, itemParticular.Foto);
					repositorioItem.Update(itemOficial);
					repositorioItem.Delete(itemParticular);

					repositorioItem.FinishTransaction();
				}
				catch (Exception ex)
				{
					repositorioItem.RollbackTransaction("OficializacaoItem");
					throw new Exception($"Não foi possível oficializar o item {itemParticular.Nome}.\n{ex.Message}", ex.InnerException);
				}

				itemParticular = itemOficial;
			}
		}

		public ItemViewModel Update(int idUsuario, ItemUpdateModel update)
		{
			ItemViewModel itemView;
			Item item = repositorioItem.GetById(update.Id, idUsuario);

			if (item.IdDonoParticular != null && item.IdDonoParticular != idUsuario)
				throw new UsuarioNaoAutorizadoException("oficializar", "item");

			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono == idUsuario || item.IdDonoParticular == idUsuario)
			{
				Item itemMesmoCodigo = repositorioItem.GetByCodigo(colecao.Id, update.Codigo);
				if (itemMesmoCodigo != null && itemMesmoCodigo.Id != update.Id)
					throw new FalhaDeValidacaoException(
						"Código", $"O código já pertence ao item {itemMesmoCodigo.Nome}");

				item.Update(update.Nome, update.Codigo, update.Descricao, update.Foto);
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

		public void DefinirRelacao(RelacaoItemUsuarioInputModel relacaoInput)
		{
			ItemUsuario relacao = repositorioItem.GetByKey(relacaoInput.IdUsuario, relacaoInput.IdItem);
			Item item = repositorioItem.GetById(relacaoInput.IdItem, relacaoInput.IdUsuario);

			if (relacaoInput.Relacao == EnumRelacaoUsuarioItem.NaoPossuo)
			{
				if (relacao != null)
				{
					Colecao colecao = repositorioColecao.GetById(item.IdColecao);

					repositorioItem.Delete(relacao);

					if (colecao.IdDono != relacao.IdUsuario)
					{
						ColecaoUsuario colecaoUsuario = repositorioColecao.GetRelacao(relacao.IdUsuario, colecao.Id);
						if (colecaoUsuario != null &&
							repositorioColecao.GetAllRelacoesItens(colecaoUsuario.IdUsuario, colecaoUsuario.IdColecao).Count() == 0)
							repositorioColecao.Delete(colecaoUsuario);
					}
				}
			}
			else
			{
				if (relacao == null)
					repositorioItem.Add(mapper.Map<ItemUsuario>(relacaoInput));
				else
				{
					relacao.Update((int)relacaoInput.Relacao, relacaoInput.Comentario);
					repositorioItem.Update(relacao);
				}

				if (item.RelacoesUsuarios.Count > 0)
				{
					ColecaoUsuario colecaoUsuario = repositorioColecao.GetRelacao(relacaoInput.IdUsuario, item.IdColecao);
					if (colecaoUsuario == null)
						repositorioColecao.Add(new ColecaoUsuario(relacaoInput.IdUsuario, item.IdColecao));
				}
			}
		}

		public void Delete(int idUsuario, int idItem)
		{
			Item item = repositorioItem.GetById(idItem);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono != idUsuario || item.IdDonoParticular != idUsuario)
				throw new UsuarioNaoAutorizadoException("excluir", "item");

			List<ItemUsuario> relacoes = repositorioItem.GetAllRelacoes(idItem).ToList();
			if (relacoes.Count > 1 || relacoes.Any(iu => iu.IdUsuario != idUsuario))
				throw new FalhaDeValidacaoException("Não é possível excluir um item que possua relação com algum usuário.");

			repositorioItem.Delete(item);

			if (colecao.IdDono != idUsuario)
			{
				ColecaoUsuario colecaoUsuario = repositorioColecao.GetRelacao(idUsuario, colecao.Id);
				if (colecaoUsuario != null &&
					repositorioColecao.GetAllRelacoesItens(colecaoUsuario.IdUsuario, colecaoUsuario.IdColecao).Count() == 0)
					repositorioColecao.Delete(colecaoUsuario);
			}
		}

		public ItemViewModel GetById(int idUsuario, int idItem)
		{
			return mapper.Map<ItemViewModel>(repositorioItem.GetById(idItem, idUsuario));
		}

		public IEnumerable<ItemBasicViewModel> GetAll(int idUsuario, int idColecao, string nome = "")
		{
			IEnumerable<Item> itens = repositorioItem.GetAllPessoais(idColecao, idUsuario);
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(
				string.IsNullOrEmpty(nome)
				? itens
				: itens.Where(i => i.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)));
		}

		public IEnumerable<ItemBasicViewModel> GetAllOriginais(int idColecao)
		{
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAll(idColecao));
		}

		public IEnumerable<ItemBasicViewModel> GetAllParticularesColecao(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("acessar os itens particulares da coleção");
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAllParticularesColecao(idColecao));
		}

		public IEnumerable<ItemBasicViewModel> GetAllParticularesItem(int idUsuario, int idItemOficial)
		{
			Item item = repositorioItem.GetById(idItemOficial, idUsuario);
			Colecao colecao = repositorioColecao.GetById(item.IdColecao);
			if (colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("acessar os itens particulares deste item");
			return mapper.Map<IEnumerable<ItemBasicViewModel>>(repositorioItem.GetAllParticularesItem(idItemOficial));
		}
	}
}
