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
	public class UsuarioRepository : IUsuarioRepository
	{
		private readonly MinhasColecoesDbContext dbContext;

		public UsuarioRepository(MinhasColecoesDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public Usuario Get(string login, string senha)
		{
			return dbContext.Usuarios.FirstOrDefault(u => u.Login.Equals(login) && u.Senha.Equals(senha));
		}
	}
}
