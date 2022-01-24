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
		public IActionResult Post(IFormFile file)
		{
			if (file.Length == 0)
				return NoContent();
			string diretorio = webHostEnv.WebRootPath;
			if (!Directory.Exists(webHostEnv.WebRootPath + "\\Upload\\"))
				Directory.CreateDirectory(webHostEnv.WebRootPath + "\\Upload\\");

			try
			{
				using (FileStream fileStream = System.IO.File.Create(webHostEnv.WebRootPath + "\\Upload\\" + file.FileName))
				{
					file.CopyTo(fileStream);
					fileStream.Flush();
				}
				return Ok(diretorio+"\\Upload\\"+ file.FileName);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
