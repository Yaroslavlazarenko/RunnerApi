using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

/// <summary>
/// Контекст базы данных для работы с сущностями бегунов, гонок и статистики гонок.
/// Класс <see cref="RunnersContext"/> наследует от <see cref="DbContext"/> и предоставляет 
/// доступ к таблицам базы данных для сущностей <see cref="Runner"/>, <see cref="Race"/> и <see cref="RaceStatistic"/>.
/// </summary>
public class RunnersContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр контекста базы данных с заданными параметрами.
    /// </summary>
    /// <param name="options">Параметры контекста базы данных, определяющие подключение и конфигурацию.</param>
    public RunnersContext(DbContextOptions<RunnersContext> options) : base(options) { }
    
    /// <summary>
    /// Коллекция бегунов, представляющая таблицу <b>Runners</b> в базе данных.
    /// </summary>
    public DbSet<Runner> Runners { get; set; }

    /// <summary>
    /// Коллекция гонок, представляющая таблицу <b>Races</b> в базе данных.
    /// </summary>
    public DbSet<Race> Races { get; set; }

    /// <summary>
    /// Коллекция статистики гонок, представляющая таблицу <b>RaceStatistics</b> в базе данных.
    /// </summary>
    public DbSet<RaceStatistic> RaceStatistics { get; set; }
}
