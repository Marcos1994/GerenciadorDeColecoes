using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ColecaoController : ControllerBase
	{
		private readonly IColecaoService serviceColecao;

		public ColecaoController(IColecaoService serviceColecao)
		{
			this.serviceColecao = serviceColecao;
		}

		[HttpGet]
		public IActionResult Get(string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ColecaoBasicViewModel> colecoes = serviceColecao.GetAll(idUsuario, nome).ToList();
			return Ok(colecoes);
		}

		[HttpGet("{idColecao}")]
		public IActionResult Get(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				ColecaoViewModel colecao = serviceColecao.GetById(idUsuario, idColecao);
				return Ok(colecao);
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}
	}
}
