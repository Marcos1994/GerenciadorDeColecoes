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

		public UsuarioBasicViewModel(int id, string nome)
		{
			Id = id;
			Nome = nome;
		}

		public int Id { get; private set; }
		public string Nome { get; private set; }
	}
}
