using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Race.Commands;

/// <summary>
/// Команда для удаления гонки по ее ID.
/// </summary>
public static class DeleteRace
{
    /// <summary>
    /// Запрос для удаления гонки по ID.
    /// </summary>
    /// <param name="Id">Идентификатор гонки, которую нужно удалить.</param>
    public record Command(int Id) : IRequest<Core.Entities.Race>;

    /// <summary>
    /// Обработчик команды для удаления гонки.
    /// </summary>
    private class Handler : IRequestHandler<Command, Core.Entities.Race>
    {
        private readonly RunnersContext _context;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public Handler(RunnersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Обрабатывает команду на удаление гонки.
        /// </summary>
        /// <param name="request">Запрос для удаления гонки.</param>
        /// <param name="cancellationToken">Токен для отмены операции.</param>
        /// <returns>Удаленную гонку.</returns>
        /// <exception cref="KeyNotFoundException">Гонка с указанным ID не найдена.</exception>
        /// <exception cref="InvalidOperationException">Не удалось удалить гонку из-за ошибок в базе данных.</exception>
        public async Task<Core.Entities.Race> Handle(Command request, CancellationToken cancellationToken)
        {
            var race = await _context.Races
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (race == null)
            {
                // Если гонка не найдена, выбрасываем исключение
                throw new KeyNotFoundException($"Race with ID {request.Id} not found.");
            }

            // Если все проверки пройдены, удаляем гонку
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

