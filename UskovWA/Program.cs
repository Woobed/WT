using Microsoft.EntityFrameworkCore;
using Postgres;
using UskovWA;
using UskovWA.Components;
using YouGileMethods;

var builder = WebApplication.CreateBuilder(args);
/////////////////////////// тут dependency injection
// это завставляет работать скрипты в blazor (типа onclick)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// добавление TaskCreatorа как scoped сервис
builder.Services.AddScoped<TaskCreator>(provider => new TaskCreator(Configurator.GetApiData()));

// регистрация дб контекста
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// регистрация OrderService
builder.Services.AddScoped<OrderService>();

///////////////////////////////////////////////// дальше код шаблона вижлы

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();