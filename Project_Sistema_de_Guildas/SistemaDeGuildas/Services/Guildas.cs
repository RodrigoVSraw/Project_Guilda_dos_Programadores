using System;
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
                return await _guildaDAO.BuscarGuildasAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter guildas: " + ex.Message, ex);
            }
        }
    }
}
