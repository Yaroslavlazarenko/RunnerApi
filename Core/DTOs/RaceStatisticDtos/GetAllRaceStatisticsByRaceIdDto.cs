namespace Core.DTOs.RaceStatisticDtos;

/// <summary>
/// DTO для получения статистики гонок по ID гонки.
/// </summary>
public class GetAllRaceStatisticsByRaceIdDto
{
    /// <summary>
    /// Идентификатор гонки, для которой требуется статистика.
    /// </summary>
    public int RaceId { get; set; }

    /// <summary>
    /// Номер страницы для пагинации.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Размер страницы для пагинации.
    /// </summary>
    public int PageSize { get; set; }
}
