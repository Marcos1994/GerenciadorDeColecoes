using Bogus;
using MinhasColecoes.Aplicacao.Enumerators;
using MinhasColecoes.Aplicacao.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class ItemInputFaker : Faker<ItemInputModel>
	{
		public ItemInputFaker()
			: this(new Faker().PickRandom<EnumRelacaoUsuarioItem>())
		{ }

		public ItemInputFaker(EnumRelacaoUsuarioItem relacao)
		{
			RuleFor(p => p.IdColecao, f => f.Random.Number(1, 100000));
			RuleFor(p => p.Nome, f => f.Name.FirstName());
			RuleFor(p => p.IdUsuario, f => f.Random.Number(1, 100000));
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(10));
			RuleFor(p => p.Codigo, f => (f.Random.Bool(0.3F)
				? "" : f.Random.Number(1, 100000).ToString()));
			RuleFor(p => p.Relacao, f => relacao);
			if(relacao != EnumRelacaoUsuarioItem.NaoPossuo)
				RuleFor(p => p.Comentario, f => f.Lorem.Sentence(5));
		}
	}
}
