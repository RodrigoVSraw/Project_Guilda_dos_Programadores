using SistemaDeGuildas.Models;
using AventureiroDATA.DAO;
using BuilderConnections;
using GuildasDATA.DAO;


Console.WriteLine("Iniciando teste de acesso ao banco de dados...");
var dao = new AventureiroDAO();

try
{
    var herois = await dao.BuscarAventureirosAsync();

    Console.WriteLine($"Sucesso! Encontrámos {herois.Count} aventureiros.");

    foreach (var h in herois)
    {
        Console.WriteLine($"- {h.Nome} (Nível {h.Nivel})");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO FATAL: {ex.Message}");
}
Console.ReadKey();
Console.WriteLine("Testando Conexão com a tabela de guildas...");
var daoG = new GuildaDAO();
try
{
    var guildas = await daoG.BuscarGuildasAsync();
    Console.WriteLine($"Sucesso! Encontrámos {guildas.Count} guildas.");
    foreach (var g in guildas)
    {
        Console.WriteLine($"- {g.Nome} (Nível Requerido: {g.NivelRequerido})");
    }

}
catch (Exception ex)
{
    Console.WriteLine($"ERRO FATAL: {ex.Message}");
} Console.ReadKey();