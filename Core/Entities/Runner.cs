namespace Core.Entities;

/// <summary>
/// Сущность бегуна, представляющая участника, который может принимать участие в гонках.
/// </summary>
public class Runner
{
    /// <summary>
    /// Идентификатор бегуна.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя бегуна.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Страна, в которой имеет гражданство бегун.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Пол бегуна.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Рейтинг бегуна, по умолчанию равен 0.
    /// </summary>
    public int RatingValue { get; set; } = 0;

    /// <summary>
    /// Список статистик гонок, в которых участвовал бегун.
    /// </summary>
    public List<RaceStatistic> Statistics { get; set; }
}

/// <summary>
/// Перечисление для указания пола бегуна.
/// </summary>
public enum Gender
{
    /// <summary>
    /// Мужской пол.
    /// </summary>
    Male = 1,

    /// <summary>
    /// Женский пол.
    /// </summary>
    Female = 2
}
