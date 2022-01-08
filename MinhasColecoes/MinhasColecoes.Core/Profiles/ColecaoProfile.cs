using AutoMapper;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Profiles
{
	public class ColecaoProfile : Profile
	{
		public ColecaoProfile()
		{
			CreateMap<ColecaoInputModel, Colecao>()
				.ForMember(d => d.UsuariosColecao, opt => opt.MapFrom(s => new[] { new ColecaoUsuario(s.IdDono, 0) }))
				.ForMember(d => d.IdColecaoMaior, opt => opt.MapFrom(s => (s.IdColecaoMaior > 0) ? s.IdColecaoMaior : null));

			CreateMap<Colecao, ColecaoBasicViewModel>();
			CreateMap<Colecao, ColecaoViewModel>()
				.ForMember(view => view.Dono, opt => opt.MapFrom(ent => new UsuarioBasicViewModel(ent.IdDono, "")))
				.ForMember(view => view.ColecaoMaior, opt => opt.MapFrom(ent =>
				(ent.IdColecaoMaior > 0) ? new ColecaoBasicViewModel((int)ent.IdColecaoMaior) : null));
			CreateMap<ColecaoUsuario, UsuarioBasicViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.IdUsuario));

			CreateMap<Colecao, ColecaoGenealogiaViewModel>();
		}
	}
}
