using Bogus;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class ItemFaker : Faker<Item>
	{
		private readonly int id;

		public ItemFaker(int idColecao, int idUsuarioRelacao, bool? original = true)
			: this(idColecao, original)
		{
			RuleFor(p => p.RelacoesUsuarios, f => new List<ItemUsuario>
				{ new ItemUsuario(idUsuarioRelacao, id, f.Random.Number(1, 5), f.Lorem.Sentence(5)) });
		}

		public ItemFaker(int idColecao, int idUsuarioRelacao, EnumRelacaoUsuarioItem relacao, bool? original = true)
			: this(idColecao, original)
		{
			RuleFor(p => p.RelacoesUsuarios, f => new List<ItemUsuario>
				{ new ItemUsuario(idUsuarioRelacao, id, (int) relacao, f.Lorem.Sentence(5)) });
		}

		public ItemFaker(int idColecao, bool? original = true)
		{
			id = new Faker().Random.Number(1, 100000);
			RuleFor(p => p.Id, f => id);
			RuleFor(p => p.IdColecao, f => idColecao);
			RuleFor(p => p.Original, f => original == true);
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(10));
			RuleFor(p => p.Codigo, f => (f.Random.Bool(0.3F)
				? "" : f.Random.Number(1, 100000).ToString()));

			if(original != true)
			{
				RuleFor(p => p.IdDonoParticular, f => f.Random.Number(1, 100000));
				if(original == false)
				{
					Item itemOriginal = new ItemFaker(idColecao).Generate();
					RuleFor(p => p.ItemOriginal, f => itemOriginal);
					RuleFor(p => p.IdOriginal, f => itemOriginal.Id);
				}
			}
		}
	}
}
