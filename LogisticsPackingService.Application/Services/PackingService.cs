using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.Exceptions;
using LogisticsPackingService.Domain.ValueObjects;

namespace LogisticsPackingService.Application.Services;

public sealed class PackingService : IPackingService
{
    private readonly IBoxCatalogProvider _boxCatalogProvider;

    public PackingService(IBoxCatalogProvider boxCatalogProvider)
    {
        _boxCatalogProvider = boxCatalogProvider;
    }

    public PackingResponseDto CalculateBoxes(PackingRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var availableBoxes = _boxCatalogProvider.GetBoxes();

        var sortedPackages = request.Packages
            .OrderByDescending(p => CalculateVolume(
                new Dimensions(p.Width, p.Height, p.Length)));

        var boxesRequired = 0;

        foreach (var packageDto in sortedPackages)
        {
            var package = new Package
            {
                Id = packageDto.Id,
                Dimensions = new Dimensions(
                    packageDto.Width,
                    packageDto.Height,
                    packageDto.Length),
                Weight = packageDto.Weight
            };

            var box = FindSmallestSuitableBox(package, availableBoxes);

            if (box is null)
            {
                throw new PackageDoesNotFitException(
                    $"Package {package.Id} cannot fit into any available shipping box.");
            }

            boxesRequired++;
        }

        return new PackingResponseDto(boxesRequired);
    }

    private static Box? FindSmallestSuitableBox(
        Package package,
        IReadOnlyList<Box> availableBoxes)
    {
        return availableBoxes
            .Where(box => CanFit(package, box))
            .OrderBy(box => CalculateVolume(box.Dimensions))
            .FirstOrDefault();
    }

    private static bool CanFit(
        Package package,
        Box box)
    {
        if (package.Weight > box.MaxWeight)
        {
            return false;
        }

        return CanFitWithRotation(package.Dimensions, box.Dimensions);
    }

    private static bool CanFitWithRotation(
        Dimensions package,
        Dimensions box)
    {
        var orientations = new[]
        {
            (package.Width, package.Height, package.Length),
            (package.Width, package.Length, package.Height),
            (package.Height, package.Width, package.Length),
            (package.Height, package.Length, package.Width),
            (package.Length, package.Width, package.Height),
            (package.Length, package.Height, package.Width)
        };

        return orientations.Any(o =>
            o.Item1 <= box.Width &&
            o.Item2 <= box.Height &&
            o.Item3 <= box.Length);
    }

    private static decimal CalculateVolume(Dimensions dimensions)
    {
        return dimensions.Width *
               dimensions.Height *
               dimensions.Length;
    }
}
