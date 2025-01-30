using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence;

/// <summary>
/// Класс регистрации сервисов для работы с базой данных.
/// Содержит метод для регистрации контекста базы данных и настройки подключения.
/// </summary>
public static class PersistenceRegistration
{
    /// <summary>
    /// Имя строки подключения, которое используется для получения строки из конфигурации.
    /// </summary>
    private const string ConnectionsStringName = "ConnectionData";

    /// <summary>
    /// Метод для регистрации контекста базы данных в контейнере зависимостей.
    /// Читает строку подключения из конфигурации и настраивает использование PostgreSQL.
    /// </summary>
    /// <param name="serviceCollection">Коллекция сервисов, в которую добавляются зависимости.</param>
    /// <param name="configuration">Объект конфигурации для доступа к строкам подключения и другим настройкам.</param>
    /// <exception cref="NullReferenceException">Выбрасывается, если строка подключения не найдена в конфигурации.</exception>
    public static void PersistenceRegistrationDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Получаем строку подключения из конфигурации
        var connectionString = configuration.GetConnectionString(ConnectionsStringName) ?? throw new NullReferenceException();
        
        // Добавляем контекст базы данных с настройкой подключения
        serviceCollection.AddDbContext<RunnersContext>(opt => opt.UseNpgsql(connectionString));
    }
}