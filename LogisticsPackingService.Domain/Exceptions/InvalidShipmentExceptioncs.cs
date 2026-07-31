using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Exceptions
{
    public class InvalidShipmentException : Exception
    {
        public InvalidShipmentException(string message)
            : base(message)
        {
        }
    }
}
