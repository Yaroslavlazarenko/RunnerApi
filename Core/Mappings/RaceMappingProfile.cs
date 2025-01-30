using AutoMapper;
using Core.DTOs.RaceDtos;
using Core.Entities;

namespace Core.Mappings;

/// <summary>
/// Профиль маппинга для сущности гонки (Race).
/// Настроен для преобразования объектов типа <see cref="RaceDto"/> в <see cref="Race"/>.
/// </summary>
public class RaceMappingProfile : Profile
{
    /// <summary>
    /// Конструктор профиля маппинга.
    /// Определяет правила преобразования данных между <see cref="RaceDto"/> и <see cref="Race"/>.
    /// </summary>
    public RaceMappingProfile()
    {
        // Создание маппинга между RaceDto и Race
        CreateMap<RaceDto, Race>();
    }
}
