using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Runner"/> для работы с Entity Framework.
/// Класс <see cref="RunnerConfiguration"/> реализует интерфейс <see cref="IEntityTypeConfiguration{TEntity}"/>,
/// чтобы настроить отображение сущности <see cref="Runner"/> на таблицу базы данных.
/// </summary>
public class RunnerConfiguration : IEntityTypeConfiguration<Runner>
{
    /// <summary>
    /// Метод конфигурации сущности <see cref="Runner"/>.
    /// Здесь задаются все параметры отображения и ограничения для таблицы в базе данных.
    /// </summary>
    /// <param name="builder">Объект для построения конфигурации сущности <see cref="Runner"/>.</param>
    public void Configure(EntityTypeBuilder<Runner> builder)
    {
        // Настройка свойства Name
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Настройка свойства Country
        builder.Property(r => r.Country)
            .IsRequired()
            .HasMaxLength(50);

        // Настройка свойства Gender
        builder.Property(r => r.Gender)
            .IsRequired()
            .HasConversion<int>();
        
        // Настройка связи с сущностью RaceStatistic
        builder.HasMany(r => r.Statistics)
            .WithOne(rs => rs.Runner)
            .HasForeignKey(rs => rs.RunnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

