using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace RunnerApi.Filters;

/// <summary>
/// Фильтр для обработки исключений в приложении.
/// </summary>
public abstract class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Инициализирует новый экземпляр фильтра исключений.
    /// </summary>
    /// <param name="logger">Логгер для записи ошибок.</param>
    /// <param name="hostEnvironment">Информация о текущем окружении приложения.</param>
    protected ExceptionFilter(ILogger<ExceptionFilter> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    /// <summary>
    /// Метод, вызываемый при возникновении исключения.
    /// </summary>
    /// <param name="context">Контекст исключения, содержащий информацию об ошибке.</param>
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        // Логирование ошибки с полными данными для разработчиков в режиме отладки
        _logger.LogError(exception, "Произошла непредвиденная ошибка.");

        // Проверяем, находимся ли мы в продакшн-среде
        if (_hostEnvironment.IsDevelopment())
        {
            // В режиме разработки показываем более подробную информацию
            context.Result = new ObjectResult(new
            {
                Message = "Произошла непредвиденная ошибка.",
                Details = exception.Message  // Показываем полное сообщение об ошибке (включая стектрейс) только в разработке
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
        else
        {
            // В продакшн-среде скрываем детали ошибки
            context.Result = new ObjectResult(new
            {
                Message = "Внутренняя ошибка сервера. Что-то пошло не так. Попробуйте снова позже."  // Сообщение без лишних деталей
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        // Обработка других исключений
        if (exception is ArgumentNullException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Отсутствуют обязательные аргументы. Пожалуйста, убедитесь, что все поля заполнены правильно."
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        else if (exception is ArgumentException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Один или несколько аргументов недействительны. Пожалуйста, проверьте ваш ввод и попробуйте снова."
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        else if (exception is ArgumentOutOfRangeException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Один или несколько аргументов вне допустимого диапазона. Пожалуйста, проверьте ваш ввод и попробуйте снова."
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        else if (exception is KeyNotFoundException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Запрашиваемый элемент не найден. Пожалуйста, проверьте ввод или попробуйте снова."
            })
            {
                StatusCode = (int)HttpStatusCode.NotFound, // NotFound (404)
            };
        }
        else if (exception is InvalidOperationException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Операцию не удалось выполнить. Пожалуйста, попробуйте снова позже."
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        else if (exception is UnauthorizedAccessException)
        {
            context.Result = new ObjectResult(new
            {
                Message = "У вас нет прав для выполнения этой операции."
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized, // Unauthorized (401)
            };
        }

        // Устанавливаем флаг, что исключение обработано
        context.ExceptionHandled = true;
    }
}

