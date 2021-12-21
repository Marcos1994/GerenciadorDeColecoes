using FluentValidation;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Validators
{
	static class UsuarioStaticValidator
	{
		public static int NomeMinLength { get; private set; } = 2;
		public static int NomeMaxLength { get; private set; } = 120;
		public static int LoginMinLength { get; private set; } = 8;
		public static int LoginMaxLength { get; private set; } = 50;
		public static int SenhaMinLength { get; private set; } = 6;
		public static int SenhaMaxLength { get; private set; } = 50;
		public static int DescricaoMaxLength { get; private set; } = 250;
	}

	public class UsuarioInputValidator : AbstractValidator<UsuarioInputModel>
	{
		public UsuarioInputValidator()
		{
			RuleFor(u => u.Nome)
				.NotEmpty().WithMessage($"O nome de usuário deve ter pelo menos {UsuarioStaticValidator.NomeMinLength} caracteres.")
				.MinimumLength(UsuarioStaticValidator.NomeMinLength).WithMessage($"O nome de usuário deve ter pelo menos {UsuarioStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(UsuarioStaticValidator.NomeMaxLength).WithMessage($"O nome de usuário deve ter no máximo {UsuarioStaticValidator.NomeMaxLength} caracteres.");

			RuleFor(u => u.Login)
				.NotEmpty().WithMessage($"O login deve ter pelo menos {UsuarioStaticValidator.LoginMinLength} caracteres.")
				.MinimumLength(UsuarioStaticValidator.LoginMinLength).WithMessage($"O login deve ter pelo menos {UsuarioStaticValidator.LoginMinLength} caracteres.")
				.MaximumLength(UsuarioStaticValidator.LoginMaxLength).WithMessage($"O login deve ter no máximo {UsuarioStaticValidator.LoginMaxLength} caracteres.")
				.Matches(@"^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-za-za-z]{2,3})+$").WithMessage("O Login deve ser um endereço de e-mail válido.");

			RuleFor(u => u.Senha)
				.NotEmpty().WithMessage($"A senha não pode ser vazia.")
				.MinimumLength(UsuarioStaticValidator.SenhaMinLength).WithMessage($"A senha deve ter pelo menos {UsuarioStaticValidator.SenhaMinLength} caracteres.")
				.MaximumLength(UsuarioStaticValidator.SenhaMaxLength).WithMessage($"A senha deve ter no máximo {UsuarioStaticValidator.SenhaMaxLength} caracteres.");
		}
	}

	public class UsuarioLoginInputValidator : AbstractValidator<UsuarioLoginInputModel>
	{
		public UsuarioLoginInputValidator()
		{
			RuleFor(u => u.Login)
				.NotEmpty().WithMessage("O login não deve ser vazio.");

			RuleFor(u => u.Senha)
				.NotEmpty().WithMessage("A senha deve ser vazia.");
		}
	}

	public class UsuarioUpdateValidator : AbstractValidator<UsuarioUpdateModel>
	{
		public UsuarioUpdateValidator()
		{
			RuleFor(u => u.Nome)
				.NotEmpty().WithMessage($"O nome de usuário deve ter pelo menos {UsuarioStaticValidator.NomeMinLength} caracteres.")
				.MinimumLength(UsuarioStaticValidator.NomeMinLength).WithMessage($"O nome de usuário deve ter pelo menos {UsuarioStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(UsuarioStaticValidator.NomeMaxLength).WithMessage($"O nome de usuário deve ter no máximo {UsuarioStaticValidator.NomeMaxLength} caracteres.");

			RuleFor(u => u.Descricao)
				.MaximumLength(UsuarioStaticValidator.DescricaoMaxLength).WithMessage($"A descrição não deve ter mais que {UsuarioStaticValidator.DescricaoMaxLength} caracteres.");
		}
	}

	public class UsuarioSenhaUpdateValidator : AbstractValidator<UsuarioSenhaUpdateModel>
	{
		public UsuarioSenhaUpdateValidator()
		{
			RuleFor(u => u.Login)
				.NotEmpty().WithMessage("O login não deve ser vazio.");

			RuleFor(u => u.Senha)
				.NotEmpty().WithMessage($"A senha não pode ser vazia.")
				.MinimumLength(UsuarioStaticValidator.SenhaMinLength).WithMessage($"A senha deve ter pelo menos {UsuarioStaticValidator.SenhaMinLength} caracteres.")
				.MaximumLength(UsuarioStaticValidator.SenhaMaxLength).WithMessage($"A senha deve ter no máximo {UsuarioStaticValidator.SenhaMaxLength} caracteres.");
		}
	}
}
