using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Update
{
	public class UsuarioUpdateModel
	{
		public int Id { get; private set; }
		public string Descricao { get; private set; }
		public string Foto { get; private set; }
	}
}
