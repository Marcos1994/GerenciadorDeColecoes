using Bogus;
using MinhasColecoes.Aplicacao.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class ColecaoInputFaker : Faker<ColecaoInputModel>
	{
		public ColecaoInputFaker(bool subColecao = false)
		{
			RuleFor(p => p.IdDono, f => f.Random.Number(1, 100000));
			RuleFor(p => p.IdColecaoMaior,
				f => subColecao
				? f.Random.Number(1, 100000) : null);
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Publica, f => f.Random.Bool(0.7F));
		}
	}
}
