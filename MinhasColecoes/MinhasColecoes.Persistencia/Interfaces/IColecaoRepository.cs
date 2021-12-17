using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IColecaoRepository : IRepository
	{
		IEnumerable<Colecao> GetAll(int idUsuario, string nome = "");
		IEnumerable<Colecao> GetAllSubcolecoes(int idUsuario, int idColecao);
		IEnumerable<Colecao> GetAllPessoais(int idDono);
		IEnumerable<Colecao> GetAllMembro(int idMembro);
		IEnumerable<Usuario> GetMembros(int idColecao);
		Colecao GetById(int id);
		void Add(Colecao colecao);
		void Add(ColecaoUsuario colecaoUsuario);
		void Update(Colecao colecao);
		void Delete(Colecao colecao);
		void Delete(ColecaoUsuario colecaoUsuario);
	}
}
