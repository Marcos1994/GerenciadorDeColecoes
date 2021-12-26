using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Exceptions
{
	public class UsuarioNaoAutorizadoException : Exception
	{
		public UsuarioNaoAutorizadoException(string acao)
			: base($"O Usuário não tem autorização para {acao}.")
		{

		}
		public UsuarioNaoAutorizadoException(string acao, string objeto)
			:base($"O Usuário não tem autorização para {acao} o/a {objeto} desejado.")
		{

		}
	}
}
