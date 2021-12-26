using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Exceptions
{
	public class ObjetoNaoEncontradoException : Exception
	{
		public ObjetoNaoEncontradoException(string tipoObjeto, Exception innerException)
			: base($"{tipoObjeto} não encontrado.", innerException)
		{
		}
	}
}
