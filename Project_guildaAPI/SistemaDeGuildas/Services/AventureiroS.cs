using System;
using SistemaDeGuildas.Models;
using Aventureiros.DAO;

namespace SistemaDeGuildas.Services
{
    public class AventureiroS
    {
        private readonly AventureiroDAO _aventureiroDAO;

        public AventureiroS()
        {
            _aventureiroDAO = new AventureiroDAO();
        }

        public async Task CadastrarAventureirosAsync(Aventureiro novoAventureiro)
        {
            if(novoAventureiro.Nivel == 1)
                novoAventureiro.Ouro += 100m; // Ouro inicial para aventureiros de nível 1
            
            await _aventureiroDAO.AdicionarAventureiroAsync(novoAventureiro);
        }

        public async Task<List<Aventureiro>> ListarAventureirosAsync()
        {
            return await _aventureiroDAO.BuscarAventureirosAsync();
        }
    }
}
