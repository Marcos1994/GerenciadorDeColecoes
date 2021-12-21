using Microsoft.EntityFrameworkCore;
using MinhasColecoes.Persistencia.Context;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Repositories
{
	public class ColecaoRepository : Repository, IColecaoRepository
	{
		private readonly MinhasColecoesDbContext dbContext;

		public ColecaoRepository(MinhasColecoesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		protected override DbContext GetContext()
		{
			return dbContext;
		}

		public IEnumerable<Colecao> GetAll(int idUsuario, string nome = "")
		{
			return dbContext.Colecoes.Where(c => (c.Publica == true || c.IdDono == idUsuario) && c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
		}

		public IEnumerable<Colecao> GetAllSubcolecoes(int idUsuario, int idColecao)
		{
			return dbContext.Colecoes.Where(c => c.IdColecaoMaior == idColecao && (c.Publica == true || c.IdDono == idUsuario));
		}

		public IEnumerable<Colecao> GetAllPessoais(int idDono)
		{
			return dbContext.Colecoes.Where(c => c.IdDono == idDono);
		}

		public IEnumerable<Colecao> GetAllMembro(int idMembro)
		{
			return from cu in dbContext.ColecoesUsuario
				   where cu.IdUsuario == idMembro
				   join c in dbContext.Colecoes
				   on cu.IdColecao equals c.Id
				   select c;
		}

		public IEnumerable<Usuario> GetMembros(int idColecao)
		{
			return from cu in dbContext.ColecoesUsuario
				   where cu.IdColecao == idColecao
				   join u in dbContext.Usuarios
				   on cu.IdColecao equals u.Id
				   select u;
		}

		public Colecao GetById(int id)
		{
			return dbContext.Colecoes.First(c => c.Id == id);
		}

		public void Add(Colecao colecao)
		{
			dbContext.Colecoes.Add(colecao);
			dbContext.SaveChanges();
		}
		public void Add(ColecaoUsuario colecaoUsuario)
		{
			dbContext.ColecoesUsuario.Add(colecaoUsuario);
			dbContext.SaveChanges();
		}

		public void Update(Colecao colecao)
		{
			dbContext.Colecoes.Update(colecao);
			dbContext.SaveChanges();
		}

		public void Delete(Colecao colecao)
		{
			dbContext.Colecoes.Remove(colecao);
			dbContext.SaveChanges();
		}

		public void Delete(ColecaoUsuario colecaoUsuario)
		{
			dbContext.ColecoesUsuario.Remove(colecaoUsuario);
			dbContext.SaveChanges();
		}
	}
}
