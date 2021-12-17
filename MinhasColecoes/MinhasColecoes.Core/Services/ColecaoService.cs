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
		private readonly IColecaoRepository repositoryColecao;
		private readonly IItemRepository repositoryItem;
		private int _idUsuario;

		public ColecaoService(IMapper mapper, IColecaoRepository repositoryColecao, IItemRepository repositoryItem)
		{
			this.mapper = mapper;
			this.repositoryColecao = repositoryColecao;
			this.repositoryItem = repositoryItem;
		}

		public void SetUsuario(int idUsuario)
		{
			_idUsuario = idUsuario;
		}

		public ColecaoViewModel Create(ColecaoInputModel input)
		{
			bool nomeRepetido = repositoryColecao.GetAll(_idUsuario).Any(c => c.Nome.ToLower() == input.Nome.ToLower());
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			input.IdDono = _idUsuario;
			Colecao colecao = mapper.Map<Colecao>(input);
			repositoryColecao.Add(colecao);
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public void Update(ColecaoUpdateModel update)
		{
			bool nomeRepetido = repositoryColecao.GetAll(_idUsuario).Any(c => c.Nome.ToLower() == update.Nome.ToLower());
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			Colecao colecao = repositoryColecao.GetById(update.Id);
			if (colecao.IdDono != _idUsuario)
				throw new Exception("O usuário atual não é o dono da coleção a ser atualizada.");

			if (!update.Publica && colecao.Publica)
			{
				bool possuiOutrosMembros = repositoryColecao.GetMembros(update.Id).Count() > 1;
				if (possuiOutrosMembros)
					throw new Exception("Não é possível tornar privada uma coleção pública que já possui algum outro membro.");
			}

			colecao.Update(update.Nome, update.Descricao, update.Foto, update.Publica);
			repositoryColecao.Update(colecao);
		}

		public void TransferirParaMembro(int idColecao, int idMembro)
		{
			throw new NotImplementedException();
		}

		public void AdicionarSubcolecao(int idSubcolecao, int? idColecao)
		{
			Colecao subcolecao = repositoryColecao.GetById(idSubcolecao);

			if (subcolecao.IdDono != _idUsuario)
				throw new Exception("O usuário atual não é o dono da coleção a ser posta como subcoleção.");

			subcolecao.SetColecaoMaior(idColecao);
			repositoryColecao.Update(subcolecao);
		}

		public void AdicionarMembro(int idColecao)
		{
			repositoryColecao.Add(new ColecaoUsuario(_idUsuario, idColecao));
		}

		public void Delete(int idColecao)
		{
			Colecao colecao = repositoryColecao.GetById(idColecao);
			if (colecao.IdDono == _idUsuario)
			{
				if (colecao.Publica)
				{
					bool possuiOutrosMembros = repositoryColecao.GetMembros(idColecao).Count() > 1;
					if (possuiOutrosMembros)
						throw new Exception("Não é possível excluir uma coleção que já possui algum outro membro.");
				}
				repositoryColecao.Delete(colecao);
			}
			else
			{
				ColecaoUsuario relacao = new ColecaoUsuario(_idUsuario, idColecao);
				repositoryColecao.Delete(relacao);
			}
		}

		public ColecaoViewModel GetById(int idColecao)
		{
			Colecao colecao = repositoryColecao.GetById(idColecao);
			colecao.Colecoes.AddRange(repositoryColecao.GetAllSubcolecoes(_idUsuario, idColecao));
			colecao.Itens.AddRange(repositoryItem.GetAllPessoais(idColecao, _idUsuario));
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public IEnumerable<ColecaoBasicViewModel> GetAll(string nome)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositoryColecao.GetAll(_idUsuario, nome));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllProprias(string nome)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositoryColecao.GetAllPessoais(_idUsuario).Where(c => c.Nome.Contains(nome)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllParticipa(string nome)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositoryColecao.GetAllMembro(_idUsuario).Where(c => c.Nome.Contains(nome)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idColecao)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositoryColecao.GetAllSubcolecoes(_idUsuario, idColecao)));
		}

		public IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao)
		{
			return mapper.Map<List<UsuarioBasicViewModel>>(repositoryColecao.GetMembros(idColecao));
		}
	}
}
