using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Runner.Queries
{
    /// <summary>
    /// Запрос для получения бегуна по его ID.
    /// </summary>
    public static class GetRunnerById
    {
        /// <summary>
        /// Запрос для получения бегуна по-указанному ID.
        /// </summary>
        /// <param name="Id">ID бегуна для поиска.</param>
        public record Query(int Id) : IRequest<Core.Entities.Runner?>;

        /// <summary>
        /// Обработчик запроса для получения бегуна по его ID.
        /// Ищет бегуна в базе данных по-указанному ID.
        /// </summary>
        public class Handler : IRequestHandler<Query, Core.Entities.Runner?>
        {
            private readonly RunnersContext _context;

            /// <summary>
            /// Конструктор обработчика запроса для получения бегуна.
            /// </summary>
            /// <param name="context">Контекст базы данных для работы с сущностью бегуна.</param>
            public Handler(RunnersContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Обработчик запроса для получения бегуна по-указанному ID.
            /// Ищет бегуна по ID в базе данных.
            /// </summary>
            /// <param name="request">Запрос с ID бегуна для поиска.</param>
            /// <param name="cancellationToken">Токен отмены операции.</param>
            /// <returns>Бегуна с указанным ID, если найден, иначе null.</returns>
            public async Task<Core.Entities.Runner?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Runners.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            }
        }
    }
}

