using SistemaDeGuildas.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aventureiros.DAO
{
    public class AventureiroDAO
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

        public async Task<List<Aventureiro>> BuscarAventureirosAsync()
        {
            var listaAventureiros = new List<Aventureiro>();

            try
            {
                using (var conn = new NpgsqlConnection(connBuilder.ToString()))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT id, nome, cargo, classe_de_combate, nivel, experiencia, vida, forca, mana, energia, ouro, habilidade_especial FROM aventureiros";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string nome = reader.GetString(1);
                                string cargo = reader.GetString(2);
                                string classeCombate = reader.GetString(3);

                                Aventureiro aventureiro;

                                switch (classeCombate.ToLower())
                                {
                                    case "guerreiro":
                                        aventureiro = new Guerreiro(nome);
                                        break;
                                    case "mago":
                                        aventureiro = new Mago(nome);
                                        break;
                                    case "arqueiro":
                                        aventureiro = new Arqueiro(nome);
                                        break;
                                    case "curandeiro":
                                        aventureiro = new Curandeiro(nome);
                                        break;
                                    default:

                                        throw new Exception($"Cargo desconhecido no banco de dados: {cargo}");
                                }

                                aventureiro.Id = reader.GetInt32(0);
                                aventureiro.Cargo = cargo;
                                aventureiro.Nivel = reader.GetInt32(4);
                                aventureiro.Experiencia = reader.GetFloat(5);
                                aventureiro.Vida = reader.GetInt32(6);
                                aventureiro.Forca = reader.GetInt32(7);
                                aventureiro.Mana = reader.GetInt32(8);
                                aventureiro.Energia = reader.GetInt32(9);
                                aventureiro.Ouro = reader.GetDecimal(10);
                                aventureiro.HabilidadeEspecial = reader.GetString(11);

                                listaAventureiros.Add(aventureiro);

                            }
                        }
                    }
                    return listaAventureiros;
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao buscar aventureiros: {ex.Message}");
            }
        }

        public async Task AdicionarAventureiroAsync(Aventureiro aventureiro)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connBuilder.ConnectionString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO aventureiros (nome, cargo, classe_de_combate, nivel, experiencia, vida, forca, mana, energia, ouro, habilidade_especial) " +
                                 "VALUES (@nome, @cargo, @classe_de_combate, @nivel, @experiencia, @vida, @forca, @mana, @energia, @ouro, @habilidade_especial) " +
                                 "RETURNING id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", aventureiro.Nome);
                        cmd.Parameters.AddWithValue("@cargo", aventureiro.Cargo);
                        cmd.Parameters.AddWithValue("@classe_de_combate", aventureiro.ClasseDeCombate);
                        cmd.Parameters.AddWithValue("@nivel", aventureiro.Nivel);
                        cmd.Parameters.AddWithValue("@experiencia", aventureiro.Experiencia);
                        cmd.Parameters.AddWithValue("@vida", aventureiro.Vida);
                        cmd.Parameters.AddWithValue("@forca", aventureiro.Forca);
                        cmd.Parameters.AddWithValue("@mana", aventureiro.Mana);
                        cmd.Parameters.AddWithValue("@energia", aventureiro.Energia);
                        cmd.Parameters.AddWithValue("@ouro", aventureiro.Ouro);
                        cmd.Parameters.AddWithValue("@habilidade_especial", aventureiro.HabilidadeEspecial);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Erro ao adicionar aventureiro: {ex.Message}");
            }
        }

    }
}
