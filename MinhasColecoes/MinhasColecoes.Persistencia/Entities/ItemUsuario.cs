using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Entities
{
	public class ItemUsuario
	{
		public ItemUsuario()
		{

		}
		public ItemUsuario(int idUsuario, int idItem, int relacao)
		{
			IdUsuario = idUsuario;
			IdItem = idItem;
			Relacao = relacao;
		}

		public int IdUsuario { get; private set; }
		public int IdItem { get; private set; }
		public int Relacao { get; private set; }
	}
}
