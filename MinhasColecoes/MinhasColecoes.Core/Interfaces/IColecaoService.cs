using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Interfaces
{
	public interface IColecaoService
	{
		void SetUsuario(int idUsuario);
		ColecaoViewModel Create(ColecaoInputModel input);
		void Update(ColecaoUpdateModel update);
		void TransferirParaMembro(int idColecao, int idMembro);
		void AdicionarSubcolecao(int idColecao, int idSubcolecao);
		void Delete(int idColecao);
		ColecaoViewModel GetById(int idColecao);
		IEnumerable<ColecaoBasicViewModel> GetAll(string nome);
		IEnumerable<ColecaoBasicViewModel> GetAllProprias(string nome);
		IEnumerable<ColecaoBasicViewModel> GetAllParticipa(string nome);
		IEnumerable<ColecaoBasicViewModel> GetAllSubcolecoes(int idColecao);
		IEnumerable<UsuarioBasicViewModel> GetAllMembros(int idColecao);
	}
}
