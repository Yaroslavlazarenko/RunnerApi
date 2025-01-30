namespace Core.DTOs.RaceStatisticDtos;

/// <summary>
/// DTO для представления статистики гонки для конкретного бегуна.
/// </summary>
public class RaceStatisticDto
{
    /// <summary>
    /// Идентификатор гонки.
    /// </summary>
    public int RaceId { get; set; }

    /// <summary>
    /// Идентификатор бегуна.
    /// </summary>
    public int RunnerId { get; set; }

    /// <summary>
    /// Время результата бегуна в гонке.
    /// </summary>
    public TimeSpan TimeResult { get; set; }
}
