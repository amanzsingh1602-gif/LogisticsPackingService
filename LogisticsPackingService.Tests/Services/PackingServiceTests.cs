using FluentAssertions;
using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Application.Services;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.ValueObjects;
using Moq;

namespace LogisticsPackingService.Tests.Services;

public class PackingServiceTests
{
    private readonly Mock<IBoxCatalogProvider> _boxCatalogProviderMock;
    private readonly Mock<IPackingAlgorithm> _packingAlgorithmMock;

    private readonly PackingService _packingService;

    public PackingServiceTests()
    {
        _boxCatalogProviderMock = new Mock<IBoxCatalogProvider>();
        _packingAlgorithmMock = new Mock<IPackingAlgorithm>();

        _boxCatalogProviderMock
            .Setup(x => x.GetBoxes())
            .Returns(GetAvailableBoxes());

        _packingService = new PackingService(
            _boxCatalogProviderMock.Object,
            _packingAlgorithmMock.Object);
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
            }
        ];
    }

    [Fact]
    public void CalculateBoxes_Should_MapAlgorithmResponse()
    {
        var request = new PackingRequestDto(
        [
            new PackageDto(1,10,10,10,5)
        ]);

        var packedBox = new PackedBox
        {
            Box = GetAvailableBoxes().First(),
            UsedWeight = 5,
            UsedHeight = 10
        };

        var shelf = new Shelf
        {
            Height = 10,
            Width = 10,
            RemainingLength = 140
        };

        shelf.Packages.Add(new Package
        {
            Id = 1,
            Dimensions = new Dimensions(10, 10, 10),
            Weight = 5
        });

        packedBox.Shelves.Add(shelf);

        _packingAlgorithmMock
            .Setup(x => x.Pack(
                It.IsAny<IReadOnlyList<Package>>(),
                It.IsAny<IReadOnlyList<Box>>()))
            .Returns(new List<PackedBox>
            {
                packedBox
            });

        var result = _packingService.CalculateBoxes(request);

        _packingAlgorithmMock.Verify(
    x => x.Pack(
        It.IsAny<IReadOnlyList<Package>>(),
        It.IsAny<IReadOnlyList<Box>>()),
    Times.Once);

        result.BoxesRequired.Should().Be(1);

        result.Boxes.Should().HaveCount(1);

        result.Boxes[0].BoxName.Should().Be("A");

        result.Boxes[0].PackageIds.Should().ContainSingle()
    .Which.Should().Be(1);
    }
}
