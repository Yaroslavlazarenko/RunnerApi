namespace Core.DTOs.RunnerDtos;

/// <summary>
/// DTO для представления параметров пагинации и сортировки списка бегунов.
/// </summary>
public class GetRunnersPageDto
{
    /// <summary>
    /// Номер страницы для пагинации.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Количество элементов на странице для пагинации.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Список полей, по которым нужно выполнить сортировку.
    /// </summary>
    public List<RunnerSortBy>? SortBy { get; set; }

    /// <summary>
    /// Список направлений сортировки, соответствующий полям в <see cref="SortBy"/>.
    /// </summary>
    public List<RunnerSortDirection>? SortDirection { get; set; }
}

/// <summary>
/// Перечисление для доступных полей сортировки списка бегунов.
/// </summary>
public enum RunnerSortBy
{
    /// <summary>
    /// Сортировка по имени бегуна.
    /// </summary>
    Name,

    /// <summary>
    /// Сортировка по стране бегуна.
    /// </summary>
    Country,

    /// <summary>
    /// Сортировка по полу бегуна.
    /// </summary>
    Gender,

    /// <summary>
    /// Сортировка по рейтингу бегуна.
    /// </summary>
    RatingValue
}

/// <summary>
/// Перечисление для направлений сортировки.
/// </summary>
public enum RunnerSortDirection
{
    /// <summary>
    /// Сортировка по возрастанию.
    /// </summary>
    Asc,

    /// <summary>
    /// Сортировка по убыванию.
    /// </summary>
    Desc
}
