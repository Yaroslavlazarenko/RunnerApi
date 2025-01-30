namespace Core.DTOs.RaceDtos;

/// <summary>
/// DTO для получения страницы с гонками с параметрами пагинации и сортировки.
/// </summary>
public class GetRacesPageDto
{
    /// <summary>
    /// Номер страницы для пагинации.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Количество элементов на странице.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Список полей для сортировки.
    /// </summary>
    public List<RaceSortBy>? SortBy { get; set; }

    /// <summary>
    /// Список направлений сортировки (по возрастанию или убыванию).
    /// </summary>
    public List<RaceSortDirection>? SortDirection { get; set; }
}

/// <summary>
/// Перечисление для указания полей, по которым можно сортировать гонки.
/// </summary>
public enum RaceSortBy
{
    /// <summary>
    /// Сортировка по полу гонки.
    /// </summary>
    RaceGender,

    /// <summary>
    /// Сортировка по дате гонки.
    /// </summary>
    Date,

    /// <summary>
    /// Сортировка по длине гонки.
    /// </summary>
    RaceLength,

    /// <summary>
    /// Сортировка по стране гонки.
    /// </summary>
    Country,

    /// <summary>
    /// Сортировка по названию гонки.
    /// </summary>
    Name
}

/// <summary>
/// Перечисление для указания направления сортировки (по возрастанию или убыванию).
/// </summary>
public enum RaceSortDirection
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