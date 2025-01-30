using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Runner.Commands
{
    /// <summary>
    /// Команда для удаления бегуна по его ID.
    /// </summary>
    public static class DeleteRunner
    {
        /// <summary>
        /// Запрос для удаления бегуна по его ID.
        /// </summary>
        /// <param name="Id">Идентификатор бегуна, которого нужно удалить.</param>
        public record Command(int Id) : IRequest<Core.Entities.Runner?>;

        /// <summary>
        /// Обработчик команды для удаления бегуна.
        /// Ищет бегуна с указанным ID и удаляет его из базы данных.
        /// </summary>
        public class Handler : IRequestHandler<Command, Core.Entities.Runner?>
        {
            private readonly RunnersContext _context;

            /// <summary>
            /// Конструктор обработчика команды удаления бегуна.
            /// </summary>
            /// <param name="context">Контекст базы данных для работы с сущностью бегуна.</param>
            public Handler(RunnersContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Обработчик команды для удаления бегуна.
            /// Ищет бегуна по указанному ID и удаляет его из базы данных.
            /// </summary>
            /// <param name="request">Запрос с ID бегуна для удаления.</param>
            /// <param name="cancellationToken">Токен отмены операции.</param>
            /// <returns>Удаленного бегуна.</returns>
            /// <exception cref="KeyNotFoundException">Если бегун с указанным ID не найден в базе данных.</exception>
            public async Task<Core.Entities.Runner?> Handle(Command request, CancellationToken cancellationToken)
            {
                var runner = await _context.Runners.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

                if (runner == null)
                {
                    // Если бегун не найден, выбрасываем исключение
                    throw new KeyNotFoundException($"Runner with ID {request.Id} not found.");
                }

                // Удаляем бегуна из базы данных
                _context.Runners.Remove(runner);
                await _context.SaveChangesAsync(cancellationToken);

                return runner;
            }
        }
    }
}



