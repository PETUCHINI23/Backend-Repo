using Backend.DTOs;
using Backend.Repository;
using FluentValidation;

namespace Backend.Validators
{
    public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator(IBrandRepository brandRepository)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id es obligatorio");
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 20).WithMessage("El nombre debe medir de 2 a 20 caracteres")
                .MustAsync(async (brand, name, cancellation) => !await brandRepository.ExistsByName(name, brand.Id))
                .WithMessage("Ya existe una marca con ese nombre");
        }
    }
}
