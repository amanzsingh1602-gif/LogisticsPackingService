using FluentAssertions;
using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Application.Services;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.Exceptions;
using LogisticsPackingService.Domain.ValueObjects;
using Moq;

namespace LogisticsPackingService.Tests.Services;

public class PackingServiceTests
{
    private readonly Mock<IBoxCatalogProvider> _boxCatalogProviderMock;
    private readonly PackingService _packingService;

    public PackingServiceTests()
    {
        _boxCatalogProviderMock = new Mock<IBoxCatalogProvider>();

        _boxCatalogProviderMock
            .Setup(x => x.GetBoxes())
            .Returns(GetAvailableBoxes());

        _packingService = new PackingService(_boxCatalogProviderMock.Object);
    }

    private static IReadOnlyList<Box> GetAvailableBoxes()
    {
        return
        [
            new Box
            {
                Name = "A",
                Dimensions = new Dimensions(150,150,150),
                MaxWeight = 1000
            },
            new Box
            {
                Name = "B",
                Dimensions = new Dimensions(100,200,100),
                MaxWeight = 1000
            },
            new Box
            {
                Name = "C",
                Dimensions = new Dimensions(300,400,300),
                MaxWeight = 3000
            }
        ];
    }

    [Fact]
    public void CalculateBoxes_ShouldReturnOne_WhenSinglePackageFits()
    {

        var request = new PackingRequestDto(
        [
            new PackageDto(
            1,
            10,
            10,
            10,
            5)
        ]);

        var result = _packingService.CalculateBoxes(request);

        result.BoxesRequired.Should().Be(1);
    }

    [Fact]
    public void CalculateBoxes_ShouldReturnThree_WhenThreePackagesFit()
    {
        var request = new PackingRequestDto(
        [
        new PackageDto(1,10,10,10,5),
        new PackageDto(2,20,20,20,5),
        new PackageDto(3,30,30,30,5)
        ]);

        var result = _packingService.CalculateBoxes(request);

        result.BoxesRequired.Should().Be(3);
    }

    [Fact]
    public void CalculateBoxes_ShouldThrow_WhenPackageDoesNotFit()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(
            1,
            1000,
            1000,
            1000,
            100)
        ]);

        Action act = () => _packingService.CalculateBoxes(request);

        act.Should()
            .Throw<PackageDoesNotFitException>();
    }

    [Fact]
    public void CalculateBoxes_ShouldThrow_WhenWeightExceedsCapacity()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(
            1,
            100,
            100,
            100,
            5000)
        ]);

        Action act = () => _packingService.CalculateBoxes(request);

        act.Should()
            .Throw<PackageDoesNotFitException>();
    }

    [Fact]
    public void CalculateBoxes_ShouldAllowRotation_WhenPackageFitsAfterRotation()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(
            1,
            200,
            100,
            100,
            100)
        ]);

        var result = _packingService.CalculateBoxes(request);

        result.BoxesRequired.Should().Be(1);
    }
}
