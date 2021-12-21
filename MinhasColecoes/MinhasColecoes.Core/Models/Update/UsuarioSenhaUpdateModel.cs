using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Update
{
	public class UsuarioSenhaUpdateModel
	{
		public int Id { get; private set; }
		public string Login { get; private set; }
		public string Senha { get; private set; }
		public string NovaSenha { get; private set; }
	}
}
