using Microsoft.EntityFrameworkCore;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Context
{
	public class MinhasColecoesContext : DbContext
	{
		public MinhasColecoesContext(DbContextOptions<MinhasColecoesContext> options)
			: base(options)
		{

		}

		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Colecao> Colecoes { get; set; }
		public DbSet<ColecaoUsuario> ColecoesUsuario { get; set; }
		public DbSet<Item> Itens { get; set; }
		public DbSet<ItemUsuario> ItensUsuario { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/* --------------- USUARIO --------------- */
			modelBuilder.Entity<Usuario>()
				.HasKey(u => u.Id);

			modelBuilder.Entity<Usuario>()
				.HasMany(u => u.ColecoesDono)
				.WithOne()
				.HasForeignKey(c => c.IdDono)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Usuario>()
				.HasMany(u => u.ColecoesParticipa)
				.WithOne()
				.HasForeignKey(c => c.IdUsuario)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Usuario>()
				.HasMany(u => u.ItensDono)
				.WithOne()
				.HasForeignKey(i => i.IdDonoParticular)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Usuario>()
				.HasMany(u => u.RelacoesItens)
				.WithOne()
				.HasForeignKey(i => i.IdUsuario)
				.OnDelete(DeleteBehavior.Cascade);

			/* --------------- COLECAO --------------- */
			modelBuilder.Entity<Colecao>()
				.HasKey(c => c.Id);

			modelBuilder.Entity<Colecao>()
				.HasMany(c => c.Itens)
				.WithOne()
				.HasForeignKey(i => i.IdColecao);

			modelBuilder.Entity<Colecao>()
				.HasMany(c => c.UsuariosColecao)
				.WithOne()
				.HasForeignKey(u => u.IdColecao);

			modelBuilder.Entity<Colecao>()
				.HasMany(c => c.Colecoes)
				.WithOne()
				.HasForeignKey(cc => cc.IdColecaoMaior);

			/* --------------- ITEM --------------- */
			modelBuilder.Entity<Item>()
				.HasKey(i => i.Id);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.ItemOriginal)
				.WithOne()
				.HasForeignKey<Item>(io => io.IdOriginal);

			/* --------------- COLECAO DO USUARIO --------------- */
			modelBuilder.Entity<ColecaoUsuario>()
				.HasKey(c => new { c.IdColecao, c.IdUsuario });

			/* --------------- ITEM DO USUARIO --------------- */
			modelBuilder.Entity<ItemUsuario>()
				.HasKey(i => new { i.IdItem, i.IdUsuario });
		}
	}
}
