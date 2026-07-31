using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Infrastructure.Configuration
{
    public sealed class BoxCatalogOptions
    {
        public required List<BoxOption> Boxes { get; init; }
    }

    public sealed class BoxOption
    {
        public required string Name { get; init; }

        public required decimal Width { get; init; }

        public required decimal Height { get; init; }

        public required decimal Length { get; init; }

        public required decimal MaxWeight { get; init; }
    }
}
