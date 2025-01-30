namespace Core.DTOs.RunnerDtos;

/// <summary>
/// DTO для обновления данных бегуна.
/// </summary>
public class UpdateRunnerDto
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
    /// Страна бегуна.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Пол бегуна.
    /// </summary>
    public Gender Gender { get; set; }
}
