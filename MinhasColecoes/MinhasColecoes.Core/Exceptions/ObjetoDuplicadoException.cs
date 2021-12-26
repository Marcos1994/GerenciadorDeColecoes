using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Exceptions
{
	public class ObjetoDuplicadoException : Exception
	{
		public ObjetoDuplicadoException(string objeto)
			: base($"Detecção de {objeto} duplicata.")
		{

		}

		public ObjetoDuplicadoException(string objeto, string campoDuplicata)
			: base($"Já existe um registro de {objeto} com este {campoDuplicata}.")
		{

		}

		public ObjetoDuplicadoException(string objeto, string campoDuplicata, string mensagemAdicional)
			: base($"Já existe um registro de {objeto} com este {campoDuplicata}.\n{mensagemAdicional}")
		{

		}
	}
}
