using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class UsuarioBasicViewModel
	{
		public UsuarioBasicViewModel()
		{

		}

		public UsuarioBasicViewModel(int id)
		{
			Id = id;
		}

		public int Id { get; private set; }
		public int Nome { get; private set; }
	}
}
