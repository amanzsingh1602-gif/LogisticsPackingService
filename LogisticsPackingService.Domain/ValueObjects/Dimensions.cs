using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.ValueObjects
{
    public readonly record struct Dimensions(
        decimal Width,
        decimal Height,
        decimal Length);
}
