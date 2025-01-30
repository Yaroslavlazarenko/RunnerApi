using System.ComponentModel.DataAnnotations;
using Application.Service.RaceStatistic.Commands;
using Application.Service.RaceStatistic.Queries;
using Core.DTOs.RaceStatisticDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RunnerApi.Controllers;

/// <summary>
/// Контроллер для управления статистикой гонок.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class RaceStatisticController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для работы со статистикой гонок.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    public RaceStatisticController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение статистики гонки по ID.
    /// </summary>
    /// <param name="id">Идентификатор статистики гонки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Статистика гонки, если найдена, иначе 404.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var raceStatistic = await _mediator.Send(new GetRaceStatisticById.Query(id), cancellationToken);

        if (raceStatistic is not null)
        {
            return Ok(raceStatistic);
        }

        return NotFound($"Race with ID {id} not found.");
    }

    /// <summary>
    /// Получение статистики по гонке по её ID.
    /// </summary>
    /// <param name="getAllRaceStatisticsByRaceIdDto">DTO с параметрами для получения статистики гонки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Список статистики для указанной гонки.</returns>
    [HttpPost]
    public async Task<IActionResult> GetStatisticsByRaceId([Required] [FromBody] GetAllRaceStatisticsByRaceIdDto getAllRaceStatisticsByRaceIdDto, CancellationToken cancellationToken)
    {
        var statistics = await _mediator.Send(new GetAllRaceStatisticsByRaceId.Query(getAllRaceStatisticsByRaceIdDto), cancellationToken);

        if (!statistics.Any())
        {
            return NotFound($"No statistics found for race with ID {getAllRaceStatisticsByRaceIdDto.RaceId}.");
        }

        return Ok(statistics);
    }

    /// <summary>
    /// Получение статистики гонок с пагинацией.
    /// </summary>
    /// <param name="getRaceStatisticPageDto">DTO с параметрами для пагинации статистики гонок.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Статистика гонок на текущей странице.</returns>
    [HttpPost]
    public async Task<IActionResult> GetPage([Required] [FromBody] GetRaceStatisticPageDto getRaceStatisticPageDto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRaceStatisticsPage.Query(getRaceStatisticPageDto), cancellationToken);

        if (!result.Any())
        {
            return NotFound("No races found.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Добавление новой статистики для гонки.
    /// </summary>
    /// <param name="request">DTO с данными для создания новой статистики гонки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Данные о созданной статистике гонки.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([Required] [FromBody] RaceStatisticDto request, CancellationToken cancellationToken)
    {
        var raceStatistic = await _mediator.Send(new AddRaceStatistic.Command(request), cancellationToken);

        if (raceStatistic is not null)
        {
            return Ok(raceStatistic);
        }

        return BadRequest("Race statistic could not be created.");
    }

    /// <summary>
    /// Удаление статистики гонки по ID.
    /// </summary>
    /// <param name="id">Идентификатор статистики гонки, которую нужно удалить.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Информация об удаленной статистике гонки.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var raceStatistic = await _mediator.Send(new DeleteRaceStatistic.Command(id), cancellationToken);

        return Ok(raceStatistic);
    }
}


