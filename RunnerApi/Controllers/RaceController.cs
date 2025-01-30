using System.ComponentModel.DataAnnotations;
using Application.Service.Race.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Service.Race.Queries;
using Core.DTOs.RaceDtos;

namespace RunnerApi.Controllers;

/// <summary>
/// Контроллер для управления гонками.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class RaceController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для работы с гонками.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    public RaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение информации о гонке по ID.
    /// </summary>
    /// <param name="id">Идентификатор гонки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Информация о гонке, если найдена, иначе 404.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var race = await _mediator.Send(new GetRaceById.Query(id), cancellationToken);

        if (race is not null)
        {
            return Ok(race);
        }

        return NotFound($"Race with ID {id} not found.");
    }

    /// <summary>
    /// Получение страницы гонок с пагинацией.
    /// </summary>
    /// <param name="getRacesPageDto">DTO с параметрами для пагинации и сортировки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Список гонок на текущей странице.</returns>
    [HttpPost]
    public async Task<IActionResult> GetPage([Required] [FromBody] GetRacesPageDto getRacesPageDto, CancellationToken cancellationToken)
    {
        var races = await _mediator.Send(new GetRacePage.Query(getRacesPageDto), cancellationToken);

        if (!races.Any())
        {
            return NotFound("No races found.");
        }

        return Ok(races);
    }

    /// <summary>
    /// Добавление новой гонки.
    /// </summary>
    /// <param name="raceDto">DTO с данными для создания новой гонки.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Данные о созданной гонке.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([Required] [FromBody] RaceDto raceDto, CancellationToken cancellationToken)
    {
        var race = await _mediator.Send(new AddRace.Command(raceDto), cancellationToken) ?? throw new InvalidOperationException("Race could not be created.");

        return Ok(race);
    }

    /// <summary>
    /// Удаление гонки по ID.
    /// </summary>
    /// <param name="id">Идентификатор гонки, которую нужно удалить.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Информация об удаленной гонке.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var race = await _mediator.Send(new DeleteRace.Command(id), cancellationToken);

        return Ok(race);
    }

    /// <summary>
    /// Запуск гонки по ID.
    /// </summary>
    /// <param name="raceId">Идентификатор гонки, которую нужно запустить.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Информация о начале гонки.</returns>
    [HttpPost("{raceId:int}")]
    public async Task<IActionResult> StartRace([Required] [FromRoute] int raceId, CancellationToken cancellationToken)
    {
        var command = new StartRace.Command(raceId);
        var race = await _mediator.Send(command, cancellationToken) ?? throw new KeyNotFoundException($"Race with ID {raceId} not found.");

        return Ok(race);
    }
}
