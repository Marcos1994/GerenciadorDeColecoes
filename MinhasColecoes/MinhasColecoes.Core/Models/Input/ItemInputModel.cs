using MinhasColecoes.Aplicacao.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Input
{
	public class ItemInputModel
	{
		public int IdColecao { get; set; }
		public int? IdOriginal { get; set; }
		public int IdDono { get; set; }
		public string Nome { get; set; }
		public string Codigo { get; set; }
		public string Descricao { get; set; }
		public EnumRelacaoUsuarioItem Relacao { get; set; }
	}
}
