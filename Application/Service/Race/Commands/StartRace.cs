using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.Race.Commands;

/// <summary>
/// Команда для старта гонки по ID.
/// </summary>
public static class StartRace
{
    /// <summary>
    /// Запрос для старта гонки по-указанному ID.
    /// </summary>
    /// <param name="RaceId">Идентификатор гонки, которую необходимо стартовать.</param>
    public record Command(int RaceId) : IRequest<Core.Entities.Race>;

    /// <summary>
    /// Обработчик команды старта гонки.
    /// Проверяет, существует ли гонка, что она не стартовала ранее, и есть ли участники.
    /// Генерирует статистику для участников и обновляет результаты.
    /// </summary>
    private class Handler : IRequestHandler<Command, Core.Entities.Race>
    {
        private static Random _random = new Random();
        private readonly RunnersContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="mapper">Маппер для преобразования данных.</param>
        public Handler(RunnersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает команду на старт гонки.
        /// Проверяет существование гонки, ее статус и наличие участников.
        /// Генерирует статистику для участников и обновляет результаты.
        /// </summary>
        /// <param name="request">Запрос с ID гонки для старта.</param>
        /// <param name="cancellationToken">Токен для отмены операции.</param>
        /// <returns>Обновленная информация о гонке с результатами участников.</returns>
        /// <exception cref="KeyNotFoundException">Если гонка с указанным ID не найдена.</exception>
        /// <exception cref="InvalidOperationException">Если гонка уже стартовала или если не найдено участников.</exception>
        public async Task<Core.Entities.Race> Handle(Command request, CancellationToken cancellationToken)
        {
            // Проверка существования гонки с данным Id
            var race = await _context.Races.Include(race => race.Statistics)
                .FirstOrDefaultAsync(r => r.Id == request.RaceId, cancellationToken);

            if (race == null)
            {
                throw new KeyNotFoundException($"Race with ID {request.RaceId} not found.");
            }

            // Проверка, что гонка не стартовала ранее
            if (race.Statistics != null && race.Statistics.Any())
            {
                throw new InvalidOperationException($"Race with ID {request.RaceId} has already started.");
            }

            // Получение списка участников в зависимости от типа гонки
            List<Core.Entities.Runner> runners;

            if (race.RaceGender == RaceGender.General)
            {
                runners = await _context.Runners.ToListAsync(cancellationToken);
            }
            else
            {
                runners = await _context.Runners
                    .Where(r => r.Gender == (Gender)race.RaceGender)
                    .ToListAsync(cancellationToken);
            }

            // Проверка, что есть хотя бы один участник
            if (runners.Count == 0)
            {
                throw new InvalidOperationException($"No runners found for race with ID {request.RaceId}.");
            }

            // Генерация статистики для участников
            var statistics = runners
                .Select(runner =>
                {
                    var stat = _mapper.Map<Core.Entities.RaceStatistic>(runner);
                    stat.Race = race;
                    stat.RaceId = race.Id;
                    stat.TimeResult = TimeSpan.FromHours((double)race.RaceLength / _random.Next(14, 20));
                    return stat;
                }).ToList();

            // Обновление статистики в гонке
            var stats = await _context.Races.FirstOrDefaultAsync(r => r.Id == request.RaceId, cancellationToken);
            if (stats != null) stats.Statistics = statistics;

            // Сортировка статистики по времени и обновление рейтингов
            var sortedStatistics = statistics.OrderBy(stat => stat.TimeResult).ToList();

            for (var i = 0; i < sortedStatistics.Count; i++)
            {
                sortedStatistics[i].Runner.RatingValue += (int)(50 * Math.Exp(-5 * i));
            }

            await _context.SaveChangesAsync(cancellationToken);

            return race;
        }
    }
}
