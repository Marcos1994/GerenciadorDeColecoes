﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	[Route("[controller]")]
	[ApiController]
	public class ColecoesController : ControllerBase
	{
		private readonly IColecaoService service;

		public ColecoesController(IColecaoService serviceColecao)
		{
			this.service = serviceColecao;
		}

		[HttpGet]
		public IActionResult GetAll(string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = service.GetAllSubcolecoes(idUsuario, null, nome).ToList();
			return Ok(colecoes);
		}

		[HttpGet("{idColecao}/Subcolecoes")]
		public IActionResult GetAllSubcolecoes(int idColecao, string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = service.GetAllSubcolecoes(idUsuario, idColecao, nome).ToList();
			return Ok(colecoes);
		}

		[HttpGet("{idColecao}/Membros")]
		public IActionResult GetAllMembros(int idColecao)
		{
			List<UsuarioBasicViewModel> colecoes = service.GetAllMembros(idColecao).ToList();
			return Ok(colecoes);
		}

		[HttpGet]
		[Route("{idColecao}/Genealogia")]
		public IActionResult GetGenealogia(int idColecao)
		{
			try
			{
				ColecaoGenealogiaViewModel colecao = service.GetAllSupercolecoes(idColecao);
				return Ok(colecao);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[HttpGet("{idColecao}")]
		public IActionResult GetById(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				ColecaoViewModel colecao = service.GetById(idUsuario, idColecao);
				return Ok(colecao);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[Authorize]
		[HttpPost]
		public IActionResult Post(ColecaoInputModel colecao)
		{
			colecao.IdDono = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			if (colecao.IdColecaoMaior == 0)
				colecao.IdColecaoMaior = null;
			try
			{
				ColecaoViewModel colecaoView = service.Create(colecao);
				return CreatedAtAction(nameof(GetById), new { idColecao = colecaoView.Id }, colecaoView);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPut("{idColecao}")]
		public IActionResult Put(int idColecao, ColecaoUpdateModel colecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			colecao.Id = idColecao;
			try
			{
				service.Update(idUsuario, colecao);
				return NoContent();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[Authorize]
		[HttpPut("{idColecao}/Participar")]
		public IActionResult Particupar(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.AdicionarMembro(idUsuario, idColecao);
				return NoContent();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[Authorize]
		[HttpPut("{idColecao}/Supercolecao")]
		public IActionResult PutSupercolecao(int idColecao, int idSupercolecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.AdicionarSupercolecao(idUsuario, idColecao, idSupercolecao);
				return NoContent();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[Authorize]
		[HttpDelete("{idColecao}")]
		public IActionResult Delete(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.Delete(idUsuario, idColecao);
				return NoContent();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}

		[Authorize]
		[HttpDelete("{idColecao}/Supercolecao")]
		public IActionResult DeleteSupercolecao(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.AdicionarSupercolecao(idUsuario, idColecao, null);
				return NoContent();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(ex.Message); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(ex.Message); }
			catch (Exception ex) { return BadRequest(ex.Message); }
		}
	}
}
