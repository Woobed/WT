using Microsoft.EntityFrameworkCore;
using Postgres;
using UskovWA;
using UskovWA.Components;
using YouGileMethods;

var builder = WebApplication.CreateBuilder(args);
/////////////////////////// ��� dependency injection
// ��� ����������� �������� ������� � blazor (���� onclick)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ���������� TaskCreator� ��� scoped ������
builder.Services.AddScoped<TaskCreator>(provider => new TaskCreator(Configurator.GetApiData()));

// ����������� �� ���������
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// ����������� OrderService
builder.Services.AddScoped<OrderService>();

///////////////////////////////////////////////// ������ ��� ������� �����

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