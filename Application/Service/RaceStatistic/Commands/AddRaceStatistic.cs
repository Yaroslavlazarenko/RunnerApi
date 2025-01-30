using AutoMapper;
using Core.DTOs.RaceStatisticDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Service.RaceStatistic.Commands;

/// <summary>
/// Команда для добавления статистики гонки для участника.
/// </summary>
public static class AddRaceStatistic
{
    /// <summary>
    /// Запрос для добавления статистики гонки для участника.
    /// </summary>
    /// <param name="RaceStatisticDto">DTO, содержащий данные для создания статистики.</param>
    public record Command(RaceStatisticDto RaceStatisticDto) : IRequest<Core.Entities.RaceStatistic?>;

    /// <summary>
    /// Обработчик команды для добавления статистики гонки для участника.
    /// </summary>
    private class Handler : IRequestHandler<Command, Core.Entities.RaceStatistic?>
    {
        private readonly RunnersContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор обработчика.
        /// </summary>
        /// <param name="context">Контекст базы данных для работы с гонками и участниками.</param>
        /// <param name="mapper">Маппер для преобразования DTO в сущности.</param>
        public Handler(RunnersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Обработчик команды для добавления статистики гонки для участника.
        /// </summary>
        /// <param name="request">Запрос с данными для статистики гонки.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Добавленная статистика гонки.</returns>
        /// <exception cref="KeyNotFoundException">Если гонка или участник с указанными ID не найдены.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Если время результата отрицательное.</exception>
        /// <exception cref="InvalidOperationException">Если статистика для этой гонки и участника уже существует.</exception>
        public async Task<Core.Entities.RaceStatistic?> Handle(Command request, CancellationToken cancellationToken)
        {
            // Проверяем, существует ли гонка с таким ID
            var raceExists = await _context.Races
                .AnyAsync(r => r.Id == request.RaceStatisticDto.RaceId, cancellationToken);
            
            if (!raceExists)
            {
                throw new KeyNotFoundException($"Race with ID {request.RaceStatisticDto.RaceId} not found.");
            }

            // Проверяем, существует ли участник с таким ID
            var runnerExists = await _context.Runners
                .AnyAsync(r => r.Id == request.RaceStatisticDto.RunnerId, cancellationToken);
            
            if (!runnerExists)
            {
                throw new KeyNotFoundException($"Runner with ID {request.RaceStatisticDto.RunnerId} not found.");
            }

            // Проверяем, что время результата не отрицательное
            if (request.RaceStatisticDto.TimeResult < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(request.RaceStatisticDto.TimeResult), "TimeResult cannot be negative.");
            }

            // Проверяем, существует ли уже статистика для данной гонки и участника
            var existingStat = await _context.RaceStatistics
                .FirstOrDefaultAsync(rs => rs.RaceId == request.RaceStatisticDto.RaceId && rs.RunnerId == request.RaceStatisticDto.RunnerId, cancellationToken);

            if (existingStat != null)
            {
                throw new InvalidOperationException($"Race statistics already exist for runner {request.RaceStatisticDto.RunnerId} in race {request.RaceStatisticDto.RaceId}.");
            }

            // Маппим DTO в сущность
            var raceStatistic = _mapper.Map<Core.Entities.RaceStatistic>(request.RaceStatisticDto);

            // Добавляем статистику в базу данных
            await _context.RaceStatistics.AddAsync(raceStatistic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return raceStatistic;
        }
    }
}

