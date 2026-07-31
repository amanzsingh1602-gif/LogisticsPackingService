using LogisticsPackingService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Application.Interfaces
{
    public interface IPackingService
    {
        Task<PackingResponseDto> CalculateBoxesAsync(
            PackingRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
