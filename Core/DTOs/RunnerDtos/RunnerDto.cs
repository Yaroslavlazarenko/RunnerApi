namespace Core.DTOs.RunnerDtos;

/// <summary>
/// DTO для представления данных бегуна.
/// </summary>
public class RunnerDto
{
    /// <summary>
    /// Имя бегуна.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Страна бегуна.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Пол бегуна.
    /// </summary>
    public Gender Gender { get; set; }
}

/// <summary>
/// Перечисление для представления пола бегуна.
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
