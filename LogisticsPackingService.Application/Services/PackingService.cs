using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Domain.Entities;

namespace LogisticsPackingService.Application.Services;

public sealed class PackingService : IPackingService
{
    private readonly IBoxCatalogProvider _boxCatalogProvider;
    private readonly IPackingAlgorithm _packingAlgorithm;

    public PackingService(
        IBoxCatalogProvider boxCatalogProvider,
        IPackingAlgorithm packingAlgorithm)
    {
        _boxCatalogProvider = boxCatalogProvider;
        _packingAlgorithm = packingAlgorithm;
    }

    public PackingResponseDto CalculateBoxes(PackingRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var packages = request.Packages
            .Select(p => new Package
            {
                Id = p.Id,
                Dimensions = new Domain.ValueObjects.Dimensions(
                    p.Width,
                    p.Height,
                    p.Length),
                Weight = p.Weight
            })
            .ToList();

        var availableBoxes = _boxCatalogProvider.GetBoxes();

        var packedBoxes = _packingAlgorithm.Pack(
            packages,
            availableBoxes);

        var packageOrder = request.Packages
    .Select((p, index) => new { p.Id, index })
    .ToDictionary(x => x.Id, x => x.index);

        var response = packedBoxes
            .Select(box => new PackedBoxDto(
                box.Box.Name,
                box.Shelves
                    .SelectMany(s => s.Packages)
                    .Select(p => p.Id)
                    .OrderBy(id => packageOrder[id])
                    .ToList()))
            .ToList();

        return new PackingResponseDto(
            response.Count,
            response);
    }
}
