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
	public interface IColecaoService
	{
		/// <summary>
		/// Cria uma coleção e adiciona o usuário atual como dono e membro dela. Os itens da coleção deverão ser cadastrados posteriormente.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="input"></param>
		/// <returns>Coleção criada.</returns>
		/// <exception cref="ObjetoDuplicadoException"></exception>
		ColecaoViewModel Create(ColecaoInputModel input);

		/// <summary>
		/// Atualiza as informações básicas da coleção.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="update"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		/// <exception cref="ObjetoDuplicadoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		void Update(int idUsuario, ColecaoUpdateModel update);

		/// <summary>
		/// Transfere uma coleção do dono para um membro desta coleção. A transferência só será concluída quando o membro aceitar a transferência.
		/// </summary>
		/// <param name="idDono"></param>
		/// <param name="idMembro"></param>
		/// <param name="idColecao"></param>
		void TransferirParaMembro(int idDono, int idMembro, int idColecao);

		/// <summary>
		/// Adiciona uma coleção como subcoleção de outra. A coleção maior pode ser de outro usuário, mas o usuário que está realizando a ação deve ser dono da subcoleção. 
		/// </summary>
		/// <param name="idSubcolecao"></param>
		/// <param name="idColecao"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		/// <exception cref="ObjetoDuplicadoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		void AdicionarSupercolecao(int idUsuario, int idSubcolecao, int? idColecao);

		/// <summary>
		/// Adiciona o usuário como membro da coleção.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		void AdicionarMembro(int idUsuario, int idColecao);

		/// <summary>
		/// Remove a relação entre o usuário e a coleção. Caso o usuário seja o dono e não haja nenhum membro nela, a coleção será excluída.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		void Delete(int idUsuario, int idColecao);

		/// <summary>
		/// Retorna a coleção referente ao id.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Coleção com informações basicas.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		/// <exception cref="UsuarioNaoAutorizadoException"></exception>
		ColecaoViewModel GetById(int idUsuario, int idColecao);

		/// <summary>
		/// Retorna todas as coleções visíveis para o usuário cujo nome contenha o parâmetro passado.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="nome"></param>
		/// <returns>Lista de coleções com suas informações básicas</returns>
		IEnumerable<ColecaoBasicViewModel> GetAll(int idUsuario, string nome = "");

		/// <summary>
		/// Retorna todas as coleções cujo o usuário é dono e que nome contenha o parâmetro passado.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="nome"></param>
		/// <returns>Lista de coleções com suas informações básicas</returns>
		IEnumerable<ColecaoBasicViewModel> GetAllProprias(int idUsuario, string nome = "");

		/// <summary>
		/// Retorna todas as coleções que o usuário participa e que nome contenha o parâmetro passado. Se o usuário for o mesmo usuário autenticado, as consulta incluirá as coleções privadas.
		/// </summary>
		/// <param name="idAutenticado"></param>
		/// <param name="idUsuario"></param>
		/// <param name="nome"></param>
		/// <returns>Lista de coleções com suas informações básicas</returns>
		IEnumerable<ColecaoBasicViewModel> GetAllParticipa(int idAutenticado, int idUsuario, string nome = "");

		/// <summary>
		/// Retorna todas as subcoleções de uma coleção visíveis para o usuário.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idColecao"></param>
		/// <returns>Lista de coleções com suas informações básicas</returns>
		IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idUsuario, int? idColecao, string nome = "");

		/// <summary>
		/// Retorna todas as supercoleções aninhadas desta coleção desde a raiz até ela.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Coleção raiz com as subcoleções aninhadas até a coleção desejada.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		ColecaoGenealogiaViewModel GetAllSupercolecoes(int idColecao);

		/// <summary>
		/// Retorna todos os membros de uma coleção
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Lista de usuários com suas informações básicas</returns>
		IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao);
	}
}
