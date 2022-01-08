using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class ColecaoViewModel : ColecaoBasicViewModel
	{
		public UsuarioBasicViewModel Dono { get; private set; }
		public ColecaoGenealogiaViewModel ColecaoMaior { get; private set; }
		public List<UsuarioBasicViewModel> UsuariosColecao { get; private set; }
		public List<ColecaoBasicViewModel> Colecoes { get; private set; } = new List<ColecaoBasicViewModel>();
		public List<ItemBasicViewModel> Itens { get; private set; } = new List<ItemBasicViewModel>();
	}
}
