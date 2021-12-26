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

		public ColecaoService(IMapper mapper, IColecaoRepository repositoryColecao, IItemRepository repositoryItem)
		{
			this.mapper = mapper;
			this.repositorioColecao = repositoryColecao;
			this.repositorioItem = repositoryItem;
		}

		public ColecaoViewModel Create(ColecaoInputModel input)
		{
			bool nomeRepetido = repositorioColecao.GetAll(input.IdDono, input.Nome).Any();
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			Colecao colecao = mapper.Map<Colecao>(input);
			repositorioColecao.Add(colecao);
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public void Update(int idUsuario, ColecaoUpdateModel update)
		{
			bool nomeRepetido = repositorioColecao.GetAll(idUsuario, update.Nome).Any(c => c.Id != update.Id);
			if (nomeRepetido)
				throw new Exception("Já existe uma coleção com esse nome.");

			Colecao colecao = repositorioColecao.GetById(update.Id);
			if (colecao.IdDono != idUsuario)
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

		public void TransferirParaMembro(int idDono, int idMembro, int idColecao)
		{
			throw new NotImplementedException();
		}

		public void AdicionarSupercolecao(int idUsuario, int idSubcolecao, int? idColecao)
		{//ATUALIZAR: Caso o usuário não seja dono da supercoleção, deverá ser criada uma solicitação.
			Colecao subcolecao = repositorioColecao.GetById(idSubcolecao);

			if (subcolecao.IdDono != idUsuario)
				throw new Exception("O usuário atual não tem permissão para adicionar esta coleção como subcoleção.");

			subcolecao.SetColecaoMaior(idColecao);
			repositorioColecao.Update(subcolecao);
		}

		public void AdicionarMembro(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);

			if(colecao.IdDono == idUsuario)
				throw new Exception("O usuário já é membro dessa coleção.");

			if (!colecao.Publica)
				throw new Exception("O usuário não tem permissão para participar desta coleção.");

			repositorioColecao.Add(new ColecaoUsuario(idUsuario, idColecao));
		}

		public void Delete(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (colecao.IdDono == idUsuario)
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
				ColecaoUsuario relacao = new ColecaoUsuario(idUsuario, idColecao);
				try
				{
					repositorioColecao.StartTransaction("DesvincularUsuarioColecao");

					repositorioColecao.Delete(relacao);
					repositorioItem.DeleteItensParticulares(relacao);
					repositorioItem.DeleteRelacoes(relacao);

					repositorioColecao.FinishTransaction();
				}
				catch (Exception ex)
				{
					repositorioColecao.RollbackTransaction("DesvincularUsuarioColecao");
					throw ex;
				}

			}
		}

		public ColecaoViewModel GetById(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (!colecao.Publica && colecao.IdDono != idUsuario)
				throw new Exception("O usuário não tem permissão para acessar essa coleção.");
			ColecaoViewModel colecaoView = mapper.Map<ColecaoViewModel>(colecao);
			colecaoView.Colecoes.AddRange(mapper.Map<List<ColecaoBasicViewModel>>(repositorioColecao.GetAllSubcolecoes(idUsuario, idColecao)));
			colecaoView.Itens.AddRange(mapper.Map<List<ItemBasicViewModel>>(repositorioItem.GetAllPessoais(idColecao, idUsuario)));
			return colecaoView;
		}

		public IEnumerable<ColecaoBasicViewModel> GetAll(int idUsuario, string nome = "")
		{
			List<Colecao> colecoes = repositorioColecao.GetAll(idUsuario, nome).ToList();
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>(colecoes);
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllProprias(int idUsuario, string nome = "")
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllPessoais(idUsuario).Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllParticipa(int idUsuario, string nome = "")
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllMembro(idUsuario).Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idUsuario, int idColecao)
		{
			return mapper.Map<IEnumerable<ColecaoBasicViewModel>>
				(repositorioColecao.GetAllSubcolecoes(idUsuario, idColecao));
		}

		public IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao)
		{
			return mapper.Map<List<UsuarioBasicViewModel>>(repositorioColecao.GetMembros(idColecao));
		}
	}
}
