using AutoMapper;
using Core.DTOs.RunnerDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Runner.Commands
{
    /// <summary>
    /// Команда для обновления данных бегуна по его ID.
    /// </summary>
    public static class UpdateRunnerById
    {
        /// <summary>
        /// Запрос для обновления данных бегуна по его ID.
        /// </summary>
        /// <param name="UpdateRunnerDto">DTO с новыми данными бегуна для обновления.</param>
        public record Command(UpdateRunnerDto UpdateRunnerDto) : IRequest<Core.Entities.Runner?>;

        /// <summary>
        /// Обработчик команды для обновления данных бегуна.
        /// Ищет бегуна с указанным ID и обновляет его данные в базе данных.
        /// </summary>
        public class Handler : IRequestHandler<Command, Core.Entities.Runner?>
        {
            private readonly RunnersContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Конструктор обработчика команды обновления бегуна.
            /// </summary>
            /// <param name="context">Контекст базы данных для работы с сущностью бегуна.</param>
            /// <param name="mapper">Объект для маппинга данных между DTO и сущностью.</param>
            public Handler(RunnersContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            /// <summary>
            /// Обработчик команды для обновления данных бегуна.
            /// Ищет бегуна по-указанному ID, обновляет его данные и сохраняет изменения.
            /// </summary>
            /// <param name="request">Запрос с новыми данными бегуна для обновления.</param>
            /// <param name="cancellationToken">Токен отмены операции.</param>
            /// <returns>Обновленного бегуна.</returns>
            /// <exception cref="KeyNotFoundException">Если бегун с указанным ID не найден в базе данных.</exception>
            public async Task<Core.Entities.Runner?> Handle(Command request, CancellationToken cancellationToken)
            {
                var runner = await _context.Runners.FirstOrDefaultAsync(r => r.Id == request.UpdateRunnerDto.Id, cancellationToken);

                if (runner == null)
                {
                    // Если бегун с указанным ID не найден, выбрасываем исключение
                    throw new KeyNotFoundException($"Runner with ID {request.UpdateRunnerDto.Id} not found.");
                }

                // Маппинг данных из DTO в сущность
                _mapper.Map(request.UpdateRunnerDto, runner);

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync(cancellationToken);

                return runner;
            }
        }
    }
}
