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
		/// Cria um item e cadastra, se houver, a relação desse item com o usuário.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>Item criado.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		ItemViewModel Create(ItemInputModel input);

		/// <summary>
		/// Transforma o item particular em um item original. Se o item possuir uma versão original, a relação entre o usuário e o item será transferida para o novo item.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idItem"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		void Oficializar(int idUsuario, int idItem);

		/// <summary>
		/// Atualiza o item selecionado. Caso o usuário não seja dono da coleção da qual o item faz parte, será criado um item particular para ele com as novas informações.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="update"></param>
		/// <returns>Item atualizado.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		ItemViewModel Update(int idUsuario, ItemUpdateModel update);

		/// <summary>
		/// Cria ou atualiza a relação entre o item e o usuário.
		/// </summary>
		/// <param name="relacoesInput"></param>
		void DefinirRelacao(RelacaoItemUsuarioInputModel relacaoInput);

		/// <summary>
		/// Exclui o item.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idItem"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		void Delete(int idUsuario, int idItem);

		/// <summary>
		/// Retorna o item referente ao id com a relação com o usuário e o item original, caso possua.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idItem"></param>
		/// <returns>Item com a relação com o usuário e o item original, caso possua.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		ItemViewModel GetById(int idUsuario, int idItem);

		/// <summary>
		/// Retorna todos os itens que o usuário tem acesso de uma determinada lista.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Lista de itens com os respectivos itens originais, caso tenham, e as relações com o usuário.</returns>
		IEnumerable<ItemBasicViewModel> GetAll(int idUsuario, int idColecao);

		/// <summary>
		/// Retorna todos os itens originais de uma coleção.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Lista de itens originais com as informações básicas de cada item.</returns>
		IEnumerable<ItemBasicViewModel> GetAllOriginais(int idColecao);

		/// <summary>
		/// Retorna todos os itens particulares que não são versões de um item original da coleção. Apenas o dono da coleção pode listar esses itens.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Lista de itens particulares com as informações básicas de cada item.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		IEnumerable<ItemBasicViewModel> GetAllParticularesColecao(int idUsuario, int idColecao);

		/// <summary>
		/// Retorna todas as versões particulares de um item. Apenas o dono da coleção pode listar esses itens.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idItemOficial"></param>
		/// <returns>Lista de itens particulares com as informações básicas de cada item.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		IEnumerable<ItemBasicViewModel> GetAllParticularesItem(int idUsuario, int idItemOficial);
	}
}
