using AutoMapper;
using MinhasColecoes.Aplicacao.Enumerators;
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
	public class ItemProfile : Profile
	{
		public ItemProfile()
		{
			CreateMap<ItemUpdateModel, ItemInputModel>();

			CreateMap<ItemInputModel, Item>()
				.ForMember(d => d.RelacoesUsuarios, opt =>
				{
					opt.Condition(s => s.Relacao != EnumRelacaoUsuarioItem.NaoPossuo);
					opt.MapFrom(s => new[] { new RelacaoItemUsuarioInputModel { Relacao = s.Relacao, IdUsuario = s.IdUsuario } });
				});
			CreateMap<RelacaoItemUsuarioInputModel, ItemUsuario>()
				.ConvertUsing(s => (s.Relacao == EnumRelacaoUsuarioItem.NaoPossuo)
					? null : new ItemUsuario(s.IdUsuario, s.IdItem, (int)s.Relacao, s.Comentario));

			CreateMap<Item, ItemViewModel>()
				.IncludeBase<Item, ItemBasicViewModel>();
			CreateMap<Item, ItemBasicViewModel>()
				.IncludeMembers(r => r.RelacoesUsuarios.FirstOrDefault());
			CreateMap<ItemUsuario, ItemBasicViewModel>();
			CreateMap<ItemUsuario, EnumRelacaoUsuarioItem>(MemberList.None)
				.ConvertUsing(src => (EnumRelacaoUsuarioItem)src.Relacao);

		}
	}
}
