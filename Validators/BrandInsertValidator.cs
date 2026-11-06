using Backend.DTOs;
using Backend.Repository;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandInsertValidator : AbstractValidator<BrandInsertDto>
    {
        public BrandInsertValidator(IBrandRepository brandRepository)
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 20).WithMessage("El nombre debe medir de 2 a 20 caracteres")
                .MustAsync(async (name, cancellation) => !await brandRepository.ExistsByName(name))
                .WithMessage("Ya existe una marca con ese nombre");
        }
    }
}
