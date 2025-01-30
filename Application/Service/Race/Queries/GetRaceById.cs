using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Race.Queries;
    
/// <summary>
/// Запрос для получения информации о гонке по-указанному Id.
/// </summary>
public static class GetRaceById
{
    /// <summary>
    /// Запрос для получения гонки по ID.
    /// </summary>
    /// <param name="Id">Идентификатор гонки для получения данных.</param>
    public record Query(int Id) : IRequest<Core.Entities.Race?>;

    /// <summary>
    /// Обработчик запроса для получения гонки по указанному Id.
    /// </summary>
    private class Handler : IRequestHandler<Query, Core.Entities.Race?>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для извлечения информации о гонке.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обрабатывает запрос для получения гонки по-указанному Id.
        /// </summary>
        /// <param name="request">Запрос с ID гонки.</param>
        /// <param name="cancellationToken">Токен для отмены операции.</param>
        /// <returns>Гонка с указанным Id, если она найдена, или null в случае отсутствия.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если ID меньше или равен нулю.</exception>
        /// <exception cref="KeyNotFoundException">Если гонка с указанным ID не найдена.</exception>
        public async Task<Core.Entities.Race?> Handle(Query request, CancellationToken cancellationToken)
        {
            // Проверка, что Id больше 0
            if (request.Id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Id), "Race Id must be greater than zero.");
            }

            var race = await _context.Races
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            // Проверка, если гонка не найдена
            if (race == null)
            {
                throw new KeyNotFoundException($"No race found with Id {request.Id}");
            }

            return race;
        }
    }
}
