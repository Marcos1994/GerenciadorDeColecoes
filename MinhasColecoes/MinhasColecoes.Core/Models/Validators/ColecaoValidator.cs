using FluentValidation;
using MinhasColecoes.Aplicacao.Models.Input;
using MinhasColecoes.Aplicacao.Models.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Aplicacao.Models.Validators
{
	static class ColecaoStaticValidator
	{
		public static int NomeMinLength { get; private set; } = 3;
		public static int NomeMaxLength { get; private set; } = 100;
		public static int DescricaoMaxLength { get; private set; } = 250;
	}

	public class ColecaoInputValidator : AbstractValidator<ColecaoInputModel>
	{
		public ColecaoInputValidator()
		{
			RuleFor(c => c.Nome)
				.NotEmpty().WithMessage($"O nome da coleção Deve ter entre {ColecaoStaticValidator.NomeMinLength} e {ColecaoStaticValidator.NomeMaxLength} caracteres.")
				.MinimumLength(ColecaoStaticValidator.NomeMinLength).WithMessage($"O nome da coleção deve ter pelo menos {ColecaoStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(ColecaoStaticValidator.NomeMaxLength).WithMessage($"O nome da coleção deve ter no máximo {ColecaoStaticValidator.NomeMaxLength} caracteres.");
			RuleFor(c => c.Descricao)
				.MaximumLength(ColecaoStaticValidator.DescricaoMaxLength).WithMessage($"A descrição da coleção deve ter no máximo {ColecaoStaticValidator.DescricaoMaxLength} caracteres.");
		}
	}

	public class ColecaoUpdateValidator : AbstractValidator<ColecaoUpdateModel>
	{
		public ColecaoUpdateValidator()
		{
			RuleFor(c => c.Nome)
				.NotEmpty().WithMessage($"O nome da coleção Deve ter entre {ColecaoStaticValidator.NomeMinLength} e {ColecaoStaticValidator.NomeMaxLength} caracteres.")
				.MinimumLength(ColecaoStaticValidator.NomeMinLength).WithMessage($"O nome da coleção deve ter pelo menos {ColecaoStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(ColecaoStaticValidator.NomeMaxLength).WithMessage($"O nome da coleção deve ter no máximo {ColecaoStaticValidator.NomeMaxLength} caracteres.");
			RuleFor(c => c.Descricao)
				.MaximumLength(ColecaoStaticValidator.DescricaoMaxLength).WithMessage($"A descrição da coleção deve ter no máximo {ColecaoStaticValidator.DescricaoMaxLength} caracteres.");
		}
	}
}
