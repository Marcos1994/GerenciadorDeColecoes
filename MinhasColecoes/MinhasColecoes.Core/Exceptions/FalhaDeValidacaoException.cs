using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Exceptions
{
	public class FalhaDeValidacaoException : Exception
	{
		public FalhaDeValidacaoException(string mensagem)
			: base(mensagem)
		{

		}
	}
}
