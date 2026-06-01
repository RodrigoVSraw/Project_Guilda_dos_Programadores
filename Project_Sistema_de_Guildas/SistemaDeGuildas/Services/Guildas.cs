using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AventureiroDATA.DAO;
using GuildasDATA.DAO;
using SistemaDeGuildas.Models;

namespace SistemaDeGuildas.Services
{
    public class GuildaS
    {
        private readonly GuildaDAO _guildaDAO;

        public GuildaS(GuildaDAO guildaDAO)
        {
            _guildaDAO = guildaDAO;
        }

        public async Task<List<Guilda>> ObterGuildasAsync()
        {
            try
            {
               
                var guildas = await _guildaDAO.BuscarGuildasAsync();

                foreach (var guilda in guildas)
                {
                    guilda.Membros = await _guildaDAO.ConsultarAventureirosDaGuildaAsync(guilda.Id);
                }

                return guildas;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter guildas: " + ex.Message, ex);
            }
        }

        public async Task<Guilda> ObterGuildaPorIdAsync(int id)
        {
            try
            {
                
                var guilda = await _guildaDAO.BuscarGuildaPorId(id);

                
                if (guilda != null)
                {
                    guilda.Membros = await _guildaDAO.ConsultarAventureirosDaGuildaAsync(id);
                }

                return guilda;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter guilda por ID: " + ex.Message, ex);
            }
        }

        public async Task CriarGuildaAsync(Guilda guilda)
        {
            try
            {
                await _guildaDAO.AdicionarGuildaAsync(guilda);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar guilda: " + ex.Message, ex);
            }
        }

        public async Task AtualizarGuildaAsync(Guilda guilda)
        {
            try
            {
                await _guildaDAO.AtualizarNivelMinimoDaGuildaAsync(guilda.Id, guilda.NivelRequerido);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar guilda: " + ex.Message, ex);
            }
        }

        public async Task ExcluirGuildaAsync(int id)
        {
            try
            {
                await _guildaDAO.ExcluirGuildaPorIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir guilda: " + ex.Message, ex);
            }
        }

        public async Task<List<Aventureiro>> ObterAventureirosDaGuildaAsync(int guildaId)
        {
            try
            {
                return await _guildaDAO.ConsultarAventureirosDaGuildaAsync(guildaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter aventureiros da guilda: " + ex.Message, ex);
            }
        }

        public async Task<List<Missao>> ConsultarMissoesAsync(int guildaId)
        {
            try
            {
                return await _guildaDAO.ConsultarMissoesDaGuilda(guildaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar missões da guilda: " + ex.Message, ex);
            }
        }
    }
}