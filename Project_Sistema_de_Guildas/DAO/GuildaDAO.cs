using Npgsql;
using SistemaDeGuildas.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuilderConnections.DAO;

namespace GuildasDATA.DAO
{
    public class GuildaDAO
    {

        public async Task<List<Guilda>> BuscarGuildasAsync()
        {
            var listaGuildas = new List<Guilda>();

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT id, nome, nivel, experiencia, nivelRequerido, descricao FROM guildas";  

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
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO guildas (nome, nivel, experiencia, nivelRequerido, descricao) VALUES (@nome, @nivel, @experiencia_da_guilda, @nivel_requerido, @descricao) RETURNING id"; 

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", guilda.Nome);
                        cmd.Parameters.AddWithValue("@nivel", guilda.Nivel);
                        cmd.Parameters.AddWithValue("@experiencia", guilda.ExperienciaGuilda);
                        cmd.Parameters.AddWithValue("@nivelrequerido", guilda.NivelRequerido);
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
        public async Task<Guilda> BuscarGuildaPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("O ID da guilda deve ser um número positivo.");

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT id, nome, nivel, experiencia, nivelRequerido, descricao FROM guildas WHERE id = @id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
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
                                return guilda;
                            }

                            return null;
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
        public async Task AtualizarNivelMinimoDaGuildaAsync(int id, int novoNivelRequerido)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("O ID da guilda deve ser um número positivo!");

            if (novoNivelRequerido <= 0)
                throw new ArgumentOutOfRangeException("O nível mínimo de uma guilda de ser pelo menos 1");

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "UPDATE guildas SET nivelRequerido = @nivelRequerido WHERE id = @id_guildas";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nivelRequerido", novoNivelRequerido);
                        cmd.Parameters.AddWithValue("@id_guilda", id);
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Nenhuma guilda encontrada com o ID fornecido.");
                        }
                    }

                }

                if (novoNivelRequerido <= 0)
                    throw new ArgumentOutOfRangeException("O nível mínimo de uma guilda de ser pelo menos 1");

                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "UPDATE guildas SET nivelrequerido = @nivelrequerido WHERE id = @id_guildas";

                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao tentar atualizar o nível mínimo da guilda: {ex.Message}");
            }


        }
        public async Task ExcluirGuildaPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID da guilda deve ser um número positivo.", nameof(id));
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "DELETE FROM guildas WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Nenhuma guilda encontrada com o ID fornecido.");
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao excluir a guilda: {ex.Message}", ex);
            }
        }
        public async Task<List<Aventureiro>> ConsultarAventureirosDaGuildaAsync(int guildaId)
        {
            var listaAventureiros = new List<Aventureiro>();

            if (guildaId <= 0)
                throw new ArgumentOutOfRangeException("O ID da guilda deve ser um número positivo.", nameof(guildaId));

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT av.id, av.nome, av.cargo, av.classe_de_combate, av.nivel FROM aventureiros av INNER JOIN guildas g ON av.id_guilda = g.id WHERE g.id = @guildaId";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@guildaId", guildaId);

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
                                        throw new InvalidOperationException($"Classe de combate desconhecida no banco de dados: {classeCombate}");
                                }

                                aventureiro.Id = reader.GetInt32(0);
                                aventureiro.Nome = nome;
                                aventureiro.Cargo = cargo;
                                aventureiro.ClasseDeCombate = classeCombate;
                                aventureiro.Nivel = reader.GetInt32(4);

                                listaAventureiros.Add(aventureiro);
                            }
                        }
                    }
                    return listaAventureiros;
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao consultar os aventureiros da guilda: {ex.Message}", ex);
            }
        }

        public async Task<List<Missao>> ConsultarMissoesDaGuilda(int id_guilda)
        {
            var listaMissoes = new List<Missao>();
            if(id_guilda <= 0)
                throw new ArgumentOutOfRangeException("O ID da guilda deve ser um número positivo.", nameof(id_guilda));
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT id, nome, descricao, ouro_recompensa, experiencia_recompensa, nivel_recomendado FROM missoes WHERE guilda_responsavel_id = @id_guildas";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_guilda", id_guilda);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var missao = new Missao(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetDecimal(3),
                                    reader.GetInt32(4),
                                    reader.GetInt32(5)
                                );
                                listaMissoes.Add(missao);
                            }
                        }
                    }
                    return listaMissoes;


                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao consultar as missões da guilda: {ex.Message}", ex);
            }
                
        }
    }
}
