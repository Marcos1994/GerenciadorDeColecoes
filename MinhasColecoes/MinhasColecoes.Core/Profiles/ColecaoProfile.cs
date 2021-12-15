using AutoMapper;
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
			CreateMap<Colecao, ColecaoBasicViewModel>();
			CreateMap<Colecao, ColecaoViewModel>()
				.ForMember(view => view.Dono, opt => opt.MapFrom(ent => new UsuarioBasicViewModel(ent.IdDono)));
		}
	}
}
