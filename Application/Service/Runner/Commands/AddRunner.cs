using AutoMapper;
using Core.DTOs.RunnerDtos;
using MediatR;
using Persistence.Contexts;

namespace Application.Service.Runner.Commands
{
    /// <summary>
    /// Команда для добавления нового бегуна в систему.
    /// </summary>
    public static class AddRunner
    {
        /// <summary>
        /// Запрос для добавления нового бегуна.
        /// </summary>
        /// <param name="RunnerDto">Объект DTO с данными нового бегуна, который будет добавлен в систему.</param>
        public record Command(RunnerDto RunnerDto) : IRequest<Core.Entities.Runner?>;

        /// <summary>
        /// Обработчик команды для добавления бегуна в базу данных.
        /// Проверяет обязательные поля и добавляет нового бегуна.
        /// </summary>
        public class Handler : IRequestHandler<Command, Core.Entities.Runner?>
        {
            private readonly RunnersContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Конструктор обработчика команды.
            /// </summary>
            /// <param name="context">Контекст базы данных для работы с сущностью бегуна.</param>
            /// <param name="mapper">Объект для маппинга DTO в сущность.</param>
            public Handler(RunnersContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            /// <summary>
            /// Обработчик команды для добавления нового бегуна.
            /// Проверяет обязательные поля, такие как имя и страну, а также корректность значения пола,
            /// затем добавляет нового бегуна в базу данных.
            /// </summary>
            /// <param name="request">Запрос с данными нового бегуна для добавления в систему.</param>
            /// <param name="cancellationToken">Токен отмены операции.</param>
            /// <returns>Добавленного бегуна.</returns>
            /// <exception cref="ArgumentException">Если обязательные поля пусты или пол (Gender) некорректен.</exception>
            public async Task<Core.Entities.Runner?> Handle(Command request, CancellationToken cancellationToken)
            {
                // Проверка на пустое или пробельное имя бегуна
                if (string.IsNullOrWhiteSpace(request.RunnerDto.Name))
                {
                    throw new ArgumentException("Runner name is required.");
                }

                // Проверка на пустую страну
                if (string.IsNullOrWhiteSpace(request.RunnerDto.Country))
                {
                    throw new ArgumentException("Runner country is required.");
                }

                // Проверка на корректное значение Gender
                if (!Enum.IsDefined(typeof(Gender), request.RunnerDto.Gender))
                {
                    throw new ArgumentException($"Invalid value for Gender: {request.RunnerDto.Gender}");
                }

                // Маппинг DTO в сущность Core.Entities.Runner
                var runner = _mapper.Map<Core.Entities.Runner>(request.RunnerDto);

                // Добавление бегуна в базу данных
                await _context.Runners.AddAsync(runner, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return runner;
            }
        }
    }
}
