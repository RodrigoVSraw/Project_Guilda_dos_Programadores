using Project_Sistema_de_Guildas.Components;
using BuilderConnections.DAO;
using AventureiroDATA.DAO;
using GuildasDATA.DAO;
using ItensDATA.DAO;
using MissoesDATA.DAO;
using SistemaDeGuildas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AventureiroDAO>();
builder.Services.AddScoped<GuildaDAO>();
builder.Services.AddScoped<ItemDAO>();
builder.Services.AddScoped<MissaoDAO>();

builder.Services.AddScoped<AventureiroS>();
builder.Services.AddScoped<GuildaS>();
builder.Services.AddScoped<MissaoS>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
