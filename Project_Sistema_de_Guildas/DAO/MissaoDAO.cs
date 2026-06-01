using Npgsql;
using System;
using SistemaDeGuildas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuilderConnections.DAO;

namespace MissoesDATA.DAO
{
    public class MissaoDAO
    {
        public async Task CriarMissaoAsync(Missao missao)
        {
            if (missao == null)
                throw new ArgumentNullException(nameof(missao), "A missão não pode ser nula.");
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"INSERT INTO missoes (nome, descricao, ouroRecompensa, experienciaRecompensa, nivelRecomendado)
                                VALUES(@nome, @descricao, @ourorecompensa, @experienciarecompensa, @nivelrecomendado) RETURNING id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("nome", missao.Nome);
                        cmd.Parameters.AddWithValue("descricao", missao.Descricao);
                        cmd.Parameters.AddWithValue("ourorecompensa", missao.OuroRecompensa);
                        cmd.Parameters.AddWithValue("experienciarecompensa", missao.ExperienciaRecompensa);
                        cmd.Parameters.AddWithValue("nivelrecomendado", missao.NivelRecomendado);

                        var resultado = await cmd.ExecuteScalarAsync();
                        if (resultado == null)
                            throw new Exception("Falha ao criar a missão. Nenhum ID retornado.");
                        missao.Id = Convert.ToInt32(resultado);
                    }

                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar a missão: " + ex.Message, ex);
            }
        }
        public async Task<List<Missao>> VerTodasAsMissoesAsync()
        {
            var missoes = new List<Missao>();
            try
            {
                await using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT id, nome, descricao, ouroRecompensa, experienciaRecompensa, nivelRecomendado FROM missoes";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var missao = new Missao(

                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetDecimal(3),
                                    reader.GetInt32(4),
                                    reader.GetInt32(5)
                                );
                                missao.Id = reader.GetInt32(0);
                                missoes.Add(missao);
                            }
                        }
                    }
                }
                return missoes;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
        public async Task<Missao> VerMissaoPorIdAsync(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"SELECT id, nome, descricao, ouroRecompensa, experienciaRecompensa, nivelRecomendado FROM missoes WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var missao = new Missao
                                    (
                                        reader.GetString(1),
                                        reader.GetString(2),
                                        reader.GetDecimal(3),
                                        reader.GetInt32(4),
                                        reader.GetInt32(5)
                                    );
                                missao.Id = reader.GetInt32(0);
                                return missao;
                            }
                            else
                            {
                                throw new Exception($"Missão com ID {id} não encontrada.");
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
        public async Task<List<Missao>> MostrarMissaoDoAventureiro(int id)
        {
            var missoesAventureiro = new List<Missao>();
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"SELECT id, nome, descricao, ouroRecompensa, experienciaRecompensa, nivelRecomendado FROM missoes WHERE id_aventureiro = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var missao = new Missao
                                (
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetDecimal(3),
                                    reader.GetInt32(4),
                                    reader.GetInt32(5)
                                );
                            missao.Id = reader.GetInt32(0);
                            missoesAventureiro.Add(missao);
                        }
                    }
                }
                return missoesAventureiro;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }


        }
        public async Task<List<Missao>> MostrarMissaoDaGuilda(int id)
        {
            var missoesGuilda = new List<Missao>();
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"SELECT id, nome, descricao, ouroRecompensa, experienciaRecompensa, nivelRecomendado FROM missoes WHERE id_guilda = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var missao = new Missao
                                (
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetDecimal(3),
                                    reader.GetInt32(4),
                                    reader.GetInt32(5)
                                );
                            missao.Id = reader.GetInt32(0);
                            missoesGuilda.Add(missao);
                        }
                    }
                }
                return missoesGuilda;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
        public async Task AlterarDescricaoDaMissao(int id, string novaDescricao)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"UPDATE missoes SET descricao = @descricao WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@descricao", novaDescricao);
                        cmd.Parameters.AddWithValue("@id", id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }

        public async Task AlterarRecompensaDaMissao(int id, decimal novoOuro, int novaExperiencia)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    
                    string sql = @"UPDATE missoes SET ouro_recompensa = @ouroRecompensa, experiencia_recompensa = @experienciaRecompensa WHERE id = @id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ouroRecompensa", novoOuro);
                        cmd.Parameters.AddWithValue("@experienciaRecompensa", novaExperiencia);
                        cmd.Parameters.AddWithValue("@id", id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Erro ao acessar o banco de dados: " + ex.Message, ex);
            }
        }
        public async Task ExcluirMissao(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"DELETE FROM missoes WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        await cmd.ExecuteNonQueryAsync();
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
