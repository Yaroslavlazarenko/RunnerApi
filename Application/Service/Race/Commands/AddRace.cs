using AutoMapper;
using Core.DTOs.RaceDtos;
using Core.Entities;
using MediatR;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Race.Commands;

/// <summary>
/// Команда для добавления новой гонки в систему.
/// </summary>
public static class AddRace
{
    /// <summary>
    /// Команда для добавления новой гонки.
    /// </summary>
    /// <param name="RaceDto">Объект, содержащий данные новой гонки.</param>
    public record Command(RaceDto RaceDto) : IRequest<Core.Entities.Race>;

    /// <summary>
    /// Обработчик команды для добавления новой гонки.
    /// Выполняет все проверки и добавляет гонку в базу данных.
    /// </summary>
    private class Handler : IRequestHandler<Command, Core.Entities.Race>
    {
        private readonly RunnersContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="mapper">Маппер для преобразования DTO в сущность.</param>
        public Handler(RunnersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает команду на добавление новой гонки.
        /// Выполняет все необходимые проверки и сохраняет гонку в базу данных.
        /// </summary>
        /// <param name="request">Запрос с данными новой гонки.</param>
        /// <param name="cancellationToken">Токен для отмены операции.</param>
        /// <returns>Добавленную гонку.</returns>
        /// <exception cref="ArgumentException">Если название или страна гонки не указаны.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Если длина гонки некорректна.</exception>
        /// <exception cref="InvalidOperationException">Если гонка с таким названием и датой уже существует.</exception>
        public async Task<Core.Entities.Race> Handle(Command request, CancellationToken cancellationToken)
        {
            // Проверка на пустое или пробельное название гонки
            if (string.IsNullOrWhiteSpace(request.RaceDto.Name))
            {
                throw new ArgumentException("Race name is required.");
            }

            // Проверка на пустую страну
            if (string.IsNullOrWhiteSpace(request.RaceDto.Country))
            {
                throw new ArgumentException("Race country is required.");
            }

            // Проверка на корректность длины гонки (должна быть больше нуля)
            if (request.RaceDto.RaceLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.RaceDto.RaceLength), "Race length must be greater than zero.");
            }

            // Проверка на допустимое значение RaceGender
            if (!Enum.IsDefined(typeof(RaceGender), request.RaceDto.RaceGender))
            {
                throw new ArgumentException($"Invalid value for RaceGender: {request.RaceDto.RaceGender}");
            }

            // Проверка на существование гонки с таким же названием и датой
            var existingRace = await _context.Races
                .FirstOrDefaultAsync(r => r.Name == request.RaceDto.Name && r.Date == request.RaceDto.Date, cancellationToken);

            // Если такая гонка уже существует, выбрасываем исключение
            if (existingRace != null)
            {
                throw new InvalidOperationException($"A race with the name '{request.RaceDto.Name}' already exists on the specified date.");
            }

            // Маппинг DTO в сущность Core.Entities.Race
            var race = _mapper.Map<Core.Entities.Race>(request.RaceDto);
            
            // Добавление гонки в базу данных
            await _context.Races.AddAsync(race, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return race;
        }
    }
}
