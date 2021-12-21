using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IUsuarioRepository
	{
		/// <summary>
		/// Retorna o usuário que tenha a combinação de Login e Senha informados.
		/// </summary>
		/// <param name="login"></param>
		/// <param name="senha"></param>
		/// <returns>Usuário com informações básicas.</returns>
		Usuario Get(string login, string senha);

		/// <summary>
		/// Retorna o usuário com o ID informado.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Usuário com informações básicas.</returns>
		Usuario GetById(int id);

		/// <summary>
		/// Retorna o usuário com o login informado.
		/// </summary>
		/// <param name="login"></param>
		/// <returns>Usuário com informações básicas.</returns>
		Usuario GetByLogin(string login);

		/// <summary>
		/// Cadastra um usuário.
		/// </summary>
		/// <param name="usuario"></param>
		void Create(Usuario usuario);

		/// <summary>
		/// Atualiza as informações do usuário.
		/// </summary>
		/// <param name="usuario"></param>
		void Update(Usuario usuario);
	}
}
