using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.API.Interfaces;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Exceptions;
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
		private readonly IUsuarioService service;
		private readonly IColecaoService serviceColecao;
		private readonly IJWTService jwt;

		public UsuarioController(IUsuarioService service, IColecaoService serviceColecao, IJWTService jwt)
		{
			this.service = service;
			this.serviceColecao = serviceColecao;
			this.jwt = jwt;
		}

		[HttpPost]
		[Route("Novo")]
		public IActionResult Post(UsuarioInputModel usuario)
		{
			try
			{
				service.Create(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}

		[HttpPost]
		[Route("Login")]
		public IActionResult Post(UsuarioLoginInputModel usuario)
		{
			UsuarioLoginViewModel usuarioLogado = service.ValidarUsuario(usuario);
			if (usuarioLogado == null)
				return BadRequest();
			usuarioLogado.SetToken(jwt.GerarToken(usuarioLogado));
			return Ok(usuarioLogado);
		}

		[HttpGet]
		[Route("VerificarToken")]
		public IActionResult Post(string token)
		{
			return (jwt.VerificarValidadeToken(token))
				? Ok()
				: Unauthorized();
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			int idAutenticado = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			UsuarioViewModel usuario;
			try
			{
				usuario = service.GetById(id);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
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
				service.Update(usuario);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
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
				service.Update(usuario);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
			return NoContent();
		}

		[HttpGet]
		[Route("{id}/MinhasColecoes")]
		public IActionResult GetParticipo(int id, string nome = "")
		{
			int idAutenticado = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = serviceColecao.GetAllParticipa(idAutenticado, id, nome).ToList();
			return Ok(colecoes);
		}
	}
}
