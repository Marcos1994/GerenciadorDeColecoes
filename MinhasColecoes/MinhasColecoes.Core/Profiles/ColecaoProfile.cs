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
				.ForMember(d => d.UsuariosColecao, opt => opt.MapFrom(s => new[] { new ColecaoUsuario(s.IdDono, 0) }));

			CreateMap<Colecao, ColecaoBasicViewModel>();
			CreateMap<Colecao, ColecaoViewModel>()
				.ForMember(view => view.Dono, opt => opt.MapFrom(ent => new UsuarioBasicViewModel(ent.IdDono, "")));
			CreateMap<ColecaoUsuario, UsuarioBasicViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.IdUsuario));
		}
	}
}
