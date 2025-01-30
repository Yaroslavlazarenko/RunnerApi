using Core.DTOs.RunnerDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Runner.Queries
{
    /// <summary>
    /// Запрос для получения страницы бегунов с учетом пагинации и сортировки.
    /// </summary>
    public static class GetRunnersPage
    {
        /// <summary>
        /// Результат пагинированного запроса, содержащий элементы и общее количество.
        /// </summary>
        /// <typeparam name="T">Тип элементов в пагинированном результате.</typeparam>
        public record PagedResult<T>
        {
            /// <summary>
            /// Коллекция элементов, возвращаемых на текущей странице.
            /// </summary>
            public IEnumerable<T> Items { get; init; } = [];

            /// <summary>
            /// Общее количество элементов для всех страниц.
            /// </summary>
            public int TotalCount { get; set; }
        }

        /// <summary>
        /// Запрос для получения страницы бегунов с пагинацией.
        /// </summary>
        /// <param name="GetRunnersPageDto">Объект с параметрами пагинации и сортировки.</param>
        public record Query(GetRunnersPageDto GetRunnersPageDto) : IRequest<PagedResult<Core.Entities.Runner>>;

        /// <summary>
        /// Обработчик запроса для получения страницы бегунов с пагинацией и сортировкой.
        /// </summary>
        public class Handler : IRequestHandler<Query, PagedResult<Core.Entities.Runner>>
        {
            private readonly RunnersContext _context;

            /// <summary>
            /// Конструктор обработчика запроса.
            /// </summary>
            /// <param name="context">Контекст базы данных для работы с сущностью бегунов.</param>
            public Handler(RunnersContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Обработчик запроса для получения страницы бегунов с пагинацией и сортировкой.
            /// </summary>
            /// <param name="request">Запрос с параметрами пагинации и сортировки.</param>
            /// <param name="cancellationToken">Токен отмены операции.</param>
            /// <returns>Пагинированный результат с бегунами и общим количеством.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Если параметры пагинации некорректны.</exception>
            /// <exception cref="ArgumentException">Если количество элементов сортировки не совпадает с направлением сортировки.</exception>
            public async Task<PagedResult<Core.Entities.Runner>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Проверка на валидность параметров пагинации
                if (request.GetRunnersPageDto.PageNumber < 1 || request.GetRunnersPageDto.PageSize <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(request.GetRunnersPageDto.PageNumber), "Invalid pagination parameters.");
                }

                var query = _context.Runners.AsQueryable();

                // Проверка и применение сортировки
                if (request.GetRunnersPageDto.SortBy is { Count: > 0 })
                {
                    if (request.GetRunnersPageDto.SortDirection != null && request.GetRunnersPageDto.SortBy.Count != request.GetRunnersPageDto.SortDirection.Count)
                    {
                        throw new ArgumentException("SortBy and SortDirection lists must have the same length.");
                    }

                    for (var i = 0; i < request.GetRunnersPageDto.SortBy.Count; i++)
                    {
                        var sortBy = request.GetRunnersPageDto.SortBy[i];
                        if (request.GetRunnersPageDto.SortDirection == null) continue;
                        var sortDirection = request.GetRunnersPageDto.SortDirection[i];

                        query = sortBy switch
                        {
                            RunnerSortBy.Name => sortDirection == RunnerSortDirection.Asc ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name),
                            RunnerSortBy.Country => sortDirection == RunnerSortDirection.Asc ? query.OrderBy(r => r.Country) : query.OrderByDescending(r => r.Country),
                            RunnerSortBy.Gender => sortDirection == RunnerSortDirection.Asc ? query.OrderBy(r => r.Gender) : query.OrderByDescending(r => r.Gender),
                            RunnerSortBy.RatingValue => sortDirection == RunnerSortDirection.Asc ? query.OrderBy(r => r.RatingValue) : query.OrderByDescending(r => r.RatingValue),
                            _ => query
                        };
                    }
                }

                // Получаем общее количество бегунов
                var totalCount = await query.CountAsync(cancellationToken);

                // Получаем элементы для текущей страницы
                var items = await query
                    .Skip((request.GetRunnersPageDto.PageNumber - 1) * request.GetRunnersPageDto.PageSize)
                    .Take(request.GetRunnersPageDto.PageSize)
                    .ToListAsync(cancellationToken);

                // Возвращаем пагинированный результат
                return new PagedResult<Core.Entities.Runner> { Items = items, TotalCount = totalCount };
            }
        }
    }
}


