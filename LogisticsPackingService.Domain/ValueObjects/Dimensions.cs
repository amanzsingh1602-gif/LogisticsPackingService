using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.ValueObjects
{
    public record Dimensions(
        decimal Length,
        decimal Width,
        decimal Height);
}
