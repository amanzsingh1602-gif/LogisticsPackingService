using LogisticsPackingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.Interfaces
{
    public interface IPackingAlgorithm
    {
        IReadOnlyList<PackedBox> Pack(
            IReadOnlyList<Package> packages,
            IReadOnlyList<Box> availableBoxes);
    }
}
