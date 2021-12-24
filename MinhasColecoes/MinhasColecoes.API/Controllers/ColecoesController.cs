using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
			List<ColecaoBasicViewModel> colecoes = service.GetAll(idUsuario, nome).ToList();
			return Ok(colecoes);
		}

		[Authorize]
		[HttpGet]
		[Route("MinhasColecoes")]
		public IActionResult GetParticipo(string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = service.GetAllParticipa(idUsuario, nome).ToList();
			return Ok(colecoes);
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
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
