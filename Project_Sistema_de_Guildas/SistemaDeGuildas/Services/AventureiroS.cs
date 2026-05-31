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
            if (novoAventureiro.Nivel == 1)
                novoAventureiro.Ouro += 100m;

            await _aventureiroDAO.AdicionarAventureiroAsync(novoAventureiro);
        }

        public async Task<List<Aventureiro>> ListarAventureirosAsync()
        {
            return await _aventureiroDAO.BuscarAventureirosAsync();
        }

        public async Task<Aventureiro> ObterAventureiroPorIdAsync(int id)
        {
            return await _aventureiroDAO.BuscarAventureiroPorIdAsync(id);
        }

        public async Task AtualizarAventureiroAsync(Aventureiro aventureiroAtualizado)
        {
            var aventureiroExistente = await _aventureiroDAO.BuscarAventureiroPorIdAsync(aventureiroAtualizado.Id);
            if (aventureiroExistente == null)
                throw new Exception("Aventureiro não encontrado.");

            await _aventureiroDAO.EvoluirAventureiroAsync(aventureiroAtualizado);
        }

        public async Task EntrarEmUmaGuildaAsync(int aventureiroId, int guildaId)
        {
            var aventureiro = await _aventureiroDAO.BuscarAventureiroPorIdAsync(aventureiroId);
            if (aventureiro == null)
                throw new Exception("Aventureiro não encontrado.");


            await _aventureiroDAO.EntrarEmUmaGuildaAsync(aventureiro.Id, aventureiro.Guilda.Id);
        }

        public async Task SairDeUmaGuildaAsync(int aventureiroId)
        {
            var aventureiro = await _aventureiroDAO.BuscarAventureiroPorIdAsync(aventureiroId);
            if (aventureiro == null)
                throw new Exception("Aventureiro não encontrado.");

            await _aventureiroDAO.SairDaGuildaAsync(aventureiro.Id);
        }

        public async Task ExcluirAventureiroAsync(int id)
        {
            var aventureiroExistente = await _aventureiroDAO.BuscarAventureiroPorIdAsync(id);
            if (aventureiroExistente == null)
                throw new Exception("Aventureiro não encontrado.");

            await _aventureiroDAO.ExcluirAventureiroPorId(id);
        }
    }
}
