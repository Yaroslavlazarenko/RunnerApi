using Core.DTOs.RaceDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Race.Queries;

/// <summary>
/// Запрос для получения списка гонок с пагинацией и возможностью сортировки.
/// </summary>
public static class GetRacePage
{
    /// <summary>
    /// Запрос для получения страницы гонок с указанными параметрами пагинации и сортировки.
    /// </summary>
    /// <param name="GetRacesPageDto">Параметры для пагинации и сортировки.</param>
    public record Query(GetRacesPageDto GetRacesPageDto) : IRequest<IEnumerable<Core.Entities.Race>>;

    /// <summary>
    /// Обработчик запроса для извлечения гонок с учетом пагинации и сортировки.
    /// </summary>
    private class Handler : IRequestHandler<Query, IEnumerable<Core.Entities.Race>>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для извлечения гонок.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обрабатывает запрос для извлечения списка гонок с учетом пагинации и сортировки.
        /// </summary>
        /// <param name="request">Запрос с параметрами пагинации и сортировки.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список гонок с учетом пагинации и сортировки.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если параметры пагинации некорректны (например, номер страницы меньше 1 или размер страницы меньше или равен нулю).</exception>
        /// <exception cref="ArgumentException">Если длина списков сортировки не совпадает.</exception>
        /// <exception cref="KeyNotFoundException">Если не найдены гонки по указанным параметрам.</exception>
        public async Task<IEnumerable<Core.Entities.Race>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Валидация параметров пагинации
            if (request.GetRacesPageDto.PageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(request.GetRacesPageDto.PageNumber), "Page number must be greater than or equal to 1.");
            }

            if (request.GetRacesPageDto.PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.GetRacesPageDto.PageSize), "Page size must be greater than 0.");
            }

            // Валидация длины сортировки
            if (request.GetRacesPageDto.SortBy?.Count > 0 && request.GetRacesPageDto.SortBy.Count != request.GetRacesPageDto.SortDirection?.Count)
            {
                throw new ArgumentException("SortBy and SortDirection lists must have the same length.");
            }

            var query = _context.Races.AsQueryable();
            
            // Проверка на пустые сортировки, и если пусто - сортируем по имени
            if (request.GetRacesPageDto.SortBy?.Count > 0)
            {
                for (int i = 0; i < request.GetRacesPageDto.SortBy.Count; i++)
                {
                    var sortBy = request.GetRacesPageDto.SortBy[i];
                    var sortDirection = request.GetRacesPageDto.SortDirection[i];

                    query = sortBy switch
                    {
                        RaceSortBy.Name => sortDirection == RaceSortDirection.Asc ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name),
                        RaceSortBy.Country => sortDirection == RaceSortDirection.Asc ? query.OrderBy(r => r.Country) : query.OrderByDescending(r => r.Country),
                        RaceSortBy.RaceGender => sortDirection == RaceSortDirection.Asc ? query.OrderBy(r => r.RaceGender) : query.OrderByDescending(r => r.RaceGender),
                        RaceSortBy.Date => sortDirection == RaceSortDirection.Asc ? query.OrderBy(r => r.Date) : query.OrderByDescending(r => r.Date),
                        RaceSortBy.RaceLength => sortDirection == RaceSortDirection.Asc ? query.OrderBy(r => r.RaceLength) : query.OrderByDescending(r => r.RaceLength),
                        _ => query
                    };
                }
            }
            else
            {
                // Если сортировки нет, по умолчанию сортируем по имени
                query = query.OrderBy(r => r.Name);
            }

            // Получаем страницы с указанной пагинацией
            var races = await query
                .Skip((request.GetRacesPageDto.PageNumber - 1) * request.GetRacesPageDto.PageSize)
                .Take(request.GetRacesPageDto.PageSize)
                .ToListAsync(cancellationToken);

            // Проверяем, есть ли результаты
            if (races == null || !races.Any())
            {
                throw new KeyNotFoundException("No races found for the given parameters.");
            }

            return races;
        }
    }
}
