using FluentValidation;
using LogisticsPackingService.Application.DTOs;

namespace LogisticsPackingService.Application.Validators;

public sealed class PackingRequestValidator
    : AbstractValidator<PackingRequestDto>
{
    public PackingRequestValidator()
    {
        RuleFor(x => x.Packages)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one package is required");

        RuleForEach(x => x.Packages)
            .SetValidator(new PackageDtoValidator());
    }
}