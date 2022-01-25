using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Services
{
	public class SaveFileService
	{
		private readonly IWebHostEnvironment webHostEnv;

		public SaveFileService(IWebHostEnvironment webHostEnv)
		{
			this.webHostEnv = webHostEnv;
		}

		public async Task<string> SaveFile(IFormFile file, string diretorioRelativo, int tamanhoMaximo)
		{
			if (!Directory.Exists(webHostEnv.WebRootPath + diretorioRelativo))
				Directory.CreateDirectory(webHostEnv.WebRootPath + diretorioRelativo);

			//Copia a imagem para a memoria
			MemoryStream ms = new MemoryStream();
			await file.CopyToAsync(ms);

			//Carrega o fluxo em memória para o objeto de processamento de imagem
			ms.Position = 0;
			Image img = await Image.LoadAsync(ms);

			//Converte a imagem para jpeg e recarrega ela
			JpegEncoder jpegEnc = new JpegEncoder();
			jpegEnc.Quality = 100;
			img.SaveAsJpeg(ms, jpegEnc);
			ms.Position = 0;
			img = await Image.LoadAsync(ms);

			//Encerro o fluxo de memória
			ms.Close();
			ms.Dispose();

			//Redimensiono a imagem
			if (img.Size().Width > tamanhoMaximo || img.Size().Height > tamanhoMaximo)
			{
				ResizeOptions resize = new ResizeOptions()
				{
					Mode = ResizeMode.Max,
					Size = new Size(tamanhoMaximo, tamanhoMaximo)
				};

				img.Mutate(i => i.Resize(resize));
			}

			//Defino e seto o nome do arquivo sem a extensão e sufixos
			string nomeArquivo = $"{DateTime.Now.ToString("yyyyMMddHHmmssFFF")}.jpg";

			//Salvo a imagem
			var diretorio = Path.Combine(webHostEnv.WebRootPath + diretorioRelativo, nomeArquivo);
			await img.SaveAsync(diretorio);

			return diretorioRelativo + nomeArquivo;
		}
	}
}
