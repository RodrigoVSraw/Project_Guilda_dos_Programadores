using System;
using SistemaDeGuildas.Models;
using AventureiroDATA.DAO;

namespace SistemaDeGuildas.Services
{
    public class AventureiroS
    {
        private readonly AventureiroDAO _aventureiroDAO;

        public AventureiroS(AventureiroDAO aventureiroDAO)
        {
            _aventureiroDAO = aventureiroDAO;
        }

        public async Task CadastrarAventureirosAsync(Aventureiro novoAventureiro)
        {
            if(novoAventureiro.Nivel == 1)
                novoAventureiro.Ouro += 100m;
            
            await _aventureiroDAO.AdicionarAventureiroAsync(novoAventureiro);
        }

        public async Task<List<Aventureiro>> ListarAventureirosAsync()
        {
            return await _aventureiroDAO.BuscarAventureirosAsync();
        }
    }
}
