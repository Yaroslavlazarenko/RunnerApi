using System.Text.Json.Serialization;
using Application;
using Core.Mappings;
using Microsoft.OpenApi.Models;
using Persistence;
using RunnerApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров с настройкой сериализации JSON для игнорирования циклических ссылок
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddMvc(options =>
{
    // Добавление фильтра для обработки исключений
    options.Filters.Add<ExceptionFilter>();
});

// Добавление конфигурации для Swagger/OpenAPI
// Подробнее об OpenAPI можно узнать по адресу: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Настройка Swagger, включая добавление описаний для модели и параметров
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Runner API",
        Version = "v1",
        Description = "API для управления бегунами и забегами"
    });

    // Опционально: Генерация описания для контроллеров
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "RunnerApi.xml"));
});


// Добавление AutoMapper для автоматического маппинга объектов
builder.Services.AddAutoMapper(typeof(ICoreAssemblyMarker).Assembly);

// Настройка CORS для разрешения всех источников, заголовков и методов
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        );
});

// Регистрация зависимостей для слоя Persistence и Application
builder.Services.PersistenceRegistrationDb(builder.Configuration);
builder.Services.ApplicationRegistrationServices(builder.Configuration);

var app = builder.Build();

// Конфигурация пайплайна HTTP-запросов
if (app.Environment.IsDevelopment())
{
    // Включение Swagger в режиме разработки
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Перенаправление на HTTPS

//app.UseAuthorization(); // Включение авторизации (закомментировано)

app.UseCors(); // Включение политики CORS

//app.UseMiddleware<GlobalExceptionHandlingMiddleware>(); // Использование middleware для глобальной обработки исключений (закомментировано)
//app.UseMiddleware<AppMiddleware>(); // Использование middleware для других глобальных настроек (закомментировано)

app.MapControllers(); // Маппинг маршрутов для контроллеров

app.Run(); // Запуск приложения
