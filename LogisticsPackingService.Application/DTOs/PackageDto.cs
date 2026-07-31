using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.DTOs
{
    public sealed record PackageDto(
        int Id,
        decimal Width,
        decimal Height,
        decimal Length,
        decimal Weight);
}
