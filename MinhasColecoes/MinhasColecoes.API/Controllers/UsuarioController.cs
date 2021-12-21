using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsuarioController : ControllerBase
	{
		private readonly IUsuarioService serviceUsuario;
		private readonly IJWTService jwt;

		public UsuarioController(IUsuarioService serviceUsuario, IJWTService jwt)
		{
			this.serviceUsuario = serviceUsuario;
			this.jwt = jwt;
		}

		[HttpPost]
		[Route("Cadastro")]
		public IActionResult Post(UsuarioInputModel usuario)
		{
			serviceUsuario.Create(usuario);
			return Ok();
		}

		[HttpPost]
		[Route("Login")]
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
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			UsuarioViewModel usuario = serviceUsuario.GetById(idUsuario);
			return Ok(usuario);
		}
	}
}
