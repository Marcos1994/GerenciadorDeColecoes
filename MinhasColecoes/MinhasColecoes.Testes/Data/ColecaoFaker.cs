using Bogus;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class ColecaoFaker : Faker<Colecao>
	{
		public ColecaoFaker(bool withChilds = false)
			: this(new Faker().Random.Number(1, 100000), withChilds) { }

		public ColecaoFaker(int idDono, bool withChilds = false)
		{
			int id = new Faker().Random.Number(1, 100000);
			RuleFor(p => p.Id, f => id);
			RuleFor(p => p.IdColecaoMaior,
				f => (f.Random.Number(1, 10) == 1)
				? f.Random.Number(1, 100000) : null);
			RuleFor(p => p.IdDono, f => idDono);
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Publica, f => f.Random.Number(1, 10) > 3);

			if (withChilds)
			{

			}
		}
	}
}
