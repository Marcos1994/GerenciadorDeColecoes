using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Entities
{
	public class ColecaoUsuario
	{
		public ColecaoUsuario()
		{

		}
		public ColecaoUsuario(int idUsuario, int idColecao)
		{
			IdUsuario = idUsuario;
			IdColecao = idColecao;
		}

		public int IdUsuario { get; private set; }
		public int IdColecao { get; private set; }
	}
}
