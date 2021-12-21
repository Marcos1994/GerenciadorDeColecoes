﻿using Microsoft.EntityFrameworkCore;
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

			List<int> idsSubstituidos = (from i in itens
										 where i.IdOriginal != null
										 select (int)i.IdOriginal).ToList();

			//Pegar apenas os itens que não são modificações particulares de outros itens
			List<Item> itensPessoais = itens.Where(i => !idsSubstituidos.Contains(i.Id)).ToList();

			for (int j = 0; j < itensPessoais.Count(); j++)
				if (itensPessoais[j].IdOriginal != null)
					itensPessoais[j].SetItemOriginal(itens.FirstOrDefault(i => i.Id == (int)itensPessoais[j].IdOriginal));

			List<ItemUsuario> relacoes = (from iu in dbContext.ItensUsuario
										  where iu.IdUsuario == idUsuario
										  join i in itensPessoais
										  on iu.IdItem equals i.Id
										  select iu).ToList();

			for (int i = 0; i < relacoes.Count(); i++)
				itensPessoais.First(item => item.Id == relacoes[i].IdItem).RelacoesUsuarios.Add(relacoes[i]);

			return itensPessoais;
		}

		public Item GetById(int id)
		{
			return dbContext.Itens.First(i => i.Id == id);
		}

		public Item GetById(int id, int idUsuario)
		{
			Item item = dbContext.Itens.First(i => i.Id == id);
			if (item.IdOriginal != null)
				item.SetItemOriginal(dbContext.Itens.First(i => i.Id == (int)item.IdOriginal));
			item.RelacoesUsuarios.Add(dbContext.ItensUsuario.FirstOrDefault(iu => iu.IdItem == id && iu.IdUsuario == idUsuario));
			return item;
		}

		public Item GetByCodigo(int idColecao, string codigo)
		{
			if (codigo.Length == 0)
				return null;
			return dbContext.Itens
				.FirstOrDefault(i =>
				i.IdColecao == idColecao
				&& i.Original
				&& i.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
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
