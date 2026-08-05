using FluentAssertions;
using LogisticsPackingService.Application.Services;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.Exceptions;
using LogisticsPackingService.Domain.ValueObjects;

namespace LogisticsPackingService.Tests.Services;

public class ShelfPackingAlgorithmTests
{
    [Fact]
    public void Pack_Should_PlaceMultiplePackages_InSameBox()
    {
        var algorithm = new ShelfPackingAlgorithm();

        var boxes = new List<Box>
{
    new Box
    {
        Name = "A",
        Dimensions = new Dimensions(150, 150, 150),
        MaxWeight = 1000
    }
};

        var packages = new List<Package>
{
    new Package
    {
        Id = 1,
        Dimensions = new Dimensions(50, 50, 50),
        Weight = 100
    },
    new Package
    {
        Id = 2,
        Dimensions = new Dimensions(50, 50, 50),
        Weight = 100
    }
};

        var result = algorithm.Pack(packages, boxes);

        result.Should().HaveCount(1);

        var packedPackages = result[0]
            .Shelves
            .SelectMany(s => s.Packages)
            .ToList();

        packedPackages.Should().HaveCount(2);

        packedPackages.Select(p => p.Id)
            .Should()
            .BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void Pack_Should_CreateNewBox_WhenWeightLimitExceeded()
    {
        var algorithm = new ShelfPackingAlgorithm();

        var boxes = new List<Box>
    {
        new Box
        {
            Name = "A",
            Dimensions = new Dimensions(150, 150, 150),
            MaxWeight = 1000
        }
    };

        var packages = new List<Package>
    {
        new Package
        {
            Id = 1,
            Dimensions = new Dimensions(50, 50, 50),
            Weight = 600
        },
        new Package
        {
            Id = 2,
            Dimensions = new Dimensions(50, 50, 50),
            Weight = 600
        }
    };

        var result = algorithm.Pack(packages, boxes);

        result.Should().HaveCount(2);

        result[0].UsedWeight.Should().Be(600);
        result[1].UsedWeight.Should().Be(600);
    }
    [Fact]
    public void Pack_Should_SelectSmallestSuitableBox()
    {
        var algorithm = new ShelfPackingAlgorithm();

        var boxes = new List<Box>
    {
        new Box
        {
            Name = "A",
            Dimensions = new Dimensions(150,150,150),
            MaxWeight = 1000
        },
        new Box
        {
            Name = "C",
            Dimensions = new Dimensions(300,400,300),
            MaxWeight = 3000
        }
    };

        var packages = new List<Package>
    {
        new Package
        {
            Id = 1,
            Dimensions = new Dimensions(100,100,100),
            Weight = 100
        }
    };

        var result = algorithm.Pack(packages, boxes);

        result.Should().HaveCount(1);

        result[0].Box.Name.Should().Be("A");
    }

    [Fact]
    public void Pack_Should_Throw_WhenPackageCannotFitAnyBox()
    {
        var algorithm = new ShelfPackingAlgorithm();

        var boxes = new List<Box>
    {
        new Box
        {
            Name = "A",
            Dimensions = new Dimensions(150,150,150),
            MaxWeight = 1000
        }
    };

        var packages = new List<Package>
    {
        new Package
        {
            Id = 1,
            Dimensions = new Dimensions(1000,1000,1000),
            Weight = 100
        }
    };

        Action act = () => algorithm.Pack(packages, boxes);

        act.Should()
            .Throw<PackageDoesNotFitException>();
    }

    [Fact]
    public void Pack_Should_RotatePackage_WhenRequired()
    {
        var algorithm = new ShelfPackingAlgorithm();

        var boxes = new List<Box>
    {
        new Box
        {
            Name = "B",
            Dimensions = new Dimensions(100,200,100),
            MaxWeight = 1000
        }
    };

        var packages = new List<Package>
    {
        new Package
        {
            Id = 1,
            Dimensions = new Dimensions(200,100,100),
            Weight = 100
        }
    };

        var result = algorithm.Pack(packages, boxes);

        result.Should().HaveCount(1);

        result[0].Box.Name.Should().Be("B");
    }
}