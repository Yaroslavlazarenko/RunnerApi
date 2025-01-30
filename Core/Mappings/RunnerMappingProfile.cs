using AutoMapper;
using Core.DTOs.RunnerDtos;
using Core.Entities;

namespace Core.Mappings;

/// <summary>
/// Профиль маппинга для бегуна (Runner).
/// Настроен для преобразования объектов типа <see cref="RunnerDto"/> в <see cref="Runner"/>
/// и для преобразования данных между <see cref="Runner"/> и <see cref="RaceStatistic"/>.
/// </summary>
public class RunnerMappingProfile : Profile
{
    /// <summary>
    /// Конструктор профиля маппинга.
    /// Определяет правила преобразования данных между <see cref="RunnerDto"/>, <see cref="UpdateRunnerDto"/>, 
    /// <see cref="Runner"/> и <see cref="RaceStatistic"/>.
    /// </summary>
    public RunnerMappingProfile()
    {
        // Маппинг между RunnerDto и Runner
        CreateMap<RunnerDto, Runner>();

        // Маппинг между UpdateRunnerDto и Runner
        CreateMap<UpdateRunnerDto, Runner>();

        // Маппинг между Runner и RaceStatistic
        // Задаем соответствие свойств для корректного маппинга
        CreateMap<Runner, RaceStatistic>()
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.Id))  // Соответствие RunnerId
            .ForMember(dest => dest.Runner, opt => opt.MapFrom(src => src));        // Соответствие Runner
    }
}