using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinhasColecoes.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class FileUploadController : ControllerBase
	{
		private readonly SaveFileService service;

		public FileUploadController(SaveFileService service)
		{
			this.service = service;
		}

		[Authorize]
		[HttpPost]
		[Route("Perfil")]
		public async Task<IActionResult> PostPerfil([FromForm(Name = "image")]IFormFile file)
		{
			if (file == null || file.Length == 0)
				return NotFound();

			try
			{
				return Ok(await service.SaveFile(file, "\\Imagens\\Perfis\\", 600));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		[Route("Colecao")]
		public async Task<IActionResult> PostColecao([FromForm(Name = "image")] IFormFile file)
		{
			if (file == null || file.Length == 0)
				return NotFound();

			try
			{
				return Ok(await service.SaveFile(file, "\\Imagens\\Colecoes\\", 450));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		[Route("Item")]
		public async Task<IActionResult> PostItem([FromForm(Name = "image")] IFormFile file)
		{
			if (file == null || file.Length == 0)
				return NotFound();

			try
			{
				return Ok(await service.SaveFile(file, "\\Imagens\\Itens\\", 900));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
