using MinhasColecoes.Aplicacao.Models.Input;
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
	}
}
