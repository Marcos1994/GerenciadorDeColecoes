using MinhasColecoes.Aplicacao.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Input
{
	public class RelacaoItemUsuarioInputModel
	{
		public int IdUsuario { get; set; }
		public int IdItem { get; set; }
		public EnumRelacaoUsuarioItem Relacao { get; set; }
		public string Comentario { get; set; }
	}
}
