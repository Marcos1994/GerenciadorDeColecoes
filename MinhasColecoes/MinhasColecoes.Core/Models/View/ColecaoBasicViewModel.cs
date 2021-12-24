using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class ColecaoBasicViewModel
	{
		public ColecaoBasicViewModel()
		{

		}

		public ColecaoBasicViewModel(int id)
		{
			Id = id;
		}

		public int Id { get; private set; }
		public string Nome { get; private set; }
		public string Descricao { get; private set; }
		public string Foto { get; private set; }
		public bool Publica { get; private set; }
	}
}
