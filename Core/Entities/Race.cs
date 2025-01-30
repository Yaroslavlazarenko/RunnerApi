namespace Core.Entities;

/// <summary>
/// Сущность гонки, представляющая основные данные о гонке.
/// </summary>
public class Race
{
    /// <summary>
    /// Идентификатор гонки.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название гонки.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Страна, в которой проводится гонка.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Дистанция гонки (в метрах).
    /// </summary>
    public int RaceLength { get; set; }

    /// <summary>
    /// Дата проведения гонки.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Пол участников гонки (мужской, женский или общий).
    /// </summary>
    public RaceGender RaceGender { get; set; }

    /// <summary>
    /// Список статистики, связанной с данной гонкой.
    /// </summary>
    public List<RaceStatistic> Statistics { get; set; }
}

/// <summary>
/// Перечисление для указания пола участников гонки.
/// </summary>
public enum RaceGender
{
    /// <summary>
    /// Мужской пол.
    /// </summary>
    Male = 1,

    /// <summary>
    /// Женский пол.
    /// </summary>
    Female = 2,

    /// <summary>
    /// Общий пол (для любых участников).
    /// </summary>
    General = 3
}