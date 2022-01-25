using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IWebHostEnvironment webHostEnv;

		public FileUploadController(IWebHostEnvironment webHostEnv)
		{
			this.webHostEnv = webHostEnv;
		}

		[Authorize]
		[HttpPost]
		public IActionResult Post([FromForm(Name = "image")]IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest();

			string diretorio = "\\Upload\\";

			if (!Directory.Exists(webHostEnv.WebRootPath + diretorio))
				Directory.CreateDirectory(webHostEnv.WebRootPath + diretorio);

			try
			{
				using (FileStream fileStream = System.IO.File.Create(webHostEnv.WebRootPath + diretorio + file.FileName))
				{
					file.CopyTo(fileStream);
					fileStream.Flush();
				}
				return Ok(diretorio + file.FileName);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
