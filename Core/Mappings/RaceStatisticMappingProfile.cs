using AutoMapper;
using Core.DTOs.RaceStatisticDtos;
using Core.Entities;

namespace Core.Mappings;

/// <summary>
/// Профиль маппинга для статистики гонки (RaceStatistic).
/// Настроен для преобразования объектов типа <see cref="RaceStatisticDto"/> в <see cref="RaceStatistic"/>.
/// </summary>
public class RaceStatisticMappingProfile : Profile
{
    /// <summary>
    /// Конструктор профиля маппинга.
    /// Определяет правила преобразования данных между <see cref="RaceStatisticDto"/> и <see cref="RaceStatistic"/>.
    /// </summary>
    public RaceStatisticMappingProfile()
    {
        // Создание маппинга между RaceStatisticDto и RaceStatistic
        CreateMap<RaceStatisticDto, RaceStatistic>();
    }
}
