using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Project_guildaAPI;
using AventureiroDATA.DAO;
using GuildasDATA.DAO;
using ItensDATA.DAO;
using MissoesDATA.DAO;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AventureiroDAO>();
builder.Services.AddScoped<GuildaDAO>();
builder.Services.AddScoped<ItemDAO>();
builder.Services.AddScoped<MissaoDAO>();    


await builder.Build().RunAsync();
