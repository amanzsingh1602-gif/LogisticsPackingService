using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.DTOs
{
    public sealed record PackingRequestDto(
        IReadOnlyList<PackageDto> Packages);
}
