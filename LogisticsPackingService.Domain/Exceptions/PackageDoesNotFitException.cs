using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Exceptions
{
    public class PackageDoesNotFitException : Exception
    {
        public PackageDoesNotFitException(string message)
            : base(message)
        {
        }
    }
}
