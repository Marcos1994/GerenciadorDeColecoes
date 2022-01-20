using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IColecaoRepository : IRepository
	{
		/// <summary>
		/// Retorna todas as coleções que o usuário pode visualizar filtrando pelo nome.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="nome"></param>
		/// <returns>Lista de coleções com as informações básicas.</returns>
		IEnumerable<Colecao> GetAll(int idUsuario, string nome = "");

		/// <summary>
		/// Retorna todas as subcoleções de uma coleção que o usuário pode visualizar.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Lista de coleções com as informações básicas.</returns>
		IEnumerable<Colecao> GetAllSubcolecoes(int idUsuario, int? idColecao, string nome = "");

		/// <summary>
		/// Retorna todas as relações que o usuário possui com os itens da coleção.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Relações de Itens e Usuário.</returns>
		IEnumerable<ItemUsuario> GetAllRelacoesItens(int idUsuario, int idColecao);

		/// <summary>
		/// Retorna todas as coleções que tem o usuário como dono.
		/// </summary>
		/// <param name="idDono"></param>
		/// <returns>Lista de coleções com as informações básicas.</returns>
		IEnumerable<Colecao> GetAllPessoais(int idDono);

		/// <summary>
		/// Retorna todas as coleções das quais o usuário é membro, incluindo ou não as coleções privadas.
		/// </summary>
		/// <param name="idMembro"></param>
		/// <param name="incluirPrivadas"></param>
		/// <returns>Lista de coleções com as informações básicas.</returns>
		IEnumerable<Colecao> GetAllMembro(int idMembro, bool incluirPrivadas = true);

		/// <summary>
		/// Retorna todos os membros de uma coleção.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Lista de usuários com as informações básicas.</returns>
		IEnumerable<Usuario> GetMembros(int idColecao);

		/// <summary>
		/// Retorna A coleção que possui o id correspondente.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Coleção com informações básicas.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		Colecao GetById(int id);

		/// <summary>
		/// Retorna, caso exista, a relação entre um usuário e uma coleção.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Relação entre usuário e coleção.</returns>
		ColecaoUsuario GetRelacao(int idUsuario, int idColecao);

		/// <summary>
		/// Cadastra uma coleção.
		/// </summary>
		/// <param name="colecao"></param>
		void Add(Colecao colecao);

		/// <summary>
		/// Cadastra uma relação entre coleção e usuário.
		/// </summary>
		/// <param name="colecaoUsuario"></param>
		void Add(ColecaoUsuario colecaoUsuario);

		/// <summary>
		/// Atualiza uma coleção.
		/// </summary>
		/// <param name="colecao"></param>
		void Update(Colecao colecao);

		/// <summary>
		/// Exclui uma coleção.
		/// </summary>
		/// <param name="colecao"></param>
		void Delete(Colecao colecao);

		/// <summary>
		/// Exclui uma relação entre coleção e usuário.
		/// </summary>
		/// <param name="colecaoUsuario"></param>
		void Delete(ColecaoUsuario colecaoUsuario);
	}
}
