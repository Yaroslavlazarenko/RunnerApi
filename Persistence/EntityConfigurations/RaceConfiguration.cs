using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Race"/> для работы с Entity Framework.
/// Класс <see cref="RaceConfiguration"/> реализует интерфейс <see cref="IEntityTypeConfiguration{TEntity}"/>,
/// чтобы настроить отображение сущности <see cref="Race"/> на таблицу базы данных.
/// </summary>
public class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    /// <summary>
    /// Метод конфигурации сущности <see cref="Race"/>.
    /// Здесь задаются все параметры отображения и ограничения для таблицы в базе данных.
    /// </summary>
    /// <param name="builder">Объект для построения конфигурации сущности <see cref="Race"/>.</param>
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        // Настройка свойства Name
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Настройка свойства Country
        builder.Property(r => r.Country)
            .IsRequired()
            .HasMaxLength(50);

        // Настройка свойства RaceLength
        builder.Property(r => r.RaceLength)
            .IsRequired();

        // Настройка свойства Date
        builder.Property(r => r.Date)
            .IsRequired();

        // Настройка свойства RaceGender
        builder.Property(r => r.RaceGender)
            .IsRequired()
            .HasConversion<int>(); 

        // Настройка таблицы для сущности Race
        builder.ToTable("Races", t => t.HasCheckConstraint("CK_Race_RaceGender", "[RaceGender] IN (1, 2, 3)"));
    }
}
