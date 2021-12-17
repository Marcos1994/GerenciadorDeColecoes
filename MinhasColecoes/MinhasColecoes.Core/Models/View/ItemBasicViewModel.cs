using MinhasColecoes.Aplicacao.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class ItemBasicViewModel
	{
		public int Id { get; private set; }
		public string Nome { get; private set; }
		public string Codigo { get; private set; }
		public string Descricao { get; private set; }
		public bool Original { get; private set; }
		public int? IdOriginal { get; private set; }
		public ItemBasicViewModel ItemOriginal { get; private set; }
		public EnumRelacaoUsuarioItem Relacao { get; private set; }

		public void SetItemOriginal(ItemBasicViewModel itemOriginal)
		{
			ItemOriginal = itemOriginal;
		}
	}
}
