using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Input
{
	public class ColecaoInputModel
	{
		public int IdDono { get; set; }
		public int? IdColecaoMaior { get; set; }
		public string Nome { get; set; }
		public string Descricao { get; set; }
		public string Foto { get; set; }
		public bool Publica { get; set; }
	}
}
