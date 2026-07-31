using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Enums
{
    public enum ShipmentStatus
    {
        Pending = 1,
        Packed,
        ReadyForDispatch,
        InTransit,
        Delivered,
        Cancelled
    }
}
