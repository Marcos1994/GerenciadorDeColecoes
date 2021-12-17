﻿using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IItemRepository : IRepository
	{
		/// <summary>
		/// Retorna todos os itens originais de uma determinada coleção.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <returns>Itens da coleção apenas com informações básicas.</returns>
		IEnumerable<Item> GetAll(int idColecao);

		/// <summary>
		/// Retorna todos os itens de uma determinada coleção considerando as modificações do usuário específico.
		/// </summary>
		/// <param name="idColecao"></param>
		/// <param name="idUsuario"></param>
		/// <returns>Itens da coleção com informações do item original, caso possua, e relações com o usuário especificado.</returns>
		IEnumerable<Item> GetAllPessoais(int idColecao, int idUsuario);

		/// <summary>
		/// Retorna um item com base no Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Item apenas com informações básicas.</returns>
		Item GetById(int id);

		/// <summary>
		/// Retorna um item com base no Id contendo todas as informações que o relacionam com determinado usuário, caso possua alguma.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="idUsuario"></param>
		/// <returns>Item com informações de sua relação com usuário e o Item Original caso possua.</returns>
		Item GetById(int id, int idUsuario);

		/// <summary>
		/// Procura por uma relação entre um usuário e um item.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <param name="idItem"></param>
		/// <returns>Relação entre Usuário e Item caso exista ou Null caso não exista.</returns>
		ItemUsuario GetByKey(int idUsuario, int idItem);

		/// <summary>
		/// Cadastra um item.
		/// </summary>
		/// <param name="item"></param>
		void Add(Item item);

		/// <summary>
		/// Cadastra uma coleção de itens.
		/// </summary>
		/// <param name="itens"></param>
		void AddRange(List<Item> itens);

		/// <summary>
		/// Cadastra uma relação entre item e usuário.
		/// </summary>
		/// <param name="itemUsuario"></param>
		void Add(ItemUsuario itemUsuario);

		/// <summary>
		/// Atualiza um item.
		/// </summary>
		/// <param name="item"></param>
		void Update(Item item);

		/// <summary>
		/// Atualiza uma relação entre item e usuário.
		/// </summary>
		/// <param name="itemUsuario"></param>
		void Update(ItemUsuario itemUsuario);

		/// <summary>
		/// Exclui um item.
		/// </summary>
		/// <param name="item"></param>
		void Delete(Item item);

		/// <summary>
		/// Exclui uma relação entre item e usuário.
		/// </summary>
		/// <param name="itemUsuario"></param>
		void Delete(ItemUsuario itemUsuario);
	}
}
