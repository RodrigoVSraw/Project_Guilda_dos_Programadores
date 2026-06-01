using System;
using SistemaDeGuildas.Models;
using MissoesDATA.DAO;
using AventureiroDATA.DAO;
using GuildasDATA.DAO;

namespace SistemaDeGuildas.Services
{
    public class MissaoS
    {
        private readonly MissaoDAO _missaoDAO;

        public MissaoS(MissaoDAO missaoDAO)
        {
            _missaoDAO = missaoDAO;
        }

        public async Task CriarMissao(Missao missao)
        {
            if (missao == null)
                throw new ArgumentNullException(nameof(missao), "A missão não pode ser nula.");

            await _missaoDAO.CriarMissaoAsync(missao);
        }

        public async Task<List<Missao>> VerTodasAsMissoes()
        {
            return await _missaoDAO.VerTodasAsMissoesAsync();
        }
        public async Task<Missao> VerMissaoPorId(int id)
        {
            return await _missaoDAO.VerMissaoPorIdAsync(id);
        }
        public async Task<List<Missao>> VerMissaoDoAventureiro(int id)
        {
            return await _missaoDAO.MostrarMissaoDoAventureiro(id);
        }

        public async Task<List<Missao>> VerMissaoDaGuilda(int id)
        {
            return await _missaoDAO.MostrarMissaoDaGuilda(id);
        }

        public async Task ExcluirMissao(int id)
        {
            await _missaoDAO.ExcluirMissao(id);
        }

        public async Task AlterarDescricao(int id, string novaDescricao)
        {
            if (id <= 0)
                throw new ArgumentException("O ID da missão deve ser válido.");

            if (string.IsNullOrWhiteSpace(novaDescricao))
                throw new ArgumentException("A nova descrição do contrato não pode estar vazia.");

            await _missaoDAO.AlterarDescricaoDaMissao(id, novaDescricao);
        }

        public async Task AlterarRecompensa(int id, decimal novoOuro, int novaExperiencia)
        {
            if (id <= 0)
                throw new ArgumentException("O ID da missão deve ser válido.");

            if (novoOuro < 0 || novaExperiencia < 0)
                throw new ArgumentException("A guilda não pode cobrar dos heróis! As recompensas devem ser positivas.");

            await _missaoDAO.AlterarRecompensaDaMissao(id, novoOuro, novaExperiencia);
        }
    }
}
