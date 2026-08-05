using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.DTOs
{
    public sealed record PackingResponseDto(
        int BoxesRequired,
        IReadOnlyList<PackedBoxDto> Boxes);
}
