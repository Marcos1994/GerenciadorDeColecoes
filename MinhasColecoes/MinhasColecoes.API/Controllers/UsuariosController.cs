using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsuariosController : ControllerBase
	{
		private readonly IUsuarioService serviceUsuario;
		private readonly IJWTService jwt;

		public UsuariosController(IUsuarioService serviceUsuario, IJWTService jwt)
		{
			this.serviceUsuario = serviceUsuario;
			this.jwt = jwt;
		}

		[HttpPost]
		public IActionResult Post(UsuarioLoginInputModel usuario)
		{
			UsuarioLoginViewModel usuarioLogado = serviceUsuario.ValidarUsuario(usuario);
			if (usuarioLogado == null)
				return BadRequest();
			usuarioLogado.SetToken(jwt.GerarToken(usuarioLogado));
			return Ok(usuarioLogado);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Get()
		{
			string login = User.Identity.Name;
			return Ok(login);
		}
	}
}
