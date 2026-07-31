using FluentValidation.TestHelper;
using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Validators;

namespace LogisticsPackingService.Tests.Validators;

public class PackageDtoValidatorTests
{
    private readonly PackageDtoValidator _validator = new();

    [Fact]
    public void Should_NotHaveValidationError_WhenPackageIsValid()
    {
        var package = new PackageDto(1, 100, 100, 100, 10);

        var result = _validator.TestValidate(package);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveValidationError_WhenIdIsLessThanOrEqualToZero()
    {
        var package = new PackageDto(0, 100, 100, 100, 10);

        var result = _validator.TestValidate(package);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_HaveValidationError_WhenWidthIsLessThanOrEqualToZero()
    {
        var package = new PackageDto(1, 0, 100, 100, 10);

        var result = _validator.TestValidate(package);

        result.ShouldHaveValidationErrorFor(x => x.Width);
    }

    [Fact]
    public void Should_HaveValidationError_WhenHeightIsLessThanOrEqualToZero()
    {
        var package = new PackageDto(1, 100, 0, 100, 10);

        var result = _validator.TestValidate(package);

        result.ShouldHaveValidationErrorFor(x => x.Height);
    }

    [Fact]
    public void Should_HaveValidationError_WhenLengthIsLessThanOrEqualToZero()
    {
        var package = new PackageDto(1, 100, 100, 0, 10);

        var result = _validator.TestValidate(package);

        result.ShouldHaveValidationErrorFor(x => x.Length);
    }

    [Fact]
    public void Should_HaveValidationError_WhenWeightIsLessThanOrEqualToZero()
    {
        var package = new PackageDto(1, 100, 100, 100, 0);

        var result = _validator.TestValidate(package);

        result.ShouldHaveValidationErrorFor(x => x.Weight);
    }
}
