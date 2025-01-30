using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="RaceStatistic"/> для работы с Entity Framework.
/// Класс <see cref="RaceStatisticConfiguration"/> реализует интерфейс <see cref="IEntityTypeConfiguration{TEntity}"/>,
/// чтобы настроить отображение сущности <see cref="RaceStatistic"/> на таблицу базы данных.
/// </summary>
public class RaceStatisticConfiguration : IEntityTypeConfiguration<RaceStatistic>
{
    /// <summary>
    /// Метод конфигурации сущности <see cref="RaceStatistic"/>.
    /// Здесь задаются все параметры отображения и ограничения для таблицы в базе данных.
    /// </summary>
    /// <param name="builder">Объект для построения конфигурации сущности <see cref="RaceStatistic"/>.</param>
    public void Configure(EntityTypeBuilder<RaceStatistic> builder)
    {
        // Настройка свойства RaceId
        builder.Property(rs => rs.RaceId)
            .IsRequired();

        // Настройка свойства RunnerId
        builder.Property(rs => rs.RunnerId)
            .IsRequired();

        // Настройка свойства TimeResult
        builder.Property(rs => rs.TimeResult)
            .IsRequired();
        
        // Настройка связи с сущностью Race
        builder.HasOne(rs => rs.Race)
            .WithMany(r => r.Statistics)
            .HasForeignKey(rs => rs.RaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Настройка связи с сущностью Runner
        builder.HasOne(rs => rs.Runner)
            .WithMany(r => r.Statistics)
            .HasForeignKey(rs => rs.RunnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
