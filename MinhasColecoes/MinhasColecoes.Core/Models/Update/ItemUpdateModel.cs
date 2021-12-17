using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Update
{
	public class ItemUpdateModel
	{
		public int Id { get; set; }
		public string Nome { get; private set; }
		public string Codigo { get; private set; }
		public string Descricao { get; private set; }
	}
}
