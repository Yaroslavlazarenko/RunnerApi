using Core.DTOs.RaceStatisticDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.RaceStatistic.Queries;

/// <summary>
/// Запрос для получения всех статистик гонки по ее ID с пагинацией.
/// </summary>
public static class GetAllRaceStatisticsByRaceId
{
    /// <summary>
    /// Запрос для получения списка статистик гонки по ID гонки с параметрами пагинации.
    /// </summary>
    /// <param name="GetAllRaceStatisticsByRaceIdDto">DTO, содержащий параметры пагинации и ID гонки.</param>
    public record Query(GetAllRaceStatisticsByRaceIdDto GetAllRaceStatisticsByRaceIdDto) : IRequest<IEnumerable<Core.Entities.RaceStatistic>>;
    
    /// <summary>
    /// Обработчик запроса для получения всех статистик гонки по ее ID с пагинацией.
    /// </summary>
    public class Handler : IRequestHandler<Query, IEnumerable<Core.Entities.RaceStatistic>>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для работы с статистиками гонок.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обработчик запроса, который извлекает статистики для гонки с указанным ID, применяя пагинацию.
        /// </summary>
        /// <param name="request">Запрос с параметрами пагинации и ID гонки.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список статистик гонки для указанного ID.</returns>
        public async Task<IEnumerable<Core.Entities.RaceStatistic>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.RaceStatistics
                .Where(r => r.RaceId == request.GetAllRaceStatisticsByRaceIdDto.RaceId)
                .Skip((request.GetAllRaceStatisticsByRaceIdDto.PageNumber - 1) * request.GetAllRaceStatisticsByRaceIdDto.PageSize)
                .Take(request.GetAllRaceStatisticsByRaceIdDto.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
