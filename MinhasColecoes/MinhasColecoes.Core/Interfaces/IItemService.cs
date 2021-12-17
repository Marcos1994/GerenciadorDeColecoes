using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Interfaces
{
	public interface IItemService
	{
		void SetUsuario(int idUsuario);
		ItemViewModel Criar(ItemInputModel input);
		void Atualizar(ItemUpdateModel update);
		void DefinirRelacoes(List<RelacaoItemUsuarioInputModel> relacoesInput);
		void Excluir(int idItem);
		ItemViewModel GetById(int idItem);
		List<ItemBasicViewModel> GetAll(int idColecao);
		List<ItemBasicViewModel> GetAllOriginais(int idColecao);
	}
}
