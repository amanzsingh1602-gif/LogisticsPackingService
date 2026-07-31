using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.Services
{
    public sealed class PackingService : IPackingService
    {
        public Task<PackingResponseDto> CalculateBoxesAsync(
            PackingRequestDto request,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
