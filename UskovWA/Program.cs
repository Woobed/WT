using UskovWA.Components;

var builder = WebApplication.CreateBuilder(args);

// Добавляем только Interactive Server Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // <- Этого достаточно

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Основная конфигурация маршрутизации
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();