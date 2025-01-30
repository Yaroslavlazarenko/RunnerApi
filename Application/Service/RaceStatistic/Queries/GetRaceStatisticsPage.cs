using Core.DTOs.RaceStatisticDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.RaceStatistic.Queries;

/// <summary>
/// Запрос для получения статистики гонок с пагинацией.
/// </summary>
public static class GetRaceStatisticsPage
{
    /// <summary>
    /// Запрос для получения страницы статистики гонок с указанными параметрами пагинации.
    /// </summary>
    /// <param name="GetRaceStatisticPageDto">Объект DTO с параметрами пагинации для получения статистики гонок.</param>
    public record Query(GetRaceStatisticPageDto GetRaceStatisticPageDto) : IRequest<IEnumerable<Core.Entities.RaceStatistic>>;

    /// <summary>
    /// Обработчик запроса для извлечения статистики гонок с пагинацией.
    /// </summary>
    public class Handler : IRequestHandler<Query, IEnumerable<Core.Entities.RaceStatistic>>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для работы со статистиками гонок.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обработчик запроса, который извлекает статистику гонок с учетом пагинации.
        /// </summary>
        /// <param name="request">Запрос с параметрами пагинации для получения статистики гонок.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список статистики гонок для указанной страницы и размера страницы.</returns>
        public async Task<IEnumerable<Core.Entities.RaceStatistic>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.RaceStatistics
                .Skip((request.GetRaceStatisticPageDto.PageNumber - 1) * request.GetRaceStatisticPageDto.PageSize)
                .Take(request.GetRaceStatisticPageDto.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
