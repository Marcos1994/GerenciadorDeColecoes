using Bogus;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Data
{
	public class UsuarioInputFaker : Faker<UsuarioInputModel>
	{
		public UsuarioInputFaker()
		{
			RuleFor(p => p.Nome, f => f.Person.FullName);
			RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15));
			RuleFor(p => p.Login, f => f.Person.Email);
			RuleFor(p => p.Senha, f => f.Random.String(6, 15));
		}
	}
}
