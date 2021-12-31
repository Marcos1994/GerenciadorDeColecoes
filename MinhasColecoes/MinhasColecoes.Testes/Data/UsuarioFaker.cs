using Bogus;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class UsuarioFaker : Faker<Usuario>
	{
		public UsuarioFaker(bool withChilds = false)
		{
			int id = new Faker().Random.Number(1, 100000);
			RuleFor(p => p.Id, f => id);
			RuleFor(p => p.Nome, f => f.Person.FullName);
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Login, f => f.Person.Email);
			RuleFor(p => p.Senha, f => f.Random.String(6, 15));

			if (withChilds)
			{
				//Adicionando as coleções das quais ele é dono
				List<Colecao> colecoes = new ColecaoFaker(id).Generate(2);
				RuleFor(p => p.ColecoesDono, f => colecoes);

				//Adicionando as coleções que ele participa (incluindo as q ele é dono)
				List<ColecaoUsuario> relacoesColecaoUsuario = new List<ColecaoUsuario>();
				for (int i = 0; i < colecoes.Count(); i++)
					relacoesColecaoUsuario.Add(new ColecaoUsuario(id, colecoes[i].Id));
				for (int i = 0; i < 4; i++)
					relacoesColecaoUsuario.Add(new ColecaoUsuario(id, new Faker().Random.Number(1, 100000)));
				RuleFor(p => p.ColecoesParticipa, f => relacoesColecaoUsuario);
			}
		}
	}
}
