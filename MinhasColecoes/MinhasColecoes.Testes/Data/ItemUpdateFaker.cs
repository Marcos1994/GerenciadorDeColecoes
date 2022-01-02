using Bogus;
using MinhasColecoes.Aplicacao.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class ItemUpdateFaker : Faker<ItemUpdateModel>
	{
		public ItemUpdateFaker()
		{
			RuleFor(p => p.Id, f => f.Random.Number(1, 100000));
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(10));
			RuleFor(p => p.Codigo, f => (f.Random.Bool(0.3F)
				? "" : f.Random.Number(1, 100000).ToString()));
		}
	}
}
