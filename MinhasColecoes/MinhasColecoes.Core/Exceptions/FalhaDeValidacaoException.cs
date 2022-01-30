using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Exceptions
{
	public class FalhaDeValidacaoException : Exception
	{
		public Dictionary<string, string[]> Errors { get; private set; } = new Dictionary<string, string[]>();
		public FalhaDeValidacaoException(string mensagem)
			: base(mensagem)
		{

		}
		public FalhaDeValidacaoException(string atributo, string erro)
			: base("Erro ao validar.")
		{
			Errors.Add(atributo, new string[] { erro });
		}
	}
}
