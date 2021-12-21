using AutoMapper;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Profiles
{
	public class UsuarioProfile : Profile
	{
		public UsuarioProfile()
		{
			CreateMap<UsuarioInputModel, Usuario>();
			CreateMap<Usuario, UsuarioBasicViewModel>();
			CreateMap<Usuario, UsuarioViewModel>();
			CreateMap<Usuario, UsuarioLoginViewModel>();
		}
	}
}
