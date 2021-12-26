﻿using Microsoft.AspNetCore.Authorization;
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
				return BadRequest(ex);
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

		[Authorize]
		[HttpGet]
		public IActionResult Get()
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			UsuarioViewModel usuario;
			try
			{
				usuario = service.GetById(idUsuario);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex); }
			catch (Exception ex) { return BadRequest(ex.Message); }
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
				service.Update(usuario);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex); }
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
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex); }
			catch (Exception ex) { return BadRequest(ex.Message); }
			return NoContent();
		}

		[Authorize]
		[HttpGet]
		[Route("MinhasColecoes")]
		public IActionResult GetParticipo(string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = serviceColecao.GetAllParticipa(idUsuario, nome).ToList();
			return Ok(colecoes);
		}
	}
}
