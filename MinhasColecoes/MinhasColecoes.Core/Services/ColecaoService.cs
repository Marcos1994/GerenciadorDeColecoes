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
		private int? _idUsuario;

		public ColecaoService(IMapper mapper, IColecaoRepository repositoryColecao)
		{
			this.mapper = mapper;
			this.repositoryColecao = repositoryColecao;
		}

		public void SetUsuario(int idUsuario)
		{
			_idUsuario = idUsuario;
		}

		public ColecaoViewModel Create(ColecaoInputModel input)
		{
			throw new NotImplementedException();
		}

		public void Update(ColecaoUpdateModel update)
		{
			throw new NotImplementedException();
		}

		public void TransferirParaMembro(int idColecao, int idMembro)
		{
			throw new NotImplementedException();
		}

		public void AdicionarSubcolecao(int idColecao, int idSubcolecao)
		{
			throw new NotImplementedException();
		}

		public void Delete(int idColecao)
		{
			throw new NotImplementedException();
		}

		public ColecaoViewModel GetById(int idColecao)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ColecaoBasicViewModel> GetAll(string nome)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllProprias(string nome)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllParticipa(string nome)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idColecao)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao)
		{
			throw new NotImplementedException();
		}
	}
}
