using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class UsuarioViewModel : UsuarioBasicViewModel
	{
		public UsuarioViewModel()
		{

		}

		public UsuarioViewModel(int id, string nome, string descricao, string foto)
			: base(id, nome)
		{
			Descricao = descricao;
			Foto = foto;
		}

		public string Descricao { get; private set; }
		public string Foto { get; private set; }
		public List<ColecaoBasicViewModel> ColecoesDono { get; private set; } = new List<ColecaoBasicViewModel>();
		public List<ColecaoBasicViewModel> ColecoesMembro { get; private set; } = new List<ColecaoBasicViewModel>();
	}
}
