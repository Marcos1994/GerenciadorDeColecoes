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
	public interface IUsuarioService
	{
		/// <summary>
		/// Procura por usuário com a combinação de Login e Senha. Caso encontre, retorna o usuário logado com o token.
		/// </summary>
		/// <param name="usuario"></param>
		/// <returns>Usuario com login e token.</returns>
		UsuarioLoginViewModel ValidarUsuario(UsuarioLoginInputModel usuario);

		/// <summary>
		/// Busca um usuário pelo Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Usuário com informações básicas.</returns>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		UsuarioViewModel GetById(int id);

		/// <summary>
		/// Cadastra um usuário com login único.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>Usuário cadastrado.</returns>
		/// <exception cref="ObjetoDuplicadoException"></exception>
		UsuarioViewModel Create(UsuarioInputModel input);

		/// <summary>
		/// Atualiza as informações do usuário.
		/// </summary>
		/// <param name="update"></param>
		/// <exception cref="ObjetoNaoEncontradoException"></exception>
		void Update(UsuarioUpdateModel update);

		/// <summary>
		/// Atualiza a senha do usuário.
		/// </summary>
		/// <param name="update"></param>
		/// <exception cref="FalhaDeValidacaoException"></exception>
		void Update(UsuarioSenhaUpdateModel update);
	}
}
