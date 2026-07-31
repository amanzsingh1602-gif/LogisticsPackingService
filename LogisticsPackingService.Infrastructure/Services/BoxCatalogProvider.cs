using LogisticsPackingService.Application.Interfaces;
using LogisticsPackingService.Domain.Entities;
using LogisticsPackingService.Domain.ValueObjects;
using LogisticsPackingService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace LogisticsPackingService.Infrastructure.Services;

public sealed class BoxCatalogProvider : IBoxCatalogProvider
{
    private readonly IReadOnlyList<Box> _boxes;

    public BoxCatalogProvider(IOptions<BoxCatalogOptions> options)
    {
        _boxes = options.Value.Boxes
            .Select(box => new Box
            {
                Name = box.Name,
                Dimensions = new Dimensions(
                    box.Width,
                    box.Height,
                    box.Length),
                MaxWeight = box.MaxWeight
            })
            .ToList();
    }

    public IReadOnlyList<Box> GetBoxes() => _boxes;
}