using AutoMapper;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
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
		private readonly IItemRepository repositoryItem;
		private int? _idUsuario;

		public ItemService(IMapper mapper, IItemRepository repositoryItem)
		{
			this.mapper = mapper;
			this.repositoryItem = repositoryItem;
		}

		public void SetUsuario(int idUsuario)
		{
			_idUsuario = idUsuario;
		}

		public ItemViewModel Criar(ItemInputModel input)
		{
			throw new NotImplementedException();
		}

		public void Atualizar(ItemUpdateModel update)
		{
			throw new NotImplementedException();
		}

		public void DefinirRelacoes(List<RelacaoItemUsuarioViewModel> relacoesInput)
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
