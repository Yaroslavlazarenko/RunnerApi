namespace Core.Entities;

/// <summary>
/// Сущность статистики гонки, представляющая результаты участника в конкретной гонке.
/// </summary>
public class RaceStatistic
{
    /// <summary>
    /// Идентификатор статистики гонки.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Гонка, к которой относится эта статистика.
    /// </summary>
    public Race Race { get; set; }

    /// <summary>
    /// Бегун, чьи результаты представлены в этой статистике.
    /// </summary>
    public Runner Runner { get; set; }

    /// <summary>
    /// Идентификатор гонки, к которой относится эта статистика.
    /// </summary>
    public int RaceId { get; set; }

    /// <summary>
    /// Идентификатор бегуна, чьи результаты представлены в этой статистике.
    /// </summary>
    public int RunnerId { get; set; }

    /// <summary>
    /// Время, затраченное бегуном на прохождение гонки.
    /// </summary>
    public TimeSpan TimeResult { get; set; }
}
