using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Entities
{
	public class Usuario
	{
		public Usuario()
		{

		}
		public Usuario(string login, string nome, string foto, string senha, string descricao)
		{
			Login = login;
			Nome = nome;
			Foto = foto;
			Senha = senha;
			Descricao = descricao;
		}

		public int Id { get; private set; }
		public string Login { get; private set; }
		public string Nome { get; private set; }
		public string Senha { get; private set; }
		public string Descricao { get; private set; }
		public string Foto { get; private set; }
		public List<Colecao> ColecoesDono { get; private set; } = new List<Colecao>();
		public List<ColecaoUsuario> ColecoesParticipa { get; private set; } = new List<ColecaoUsuario>();
		public List<ItemUsuario> RelacoesItens { get; private set; } = new List<ItemUsuario>();
		public List<Item> ItensDono { get; private set; } = new List<Item>();

		public void Update(string nome, string foto, string descricao)
		{
			Nome = nome;
			Foto = foto;
			Descricao = descricao;
		}

		public void UpdateSenha(string senha)
		{
			Senha = senha;
		}
	}
}
