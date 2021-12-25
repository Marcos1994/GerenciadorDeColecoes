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
	[Route("Colecoes/{idColecao}/[controller]")]
	[ApiController]
	public class ItensController : ControllerBase
	{
		private readonly IItemService service;

		public ItensController(IItemService service)
		{
			this.service = service;
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
				return CreatedAtAction(nameof(GetById), new { idColecao = idColecao, idItem = itemView.Id }, itemView);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
		[HttpPut("{idItem}")]
		public IActionResult Put(int idItem, ItemUpdateModel item)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			item.Id = idItem;
			return Ok("Funcionalidade não implementada.\nAtualiza um item de acordo com o Id.");
		}

		[Authorize]
		[HttpGet("{idItem}/Particulares")]
		public IActionResult GetParticularesItem(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			return Ok("Funcionalidade não implementada.\nLista todos os itens particulares deste item.\nLiberado apenas para o dono da coleção.");
		}

		[Authorize]
		[HttpPut("{idItem}/Particulares")]
		public IActionResult PutJuntar(int idItem, int idItemParticular)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			return Ok("Funcionalidade não implementada.\nAtualiza o item idItem com as informações do item idItemParticular e exclui ele.");
		}

		[Authorize]
		[HttpGet("Particulares")]
		public IActionResult GetParticularesColecao(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			return Ok("Funcionalidade não implementada.\nLista todos os itens particulares de uma coleção.");
		}

		[Authorize]
		[HttpPut("Particulares/{idItem}")]
		public IActionResult PutOficializar(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			return Ok("Funcionalidade não implementada.\nTorna um item particular como oficial.");
		}
	}
}
