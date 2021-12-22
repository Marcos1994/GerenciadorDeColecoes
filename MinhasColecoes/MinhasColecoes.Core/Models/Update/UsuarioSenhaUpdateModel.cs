using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Update
{
	public class UsuarioSenhaUpdateModel
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Senha { get; set; }
		public string NovaSenha { get; set; }
	}
}
