﻿using MinhasColecoes.Persistencia.Context;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Repositories
{
	public class UsuarioRepository : IUsuarioRepository
	{
		private readonly MinhasColecoesDbContext dbContext;

		public UsuarioRepository(MinhasColecoesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public Usuario Get(string login, string senha)
		{
			Usuario usuario = this.GetByLogin(login);
			return (usuario != null && usuario.Senha == senha)
				? usuario
				: null;
		}

		public Usuario GetById(int id)
		{
			return dbContext.Usuarios.First(u => u.Id == id);
		}

		public Usuario GetByLogin(string login)
		{
			return dbContext.Usuarios.FirstOrDefault(u => u.Login == login);
		}

		public void Create(Usuario usuario)
		{
			dbContext.Add(usuario);
			dbContext.SaveChanges();
		}

		public void Update(Usuario usuario)
		{
			dbContext.Update(usuario);
			dbContext.SaveChanges();
		}
	}
}
