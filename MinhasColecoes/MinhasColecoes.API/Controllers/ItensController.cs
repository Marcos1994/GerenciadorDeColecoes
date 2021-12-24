using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItensController : ControllerBase
	{
		private readonly IItemService service;

		public ItensController(IItemService service)
		{
			this.service = service;
		}

		[Authorize]
		[HttpPost("{idColecao}")]
		public IActionResult PostComment(int idColecao, ItemInputModel item)
		{
			return NoContent();
		}
	}
}
