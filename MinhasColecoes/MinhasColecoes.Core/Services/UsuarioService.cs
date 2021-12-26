using AutoMapper;
using MinhasColecoes.Aplicacao.Exceptions;
using MinhasColecoes.Aplicacao.Interfaces;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using MinhasColecoes.Aplicacao.Models.View;
using MinhasColecoes.Persistencia.Entities;
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

		public UsuarioViewModel GetById(int id)
		{
			return mapper.Map<UsuarioViewModel>(repositorioUsuario.GetById(id));
		}

		public UsuarioViewModel Create(UsuarioInputModel input)
		{
			bool loginExistente = repositorioUsuario.GetByLogin(input.Login) != null;
			if (loginExistente)
				throw new ObjetoDuplicadoException("usuário", "login");
			Usuario usuario = mapper.Map<Usuario>(input);
			repositorioUsuario.Create(usuario);
			return mapper.Map<UsuarioViewModel>(usuario);
		}

		public void Update(UsuarioUpdateModel update)
		{
			Usuario usuario = repositorioUsuario.GetById(update.Id);
			usuario.Update(update.Nome, update.Descricao, update.Foto);
			repositorioUsuario.Update(usuario);
		}

		public void Update(UsuarioSenhaUpdateModel update)
		{
			Usuario usuario = repositorioUsuario.Get(update.Login, update.Senha);
			if (usuario == null)
				throw new FalhaDeValidacaoException("Combinação de Login e Senha inválida.");
			usuario.UpdateSenha(update.NovaSenha);
			repositorioUsuario.Update(usuario);
		}
	}
}
