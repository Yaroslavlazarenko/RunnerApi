using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Класс, содержащий метод для регистрации сервисов приложения.
/// </summary>
public static class ApplicationRegistration
{
    /// <summary>
    /// Метод расширения для регистрации сервисов приложения в контейнере зависимостей.
    /// Этот метод добавляет MediatR и регистрирует все сервисы из текущей сборки.
    /// </summary>
    /// <param name="services">Коллекция сервисов, в которую будут добавлены зависимости.</param>
    /// <param name="configuration">Конфигурация приложения, используемая для настройки сервисов.</param>
    public static void ApplicationRegistrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
