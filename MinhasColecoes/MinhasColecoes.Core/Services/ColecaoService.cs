using AutoMapper;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
using MinhasColecoes.Persistencia.Exceptions;
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
			bool nomeRepetido = repositorioColecao.GetAllSubcolecoes(input.IdDono, input.IdColecaoMaior, input.Nome)
				.Any(c => c.Nome.Equals(input.Nome));
			if (nomeRepetido) //São consideradas apenas as coleções que são visiveis para o usuário.
				throw new FalhaDeValidacaoException(
					"Nome", "Já existe uma coleção com este nome.");

			Colecao colecao = mapper.Map<Colecao>(input);
			repositorioColecao.Add(colecao);
			return mapper.Map<ColecaoViewModel>(colecao);
		}

		public void Update(int idUsuario, ColecaoUpdateModel update)
		{
			Colecao colecao = repositorioColecao.GetById(update.Id);
			if (colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("atualizar", "Coleção");

			bool nomeRepetido = repositorioColecao.GetAllSubcolecoes(idUsuario, colecao.IdColecaoMaior, update.Nome)
				.Any(c => c.Id != update.Id
				&& c.Nome.Equals(update.Nome));
			if (nomeRepetido)
			{
				if (!colecao.Publica && update.Publica)
					throw new FalhaDeValidacaoException(
						"Nome", "Já existe uma coleção pública com este nome.");
				else
					throw new FalhaDeValidacaoException(
						"Nome", "Já existe uma coleção com este nome.");
			}

			if (!update.Publica && colecao.Publica)
			{
				bool possuiOutrosMembros = repositorioColecao.GetMembros(update.Id).Count() > 1;
				if (possuiOutrosMembros)
					throw new FalhaDeValidacaoException("Não é possível tornar privada uma coleção pública que já possui algum outro membro.");
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
				throw new UsuarioNaoAutorizadoException("adicionar esta coleção como subcoleção de outra");

			bool nomeRepetido = repositorioColecao.GetAllSubcolecoes(idUsuario, idColecao, subcolecao.Nome)
				.Any(c => c.Id != subcolecao.Id
				&& c.Nome.Equals(subcolecao.Nome));
			if (nomeRepetido)
			{
				if (idColecao == null)
					throw new FalhaDeValidacaoException("Já existe uma coleção pública com esse nome.");
				else
					throw new FalhaDeValidacaoException(
						"Nome", "Já existe uma subcoleção com este nome.");
			}

			//Verifico se não haverá referência ciclica entre as coleções
			if (idColecao != null
				&& VerificarGenealogia(GetAllSupercolecoes((int)idColecao), idSubcolecao))
				throw new FalhaDeValidacaoException("A supercoleção a ser definida é descendente desta coleção.");

			try
			{
				subcolecao.SetColecaoMaior(idColecao);
			}
			catch (ObjetoNaoEncontradoException ex)
			{
				throw new ObjetoNaoEncontradoException("supercoleção", ex.InnerException);
			}

			repositorioColecao.Update(subcolecao);
		}

		public void AdicionarMembro(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);

			if (!colecao.Publica)
				throw new UsuarioNaoAutorizadoException("participar desta coleção");

			repositorioColecao.Add(new ColecaoUsuario(idUsuario, idColecao));
		}

		public void Delete(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("Excluir coleção");

			if (colecao.Publica)
			{
				bool possuiOutrosMembros = repositorioColecao.GetMembros(idColecao).Count() > 1;
				if (possuiOutrosMembros)
					throw new FalhaDeValidacaoException("Não é possível excluir uma coleção que já possui algum outro membro.");
			}
			repositorioColecao.Delete(colecao);
		}

		public ColecaoViewModel GetById(int idUsuario, int idColecao)
		{
			Colecao colecao = repositorioColecao.GetById(idColecao);
			if (!colecao.Publica && colecao.IdDono != idUsuario)
				throw new UsuarioNaoAutorizadoException("acessar", "coleção");
			ColecaoViewModel colecaoView = mapper.Map<ColecaoViewModel>(colecao);
			colecaoView.SetUsuarioParticipa(repositorioColecao.GetMembros(colecao.Id).Any(m => m.Id == idUsuario));
			
			return colecaoView;
		}

		public IEnumerable<ColecaoBasicViewModel> GetAll(int idUsuario, string nome = "")
		{
			return SetQuantidadeMembosESubcolecoes(idUsuario,
				repositorioColecao.GetAll(idUsuario, nome));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllProprias(int idUsuario, string nome = "")
		{
			return SetQuantidadeMembosESubcolecoes(idUsuario,
				repositorioColecao.GetAllPessoais(idUsuario)
				.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllParticipa(int idAutenticado, int idUsuario, string nome = "")
		{
			bool incluirPrivadas = idAutenticado == idUsuario;
			return SetQuantidadeMembosESubcolecoes(idUsuario,
				repositorioColecao.GetAllMembro(idUsuario, incluirPrivadas)
				.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)));
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idUsuario, int? idColecao, string nome = "")
		{
			return SetQuantidadeMembosESubcolecoes(idUsuario, repositorioColecao.GetAllSubcolecoes(idUsuario, idColecao, nome));
		}

		public ColecaoGenealogiaViewModel GetAllSupercolecoes(int idColecao)
		{
			//Pego a coleção
			ColecaoGenealogiaViewModel colecao = mapper.Map<ColecaoGenealogiaViewModel>(repositorioColecao.GetById(idColecao));

			//Se ela for a coleção raiz eu retorno
			if (colecao.IdColecaoMaior == null)
				return colecao;

			//Se não, procuro pelo pai dela, até encontrar a raiz, defino como pai e depois retorno
			colecao.SetPai(GetAllSupercolecoes((int) colecao.IdColecaoMaior));
			return colecao;
		}

		public IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao)
		{
			return mapper.Map<List<UsuarioBasicViewModel>>(repositorioColecao.GetMembros(idColecao));
		}
	
		/// <summary>
		/// Verifica se uma coleção está presente em uma genealogia
		/// </summary>
		/// <param name="genealogia"></param>
		/// <param name="idColecao"></param>
		/// <returns></returns>
		public bool VerificarGenealogia(ColecaoGenealogiaViewModel genealogia, int idColecao)
		{
			//A coleção está na genealogia?
			if (genealogia.Id == idColecao)
				return true;

			//Ainda tem pai pra verificar?
			if(genealogia.IdColecaoMaior == null)
				return false;

			//Verifica se o ID dela bate com o do pai
			return VerificarGenealogia(genealogia.ColecaoPai, idColecao);
		}

		private List<ColecaoBasicViewModel> SetQuantidadeMembosESubcolecoes(int idUsuario, IEnumerable<Colecao> colecoes)
		{
			List<ColecaoBasicViewModel> colecoesView = mapper.Map<List<ColecaoBasicViewModel>>(colecoes);
			List<Usuario> membros;
			for (int i = 0; i < colecoesView.Count; i++)
			{
				membros = repositorioColecao.GetMembros(colecoesView[i].Id).ToList();
				colecoesView[i].SetQuantidadeMembros(membros.Count());
				colecoesView[i].SetUsuarioParticipa(membros.Any(m => m.Id == idUsuario));
				colecoesView[i].SetQuantidadeSubcolecoes(repositorioColecao.GetAllSubcolecoes(idUsuario, colecoesView[i].Id).Count());
			}
			return colecoesView;
		}
	}
}
