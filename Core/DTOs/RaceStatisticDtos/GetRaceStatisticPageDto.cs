namespace Core.DTOs.RaceStatisticDtos;

/// <summary>
/// DTO для получения статистики гонок с пагинацией.
/// </summary>
public class GetRaceStatisticPageDto
{
    /// <summary>
    /// Номер страницы для пагинации.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Размер страницы для пагинации.
    /// </summary>
    public int PageSize { get; set; }
}
