using Core.Entities;

namespace Core.DTOs.RaceDtos;

/// <summary>
/// DTO для гонки, содержащий основные параметры гонки.
/// </summary>
public class RaceDto
{
    /// <summary>
    /// Пол гонки (мужской или женский).
    /// </summary>
    public RaceGender RaceGender { get; set; }

    /// <summary>
    /// Дата проведения гонки.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Длина гонки в метрах.
    /// </summary>
    public int RaceLength { get; set; }

    /// <summary>
    /// Страна проведения гонки.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Название гонки.
    /// </summary>
    public string Name { get; set; }
}
