using AutoMapper;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Services
{
	public class ColecaoService : IColecaoService
	{
		private readonly IMapper mapper;
		private readonly IColecaoRepository repositorioColecao;
		private readonly IItemRepository repositorioItem;
		private int _idUsuario;

		public ColecaoService(IMapper mapper, IColecaoRepository repositoryColecao, IItemRepository repositoryItem)
		{
			this.mapper = mapper;
			this.repositorioColecao = repositoryColecao;
			this.repositorioItem = repositoryItem;
		}

		public void SetUsuario(int idUsuario)
		{
			_idUsuario = idUsuario;
		}

		public ColecaoViewModel Create(ColecaoInputModel input)
		{
			bool nomeRepetido = repositorioColecao.GetAll(_idUsuario).Any(c => c.Nome.ToLower() == input.Nome.ToLower());
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			input.IdDono = _idUsuario;
			Colecao colecao = mapper.Map<Colecao>(input);
			repositorioColecao.Add(colecao);
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public void Update(ColecaoUpdateModel update)
		{
			bool nomeRepetido = repositorioColecao.GetAll(_idUsuario).Any(c => c.Nome.ToLower() == update.Nome.ToLower());
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			Colecao colecao = repositorioColecao.GetById(update.Id);
			if (colecao.IdDono != _idUsuario)
				throw new Exception("O usuário atual não tem permissão para atualizar a coleção.");

			if (!update.Publica && colecao.Publica)
			{
				bool possuiOutrosMembros = repositorioColecao.GetMembros(update.Id).Count() > 1;
				if (possuiOutrosMembros)
					throw new Exception("Não é possível tornar privada uma coleção pública que já possui algum outro membro.");
			}

			colecao.Update(update.Nome, update.Descricao, update.Foto, update.Publica);
			repositorioColecao.Update(colecao);
		}

		public void TransferirParaMembro(int idColecao, int idMembro)
		{
			throw new NotImplementedException();
		}

		public void AdicionarSubcolecao(int idSubcolecao, int? idColecao)
		{
			Colecao subcolecao = repositorioColecao.GetById(idSubcolecao);

			if (subcolecao.IdDono != _idUsuario)
				throw new Exception("O usuário atual não tem permissão para adicionar esta coleção como subcoleção.");

			subcolecao.SetColecaoMaior(idColecao);
			repositorioColecao.Update(subcolecao);
		}

		public void AdicionarMembro(int idColecao)
		{
			repositorioColecao.Add(new ColecaoUsuario(_idUsuario, idColecao));
		}

		public void Delete(int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (colecao.IdDono == _idUsuario)
			{
				if (colecao.Publica)
				{
					bool possuiOutrosMembros = repositorioColecao.GetMembros(idColecao).Count() > 1;
					if (possuiOutrosMembros)
						throw new Exception("Não é possível excluir uma coleção que já possui algum outro membro.");
				}
				repositorioColecao.Delete(colecao);
			}
			else
			{
				ColecaoUsuario relacao = new ColecaoUsuario(_idUsuario, idColecao);
				repositorioColecao.Delete(relacao);
			}
		}

		public ColecaoViewModel GetById(int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			colecao.Colecoes.AddRange(repositorioColecao.GetAllSubcolecoes(_idUsuario, idColecao));
			colecao.Itens.AddRange(repositorioItem.GetAllPessoais(idColecao, _idUsuario));
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public IEnumerable<ColecaoBasicViewModel> GetAll(string nome = "")
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAll(_idUsuario, nome));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllProprias(string nome = "")
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllPessoais(_idUsuario).Where(c => c.Nome.Contains(nome)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllParticipa(string nome = "")
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllMembro(_idUsuario).Where(c => c.Nome.Contains(nome)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idColecao)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllSubcolecoes(_idUsuario, idColecao));
		}

		public IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao)
		{
			return mapper.Map<List<UsuarioBasicViewModel>>(repositorioColecao.GetMembros(idColecao));
		}
	}
}
