using SistemaDeGuildas.Models;
using AventureiroDATA.DAO;
using BuilderConnections;


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