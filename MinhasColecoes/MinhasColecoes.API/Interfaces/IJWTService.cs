using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Interfaces
{
	public interface IJWTService
	{
		string GerarToken(UsuarioLoginViewModel usuario);
		bool VerificarValidadeToken(string token);
	}
}
