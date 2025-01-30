using System.ComponentModel.DataAnnotations;
using Application.Service.Runner.Commands;
using Application.Service.Runner.Queries;
using Core.DTOs.RunnerDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RunnerApi.Controllers;

/// <summary>
/// Контроллер для управления бегунами.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class RunnerController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для работы с бегунами.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    public RunnerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение бегуна по ID.
    /// </summary>
    /// <param name="id">Идентификатор бегуна.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Бегун с указанным ID или 404, если не найден.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var runner = await _mediator.Send(new GetRunnerById.Query(id), cancellationToken);
        return runner != null ? Ok(runner) : NotFound($"Runner with ID {id} not found.");
    }

    /// <summary>
    /// Получение списка бегунов с пагинацией.
    /// </summary>
    /// <param name="getRunnersPageDto">DTO с параметрами для пагинации бегунов.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Страница с бегунами или 404, если бегуны не найдены.</returns>
    [HttpPost]
    public async Task<IActionResult> GetPage([Required] [FromBody] GetRunnersPageDto getRunnersPageDto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRunnersPage.Query(getRunnersPageDto), cancellationToken);
        return result.Items.Any() ? Ok(result) : NotFound("No runners found.");
    }

    /// <summary>
    /// Добавление нового бегуна.
    /// </summary>
    /// <param name="runnerDto">DTO с данными нового бегуна.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Созданный бегун или 400, если создание не удалось.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([Required] [FromBody] RunnerDto runnerDto, CancellationToken cancellationToken)
    {
        var runner = await _mediator.Send(new AddRunner.Command(runnerDto), cancellationToken);
        return runner != null ? Ok(runner) : BadRequest("Runner could not be created.");
    }

    /// <summary>
    /// Обновление информации о бегуне.
    /// </summary>
    /// <param name="updateRunnerDto">DTO с обновленными данными бегуна.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Обновленный бегун или 404, если бегун не найден.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([Required] [FromBody] UpdateRunnerDto updateRunnerDto, CancellationToken cancellationToken)
    {
        var runner = await _mediator.Send(new UpdateRunnerById.Command(updateRunnerDto), cancellationToken);
        return runner != null ? Ok(runner) : NotFound($"Runner with ID {updateRunnerDto.Id} not found.");
    }

    /// <summary>
    /// Удаление бегуна по ID.
    /// </summary>
    /// <param name="id">Идентификатор бегуна, которого нужно удалить.</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Информация об удаленном бегуне или 404, если бегун не найден.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([Required] [FromRoute] int id, CancellationToken cancellationToken)
    {
        var runner = await _mediator.Send(new DeleteRunner.Command(id), cancellationToken);
        return runner != null ? Ok(runner) : NotFound($"Runner with ID {id} not found.");
    }
}
