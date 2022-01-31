using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[Route("[controller]")]
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
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (FalhaDeValidacaoException ex) { return BadRequest(new ErrorModel(ex.Message, ex.Errors)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
		}

		[HttpGet("{idItem}")]
		public IActionResult GetById(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				return Ok(service.GetById(idUsuario, idItem));
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
		}

		[HttpGet("{idColecao}/PorNome")]
		public IActionResult GetById(int idColecao, string nome = "")
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				return Ok(service.GetAll(idUsuario, idColecao, nome));
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
		}

		[Authorize]
		[HttpPut("{idItem}")]
		public IActionResult Put(int idItem, ItemUpdateModel item)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			item.Id = idItem;
			try
			{
				service.Update(idUsuario, item);
			}
			catch (FalhaDeValidacaoException ex) { return BadRequest(new ErrorModel(ex.Message, ex.Errors)); }
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return NoContent();
		}

		[Authorize]
		[HttpPut("{idItem}/Relacao")]
		public IActionResult Put(int idItem, EnumRelacaoUsuarioItem relacao)
		{
			try
			{
				service.DefinirRelacao(new RelacaoItemUsuarioInputModel
				{
					IdUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
					IdItem = idItem,
					Relacao = relacao
				});
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new ErrorModel(ex.Message));
			}
		}

		[Authorize]
		[HttpGet("{idItem}/Particulares")]
		public IActionResult GetParticularesItem(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ItemBasicViewModel> itens;
			try
			{
				itens = service.GetAllParticularesItem(idUsuario, idItem).ToList();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return Ok(itens);
		}

		[Authorize]
		[HttpPut("{idItem}/Particulares")]
		public IActionResult PutJuntar(int idItem, int idItemParticular)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.Oficializar(idUsuario, idItemParticular);
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return NoContent();
		}

		[Authorize]
		[HttpGet("Particulares")]
		public IActionResult GetParticularesColecao(int idColecao)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			List<ItemBasicViewModel> itens;
			try
			{
				itens = service.GetAllParticularesColecao(idUsuario, idColecao).ToList();
			}
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return Ok(itens);
		}

		[Authorize]
		[HttpPut("Particulares/{idItem}")]
		public IActionResult PutOficializar(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.Oficializar(idUsuario, idItem);
			}
			catch (FalhaDeValidacaoException ex) { return BadRequest(new ErrorModel(ex.Message, ex.Errors)); }
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return NoContent();
		}
	
		[Authorize]
		[HttpDelete("{id}")]
		public IActionResult Delete(int idItem)
		{
			int idUsuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				service.Delete(idUsuario, idItem);
			}
			catch (FalhaDeValidacaoException ex) { return BadRequest(new ErrorModel(ex.Message, ex.Errors)); }
			catch (ObjetoNaoEncontradoException ex) { return NotFound(new ErrorModel(ex.Message)); }
			catch (UsuarioNaoAutorizadoException ex) { return Unauthorized(new ErrorModel(ex.Message)); }
			catch (Exception ex) { return BadRequest(new ErrorModel(ex.Message)); }
			return NoContent();
		}
	}
}
