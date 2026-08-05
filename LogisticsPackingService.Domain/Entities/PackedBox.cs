using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public sealed class PackedBox
    {
        public required Box Box { get; init; }

        public decimal UsedWeight { get; set; }

        public decimal UsedHeight { get; set; }

        public List<Shelf> Shelves { get; } = new();
    }
}
