using System;

namespace SantiyeAPI.Services;
using SantiyeAPI.DTOs;

public interface IIsciService
{
    Task<PagedResult<IsciListDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IsciDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IsciDetailDto> CreateAsync(IsciCreateDto createDto, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, IsciUpdateDto updateDto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
