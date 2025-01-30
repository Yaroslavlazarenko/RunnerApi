using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.RaceStatistic.Queries;

/// <summary>
/// Запрос для получения статистики гонки по ее ID.
/// </summary>
public static class GetRaceStatisticById
{
    /// <summary>
    /// Запрос для получения статистики гонки по указанному ID.
    /// </summary>
    /// <param name="Id">ID статистики гонки, которую нужно извлечь.</param>
    public record Query(int Id) : IRequest<Core.Entities.RaceStatistic?>;
    
    /// <summary>
    /// Обработчик запроса для получения статистики гонки по ее ID.
    /// </summary>
    public class Handler : IRequestHandler<Query, Core.Entities.RaceStatistic?>
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
        /// Обработчик запроса, который извлекает статистику гонки по указанному ID.
        /// </summary>
        /// <param name="request">Запрос с ID статистики гонки.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Статистика гонки с указанным ID, если найдена.</returns>
        public async Task<Core.Entities.RaceStatistic?> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.RaceStatistics.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        }
    }
}



