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
		public ColecaoFaker(int idColecaoMaior)
			: this()
		{
			RuleFor(p => p.IdColecaoMaior, f => idColecaoMaior);
		}

		public ColecaoFaker(bool withChilds = false)
			: this(new Faker().Random.Number(1, 100000), withChilds) { }


		public ColecaoFaker(int idDono, bool withChilds = false)
		{
			int id = new Faker().Random.Number(1, 100000);
			RuleFor(p => p.Id, f => id);
			RuleFor(p => p.IdColecaoMaior, f => null);
			RuleFor(p => p.IdDono, f => idDono);
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Publica, f => f.Random.Bool(0.7F));

			if (withChilds)
			{
				List<ColecaoUsuario> relacoesColecaoUsuario = new List<ColecaoUsuario>();
				relacoesColecaoUsuario.Add(new ColecaoUsuario(idDono, id));
				for (int i = 0; i < new Faker().Random.Number(1, 4); i++)
					relacoesColecaoUsuario.Add(new ColecaoUsuario(new Faker().Random.Number(1, 100000), id));
				RuleFor(p => p.UsuariosColecao, f => relacoesColecaoUsuario);

				RuleFor(p => p.Colecoes, f => new ColecaoFaker(id).Generate(f.Random.Number(2, 5)));

				RuleFor(p => p.Itens, f => new ItemFaker(id, f.Random.Bool(0.7F)).Generate(f.Random.Number(3, 8)));
			}
		}
	}
}
