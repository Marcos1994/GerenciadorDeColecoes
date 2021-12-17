using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Entities
{
	public class Item
	{
		public Item()
		{

		}
		public Item(int idColecao, bool original, int? idOriginal, Item itemOriginal, int? idDonoParticular, string nome, string codigo, string descricao)
		{
			IdColecao = idColecao;
			Original = original;
			IdOriginal = idOriginal;
			ItemOriginal = itemOriginal;
			IdDonoParticular = idDonoParticular;
			Nome = nome;
			Codigo = codigo;
			Descricao = descricao;
		}

		public int Id { get; private set; }
		public int IdColecao { get; private set; }
		public bool Original { get; private set; }
		public int? IdOriginal { get; private set; }
		public Item ItemOriginal { get; private set; }
		public int? IdDonoParticular { get; private set; }
		public string Nome { get; private set; }
		public string Codigo { get; private set; }
		public string Descricao { get; private set; }
		public List<ItemUsuario> RelacoesUsuarios { get; private set; } = new List<ItemUsuario>();

		public void Update(string nome, string codigo, string descricao)
		{
			Nome = nome;
			Codigo = codigo;
			Descricao = descricao;
		}

		public void SetItemOriginal(Item itemOriginal)
		{
			ItemOriginal = itemOriginal;
			SetItemOriginal((itemOriginal == null) ? null : itemOriginal.Id);
		}

		public void SetItemOriginal(int? idItemOriginal)
		{
			IdOriginal = idItemOriginal;
		}

		public void SetOriginal(bool original)
		{
			Original = original;
		}

		public void SetDonoParticular(int? idDonoParticular)
		{
			IdDonoParticular = idDonoParticular;
		}
	}
}
