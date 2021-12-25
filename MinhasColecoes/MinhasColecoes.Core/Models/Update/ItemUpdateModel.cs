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
		public string Nome { get; set; }
		public string Codigo { get; set; }
		public string Descricao { get; set; }
	}
}
