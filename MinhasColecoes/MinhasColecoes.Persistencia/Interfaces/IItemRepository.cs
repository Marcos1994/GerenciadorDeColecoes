using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IItemRepository : IRepository
	{
		IEnumerable<Item> GetAll(int idColecao);
		IEnumerable<Item> GetAllPessoais(int idColecao, int idUsuario);
		Item GetById(int id);
		Item GetById(int id, int idUsuario);
		ItemUsuario GetByKey(int idUsuario, int idItem);
		void Add(Item item);
		void AddRange(List<Item> itens);
		void Add(ItemUsuario itemUsuario);
		void Update(Item item);
		void Update(ItemUsuario itemUsuario);
		void Delete(Item item);
		void Delete(ItemUsuario itemUsuario);
	}
}
