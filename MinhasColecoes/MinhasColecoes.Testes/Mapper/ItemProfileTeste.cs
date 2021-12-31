using AutoMapper;
using MinhasColecoes.Aplicacao.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Testes.Mapper
{
	public class ItemProfileTeste
	{
		private readonly IMapper mapper;

		public ItemProfileTeste()
		{
			if (mapper == null)
			{
				var mappingConfig = new MapperConfiguration(mc =>
				{
					mc.AddProfile(new ColecaoProfile());
					mc.AddProfile(new ItemProfile());
					mc.AddProfile(new UsuarioProfile());
				});
				mapper = mappingConfig.CreateMapper();
			}
		}
	}
}
