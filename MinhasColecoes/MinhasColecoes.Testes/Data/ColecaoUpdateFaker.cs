using Bogus;
using MinhasColecoes.Aplicacao.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	class ColecaoUpdateFaker : Faker<ColecaoUpdateModel>
	{
		public ColecaoUpdateFaker()
			: this(new Faker().Random.Number(1, 100000)) { }

		public ColecaoUpdateFaker(int id)
		{
			RuleFor(p => p.Id, f => id);
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Foto, f => f.Lorem.Sentence(3));
			RuleFor(p => p.Publica, f => f.Random.Bool(0.7F));
		}
	}
}
