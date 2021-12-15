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
	public class ItemRepository : Repository, IItemRepository
	{
		private readonly MinhasColecoesDbContext dbContext;

		public ItemRepository(MinhasColecoesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		protected override DbContext GetContext()
		{
			return dbContext;
		}

		public IEnumerable<Item> GetAll(int idColecao)
		{
			return dbContext.Itens.Where(i => i.IdColecao == idColecao && i.Original == true);
		}

		public IEnumerable<Item> GetAllPessoais(int idColecao, int idUsuario)
		{
			List<Item> itens = dbContext.Itens.Where(i => i.IdColecao == idColecao && (i.Original == true || i.IdDonoParticular == idUsuario)).ToList();

			List<ItemUsuario> relacoes = (from iu in dbContext.ItensUsuario
										  where iu.IdUsuario == idUsuario
										  join i in itens
										  on iu.IdItem equals i.Id
										  select iu).ToList();

			for (int i = 0; i < relacoes.Count(); i++)
				itens.First(item => item.Id == relacoes[i].IdItem).RelacoesUsuarios.Add(relacoes[i]);

			return itens;
		}

		public Item GetById(int id)
		{
			return dbContext.Itens.First(i => i.Id == id);
		}

		public ItemUsuario GetByKey(int idUsuario, int idItem)
		{
			return dbContext.ItensUsuario.FirstOrDefault(i => i.IdUsuario == idUsuario && i.IdItem == idItem);
		}

		public void Add(Item item)
		{
			dbContext.Itens.Add(item);
			dbContext.SaveChanges();
		}

		public void AddRange(List<Item> itens)
		{
			dbContext.Itens.AddRange(itens);
			dbContext.SaveChanges();
		}

		public void Add(ItemUsuario itemUsuario)
		{
			dbContext.ItensUsuario.Add(itemUsuario);
			dbContext.SaveChanges();
		}

		public void Update(Item item)
		{
			dbContext.Itens.Update(item);
			dbContext.SaveChanges();
		}

		public void Update(ItemUsuario itemUsuario)
		{
			dbContext.ItensUsuario.Update(itemUsuario);
			dbContext.SaveChanges();
		}

		public void Delete(Item item)
		{
			dbContext.Itens.Remove(item);
			dbContext.SaveChanges();
		}

		public void Delete(ItemUsuario itemUsuario)
		{
			dbContext.ItensUsuario.Remove(itemUsuario);
			dbContext.SaveChanges();
		}
	}
}
