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
	class UsuarioProfile : Profile
	{
		public UsuarioProfile()
		{
			CreateMap<Usuario, UsuarioBasicViewModel>();
		}
	}
}
