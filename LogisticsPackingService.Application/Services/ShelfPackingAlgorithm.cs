using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.Exceptions;
using LogisticsPackingService.Domain.ValueObjects;

namespace LogisticsPackingService.Application.Services;

public sealed class ShelfPackingAlgorithm : IPackingAlgorithm
{
    public IReadOnlyList<PackedBox> Pack(
        IReadOnlyList<Package> packages,
        IReadOnlyList<Box> availableBoxes)
    {
        var sortedPackages = packages
            .OrderByDescending(p => CalculateVolume(p.Dimensions))
            .ToList();

        var packedBoxes = new List<PackedBox>();

        foreach (var package in sortedPackages)
        {
            var packed = false;

            foreach (var packedBox in packedBoxes)
            {
                if (TryPlaceInExistingBox(package, packedBox))
                {
                    packed = true;
                    break;
                }
            }

            if (packed)
                continue;

            var box = FindSmallestSuitableBox(package, availableBoxes);

            if (box is null)
            {
                throw new PackageDoesNotFitException(
                    $"Package {package.Id} cannot fit into any available shipping box.");
            }

            packedBoxes.Add(CreatePackedBox(package, box));
        }

        return packedBoxes;
    }

    private PackedBox CreatePackedBox(
        Package package,
        Box box)
    {
        var packedBox = new PackedBox
        {
            Box = box,
            UsedWeight = package.Weight,
            UsedHeight = package.Dimensions.Height
        };

        var shelf = new Shelf
        {
            Height = package.Dimensions.Height,
            RemainingLength =
                box.Dimensions.Length - package.Dimensions.Length
        };

        shelf.Packages.Add(package);

        packedBox.Shelves.Add(shelf);

        return packedBox;
    }

    private bool TryPlaceInExistingBox(
     Package package,
     PackedBox packedBox)
    {
        if (packedBox.UsedWeight + package.Weight >
            packedBox.Box.MaxWeight)
        {
            return false;
        }

        Shelf? bestShelf = null;
        Dimensions? bestOrientation = null;
        decimal smallestRemainingLength = decimal.MaxValue;

        foreach (var shelf in packedBox.Shelves)
        {
            var orientation = GetBestOrientation(
                package,
                packedBox.Box,
                shelf.Height,
                shelf.RemainingLength);

            if (!orientation.HasValue)
                continue;

            var selectedOrientation = orientation.Value;

            var remaining =
                shelf.RemainingLength - selectedOrientation.Length;

            if (remaining < smallestRemainingLength)
            {
                smallestRemainingLength = remaining;
                bestShelf = shelf;
                bestOrientation = selectedOrientation;
            }
        }

        if (bestShelf is not null && bestOrientation.HasValue)
        {
            return TryPlaceOnShelf(
                package,
                packedBox,
                bestShelf,
                bestOrientation.Value);
        }

        return TryCreateNewShelf(
            package,
            packedBox);
    }


    private bool TryPlaceOnShelf(
    Package package,
    PackedBox packedBox,
    Shelf shelf,Dimensions orientation)
    {

        shelf.Packages.Add(new Package
        {
            Id = package.Id,
            Dimensions = new Dimensions(
                orientation.Width,
                orientation.Height,
                orientation.Length),
            Weight = package.Weight
        });

        shelf.RemainingLength -= orientation.Length;

        packedBox.UsedWeight += package.Weight;

        return true;
    }

    private bool TryCreateNewShelf(
        Package package,
        PackedBox packedBox)
    {
        var orientation = GetBestOrientation(
            package,
            packedBox.Box,
            null,
            packedBox.Box.Dimensions.Length);

        if (orientation is null)
            return false;

        if (packedBox.UsedHeight + orientation.Value.Height >
            packedBox.Box.Dimensions.Height)
        {
            return false;
        }

        var shelf = new Shelf
        {
            Height = orientation.Value.Height,
            RemainingLength =
                packedBox.Box.Dimensions.Length -
                orientation.Value.Length
        };

        shelf.Packages.Add(new Package
        {
            Id = package.Id,
            Dimensions = new Dimensions(
                orientation.Value.Width,
                orientation.Value.Height,
                orientation.Value.Length),
            Weight = package.Weight
        });

        packedBox.Shelves.Add(shelf);

        packedBox.UsedWeight += package.Weight;

        packedBox.UsedHeight += orientation.Value.Height;

        return true;
    }

    private Dimensions? GetBestOrientation(
     Package package,
     Box box,
     decimal? shelfHeight,
     decimal availableLength)
    {
        var orientations = new[]
        {
        new Dimensions(package.Dimensions.Width, package.Dimensions.Height, package.Dimensions.Length),
        new Dimensions(package.Dimensions.Width, package.Dimensions.Length, package.Dimensions.Height),
        new Dimensions(package.Dimensions.Height, package.Dimensions.Width, package.Dimensions.Length),
        new Dimensions(package.Dimensions.Height, package.Dimensions.Length, package.Dimensions.Width),
        new Dimensions(package.Dimensions.Length, package.Dimensions.Width, package.Dimensions.Height),
        new Dimensions(package.Dimensions.Length, package.Dimensions.Height, package.Dimensions.Width)
    };

        Dimensions? bestOrientation = null;
        decimal smallestRemainingLength = decimal.MaxValue;

        foreach (var orientation in orientations)
        {
            if (orientation.Width > box.Dimensions.Width)
                continue;

            if (orientation.Length > box.Dimensions.Length)
                continue;

            if (shelfHeight.HasValue)
            {
                if (orientation.Height > shelfHeight.Value)
                    continue;
            }
            else
            {
                if (orientation.Height > box.Dimensions.Height)
                    continue;
            }

            if (orientation.Length > availableLength)
                continue;

            var remainingLength =
                availableLength - orientation.Length;

            if (remainingLength < smallestRemainingLength)
            {
                smallestRemainingLength = remainingLength;
                bestOrientation = orientation;
            }
        }

        return bestOrientation;
    }

    private Box? FindSmallestSuitableBox(
    Package package,
    IReadOnlyList<Box> availableBoxes)
    {
        return availableBoxes
            .Where(box =>
            {
                if (package.Weight > box.MaxWeight)
                    return false;

                return GetBestOrientation(package, box, null,box.Dimensions.Length) != null;
            })
            .OrderBy(box =>
                CalculateVolume(box.Dimensions))
            .FirstOrDefault();
    }

    private static decimal CalculateVolume(
        Dimensions dimensions)
    {
        return dimensions.Width *
               dimensions.Height *
               dimensions.Length;
    }

}


