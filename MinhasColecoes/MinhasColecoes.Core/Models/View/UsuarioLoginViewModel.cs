using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.View
{
	public class UsuarioLoginViewModel : UsuarioBasicViewModel
	{
		public string Login { get; private set; }
		public string Token { get; private set; }

		public void SetToken(string token)
		{
			Token = token;
		}
	}
}
