using Npgsql;
using SistemaDeGuildas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildasDATA.DAO
{
    public class GuildaDAO
    {
        private NpgsqlConnectionStringBuilder connBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "guilda_db",
            Username = "postgres",
            Password = "admin",

            SslMode = SslMode.Disable
        };

        public async Task<List<Guilda>> BuscarGuildasAsync()
        {
            var listaGuildas = new List<Guilda>();

            try
            {
                using (var conn = new NpgsqlConnection(connBuilder.ToString()))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT id, nome, nivel, experiencia_da_guilda, nivel_requerido, descricao FROM guildas";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var guilda = new Guilda(
                                    reader.GetString(1),
                                    reader.GetInt32(4),
                                    reader.GetString(5)
                                );

                                guilda.Id = reader.GetInt32(0);
                                guilda.Nivel = reader.GetInt32(2);
                                guilda.ExperienciaGuilda = reader.GetFloat(3);
                                guilda.Membros = new List<Aventureiro>();

                                listaGuildas.Add(guilda);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }

            return listaGuildas;
        }

        public async Task AdicionarGuildaAsync(Guilda guilda)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connBuilder.ToString()))
                {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO guildas (nome, nivel, experiencia_da_guilda, nivel_requerido, descricao) VALUES (@nome, @nivel, @experiencia_da_guilda, @nivel_requerido, @descricao) RETURNING id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", guilda.Nome);
                        cmd.Parameters.AddWithValue("@nivel", guilda.Nivel);
                        cmd.Parameters.AddWithValue("@experiencia_da_guilda", guilda.ExperienciaGuilda);
                        cmd.Parameters.AddWithValue("@nivel_requerido", guilda.NivelRequerido);
                        cmd.Parameters.AddWithValue("@descricao", guilda.Descricao);

                        guilda.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
    }
}
