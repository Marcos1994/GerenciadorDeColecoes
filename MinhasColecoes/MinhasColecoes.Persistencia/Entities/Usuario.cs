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
		public Usuario(string nome, string foto)
		{
			Nome = nome;
			Foto = foto;
		}

		public int Id { get; private set; }
		public string Nome { get; private set; }
		public string Foto { get; private set; }
		public List<Colecao> ColecoesDono { get; private set; } = new List<Colecao>();
		public List<ColecaoUsuario> ColecoesParticipa { get; private set; } = new List<ColecaoUsuario>();
		public List<ItemUsuario> RelacoesItens { get; private set; } = new List<ItemUsuario>();
		public List<Item> ItensDono { get; private set; } = new List<Item>();
	}
}
