﻿using AutoMapper;
using MinhasColecoes.Aplicacao.Enumerators;
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
	public class ItemProfile : Profile
	{
		public ItemProfile()
		{
			CreateMap<ItemInputModel, Item>()
				.ForMember(d => d.IdDonoParticular, opt => opt.MapFrom(s => s.IdUsuario));
			CreateMap<RelacaoItemUsuarioViewModel, ItemUsuario>();

			CreateMap<Item, ItemBasicViewModel>().IncludeMembers(r => r.RelacoesUsuarios.FirstOrDefault());
			CreateMap<ItemUsuario, ItemBasicViewModel>();
			CreateMap<ItemUsuario, EnumRelacaoUsuarioItem>(MemberList.None)
				.ConvertUsing(src => (EnumRelacaoUsuarioItem)src.Relacao);
		}
	}
}
