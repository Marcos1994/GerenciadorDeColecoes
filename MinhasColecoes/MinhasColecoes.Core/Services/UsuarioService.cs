using AutoMapper;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Services
{
	public class UsuarioService : IUsuarioService
	{
		private readonly IUsuarioRepository repositorioUsuario;
		private readonly IMapper mapper;

		public UsuarioService(IUsuarioRepository repositorioUsuario, IMapper mapper)
		{
			this.repositorioUsuario = repositorioUsuario;
			this.mapper = mapper;
		}

		public UsuarioLoginViewModel ValidarUsuario(UsuarioLoginInputModel usuario)
		{
			return mapper.Map<UsuarioLoginViewModel>(repositorioUsuario.Get(usuario.Login, usuario.Senha));
		}
	}
}
