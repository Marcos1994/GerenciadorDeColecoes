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
		/// <summary>
		/// Define o usuário que irá trabalhar com os itens.
		/// </summary>
		/// <param name="idUsuario"></param>
		void SetUsuario(int idUsuario);

		/// <summary>
		/// Cria um item e cadastra, se houver, a relação desse item com o usuário.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>Item criado.</returns>
		ItemViewModel Criar(ItemInputModel input);

		/// <summary>
		/// Atualiza o item selecionado. Caso o usuário não seja dono da coleção da qual o item faz parte, será criado um item particular para ele com as novas informações.
		/// </summary>
		/// <param name="update"></param>
		/// <returns>Item atualizado.</returns>
		ItemViewModel Atualizar(ItemUpdateModel update);

		/// <summary>
		/// Cria ou atualiza as relações entre itens e usuários.
		/// </summary>
		/// <param name="relacoesInput"></param>
		void DefinirRelacoes(List<RelacaoItemUsuarioInputModel> relacoesInput);

		/// <summary>
		/// Exclui o item.
		/// </summary>
		/// <param name="idItem"></param>
		void Excluir(int idItem);

		/// <summary>
		/// Retorna o item referente ao id com a relação com o usuário e o item original, caso possua.
		/// </summary>
		/// <param name="idItem"></param>
		/// <returns>Item com a relação com o usuário e o item original, caso possua.</returns>
		ItemViewModel GetById(int idItem);

		/// <summary>
		/// Retorna todos os itens que o usuário tem acesso de uma determinada lista.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Lista de itens com os respectivos itens originais, caso tenham, e as relações com o usuário.</returns>
		List<ItemBasicViewModel> GetAll(int idColecao);

		/// <summary>
		/// Retorna todos os itens originais de uma coleção.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Lista de itens originais com as informações básicas de cada item.</returns>
		List<ItemBasicViewModel> GetAllOriginais(int idColecao);
	}
}
