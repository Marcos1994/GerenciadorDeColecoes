using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	[Route("Colecoes/{idColecao}/Itens")]
	[ApiController]
	public class ItensController : ControllerBase
	{
		private readonly IItemService service;

		public ItensController(IItemService service)
		{
			this.service = service;
		}

		[HttpGet("{idItem}")]
		public IActionResult GetById(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				return Ok(service.GetById(idUsuario, idItem));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public IActionResult Post(int idColecao, ItemInputModel item)
		{
			item.IdUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			item.IdColecao = idColecao;
			try
			{
				ItemViewModel itemView = service.Create(item);
				return CreatedAtAction(nameof(GetById), new { idItem = itemView.Id }, itemView);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
