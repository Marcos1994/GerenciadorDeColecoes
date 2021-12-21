using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Input
{
	public class UsuarioInputModel
	{
		public int Id { get; set; }
		public string Nome { get; set; }
		public string Login { get; set; }
		public string Senha { get; set; }
		public string Descricao { get; set; }
		public string Foto { get; set; }
	}
}
