using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
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
		private readonly IColecaoService serviceColecao;
		private readonly IJWTService jwt;

		public UsuarioController(IUsuarioService serviceUsuario, IColecaoService serviceColecao, IJWTService jwt)
		{
			this.serviceUsuario = serviceUsuario;
			this.serviceColecao = serviceColecao;
			this.jwt = jwt;
		}

		[HttpPost]
		[Route("Cadastro")]
		public IActionResult Post(UsuarioInputModel usuario)
		{
			try
			{
				serviceUsuario.Create(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
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
			UsuarioViewModel usuario;
			try
			{
				usuario = serviceUsuario.GetById(idUsuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
			usuario.ColecoesMembro.AddRange(serviceColecao.GetAllParticipa(idUsuario));
			usuario.ColecoesDono.AddRange(serviceColecao.GetAllProprias(idUsuario));
			return Ok(usuario);
		}

		[Authorize]
		[HttpPut]
		[Route("Atualizar")]
		public IActionResult Put(UsuarioUpdateModel usuario)
		{
			usuario.Id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				serviceUsuario.Update(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
			return Ok();
		}

		[Authorize]
		[HttpPut]
		[Route("AlterarSenha")]
		public IActionResult Put(UsuarioSenhaUpdateModel usuario)
		{
			usuario.Id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				serviceUsuario.Update(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}
	}
}
