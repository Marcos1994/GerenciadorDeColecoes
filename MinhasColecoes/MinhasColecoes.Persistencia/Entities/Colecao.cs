using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Entities
{
	public class Colecao
	{
		public Colecao()
		{

		}
		public Colecao(int? idColecaoMaior, int idDono, string nome, string descricao, string foto, bool publica)
		{
			IdColecaoMaior = idColecaoMaior;
			IdDono = idDono;
			Nome = nome;
			Descricao = descricao;
			Foto = foto;
			Publica = publica;
		}

		public int Id { get; private set; }
		public int? IdColecaoMaior { get; private set; }
		public int IdDono { get; private set; }
		public string Nome { get; private set; }
		public string Descricao { get; private set; }
		public string Foto { get; private set; }
		public bool Publica { get; private set; }
		public List<Colecao> Colecoes { get; private set; } = new List<Colecao>();
		public List<ColecaoUsuario> UsuariosColecao { get; private set; } = new List<ColecaoUsuario>();
		public List<Item> Itens { get; private set; } = new List<Item>();

		public void Update(string nome, string descricao, string foto, bool publica)
		{
			Nome = nome;
			Descricao = descricao;
			Foto = foto;
			Publica = publica;
		}

		public void SetColecaoMaior(int? idColecaoMaior)
		{
			IdColecaoMaior = idColecaoMaior;
		}
	}
}
