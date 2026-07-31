using FluentValidation.TestHelper;
using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Validators;

namespace LogisticsPackingService.Tests.Validators;

public class PackingRequestValidatorTests
{
    private readonly PackingRequestValidator _validator = new();

    [Fact]
    public void Should_NotHaveValidationError_WhenRequestIsValid()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(1, 100, 100, 100, 10)
        ]);
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveValidationError_WhenPackagesCollectionIsEmpty()
    {
        var request = new PackingRequestDto([]);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Packages);
    }

    [Fact]
    public void Should_HaveValidationError_WhenPackageInCollectionIsInvalid()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(1, 0, 100, 100, 10)
        ]);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor("Packages[0].Width");
    }
}
