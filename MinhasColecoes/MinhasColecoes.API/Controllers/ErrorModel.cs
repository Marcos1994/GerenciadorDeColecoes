using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasColecoes.API.Controllers
{
	public class ErrorModel
	{
		public string Title { get; set; }
		public Dictionary<string, string[]> Errors { get; set; }

		public ErrorModel(string title, Dictionary<string, string[]> errors)
		{
			Title = title;
			Errors = errors;
		}

		public ErrorModel(string title)
		{
			Title = title;
			Errors = new Dictionary<string, string[]>();
		}
	}
}
