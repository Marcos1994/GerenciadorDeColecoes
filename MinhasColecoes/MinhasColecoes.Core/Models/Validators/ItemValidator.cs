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
	static class ItemStaticValidator
	{
		public static int NomeMinLength { get; private set; } = 2;
		public static int NomeMaxLength { get; private set; } = 100;
		public static int CodigoMaxLength { get; private set; } = 25;
		public static int DescricaoMaxLength { get; private set; } = 250;
	}

	public class ItemInputValidator : AbstractValidator<ItemInputModel>
	{
		public ItemInputValidator()
		{
			RuleFor(i => i.Nome)
				.NotEmpty().WithMessage($"O nome do item Deve ter entre {ItemStaticValidator.NomeMinLength} e {ItemStaticValidator.NomeMaxLength} caracteres.")
				.MinimumLength(ItemStaticValidator.NomeMinLength).WithMessage($"O nome do item deve ter pelo menos {ItemStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(ItemStaticValidator.NomeMaxLength).WithMessage($"O nome do item deve ter no máximo {ItemStaticValidator.NomeMaxLength} caracteres.");
			RuleFor(i => i.Codigo)
				.MaximumLength(ItemStaticValidator.DescricaoMaxLength).WithMessage($"O código do item deve ter no máximo {ItemStaticValidator.CodigoMaxLength } caracteres.");
			RuleFor(i => i.Descricao)
				.MaximumLength(ItemStaticValidator.DescricaoMaxLength).WithMessage($"A descrição do item deve ter no máximo {ItemStaticValidator.DescricaoMaxLength} caracteres.");
		}
	}

	public class ItemUpdateValidator : AbstractValidator<ItemUpdateModel>
	{
		public ItemUpdateValidator()
		{
			RuleFor(i => i.Nome)
				.NotEmpty().WithMessage($"O nome do item Deve ter entre {ItemStaticValidator.NomeMinLength} e {ItemStaticValidator.NomeMaxLength} caracteres.")
				.MinimumLength(ItemStaticValidator.NomeMinLength).WithMessage($"O nome do item deve ter pelo menos {ItemStaticValidator.NomeMinLength} caracteres.")
				.MaximumLength(ItemStaticValidator.NomeMaxLength).WithMessage($"O nome do item deve ter no máximo {ItemStaticValidator.NomeMaxLength} caracteres.");
			RuleFor(i => i.Codigo)
				.MaximumLength(ItemStaticValidator.DescricaoMaxLength).WithMessage($"O código do item deve ter no máximo {ItemStaticValidator.CodigoMaxLength } caracteres.");
			RuleFor(i => i.Descricao)
				.MaximumLength(ItemStaticValidator.DescricaoMaxLength).WithMessage($"A descrição do item deve ter no máximo {ItemStaticValidator.DescricaoMaxLength} caracteres.");
		}
	}
}
