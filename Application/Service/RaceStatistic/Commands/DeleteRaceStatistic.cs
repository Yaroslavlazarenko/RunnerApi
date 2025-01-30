using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.RaceStatistic.Commands;

/// <summary>
/// Команда для удаления статистики гонки по ID гонки.
/// </summary>
public static class DeleteRaceStatistic
{
    /// <summary>
    /// Запрос для удаления статистики гонки по ID гонки.
    /// </summary>
    /// <param name="Id">ID гонки, для которой удаляется статистика.</param>
    public record Command(int Id) : IRequest<Core.Entities.Race>;

    /// <summary>
    /// Обработчик команды для удаления статистики гонки по ID.
    /// </summary>
    private class Handler : IRequestHandler<Command, Core.Entities.Race>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для работы с гонками и статистикой.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обработчик запроса для удаления статистики гонки.
        /// Проверяет, существует ли гонка, есть ли связанные статистики, и удаляет гонку и ее статистики.
        /// </summary>
        /// <param name="request">Запрос с ID гонки для удаления.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Удаленную гонку.</returns>
        /// <exception cref="KeyNotFoundException">Если гонка с указанным ID не найдена.</exception>
        /// <exception cref="InvalidOperationException">Если гонка имеет связанные статистики, или если не удалось удалить гонку из-за ограничений базы данных.</exception>
        public async Task<Core.Entities.Race> Handle(Command request, CancellationToken cancellationToken)
        {
            var race = await _context.Races
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            // Проверка, существует ли гонка с указанным ID
            if (race == null)
            {
                throw new KeyNotFoundException($"Race with ID {request.Id} not found.");
            }

            // Удаляем гонку
            _context.Races.Remove(race);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                // Обрабатываем ошибки при обновлении базы данных, например, связанные с внешними ключами
                throw new InvalidOperationException("Failed to delete race due to database constraints.", ex);
            }

            return race;
        }
    }
}




