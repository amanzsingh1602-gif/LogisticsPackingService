using FluentValidation;
using LogisticsPackingService.Application.DTOs;

namespace LogisticsPackingService.Application.Validators;

public sealed class PackageDtoValidator : AbstractValidator<PackageDto>
{
    public PackageDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Width)
            .GreaterThan(0);

        RuleFor(x => x.Height)
            .GreaterThan(0);

        RuleFor(x => x.Length)
            .GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThan(0);
    }
}
