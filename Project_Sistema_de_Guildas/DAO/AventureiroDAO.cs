using SistemaDeGuildas.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuilderConnections.DAO;

namespace AventureiroDATA.DAO
{
    public class AventureiroDAO
    {


        public async Task<List<Aventureiro>> BuscarAventureirosAsync()
        {
            var listaAventureiros = new List<Aventureiro>();

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"
                        SELECT 
                            id, nome, cargo, classe_de_combate, nivel, experiencia, 
                            vida, forca, mana, energia, ouro, habilidade_especial, id_guilda
                        FROM aventureiros";
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
                                        throw new InvalidOperationException($"Classe de combate desconhecida no banco de dados: {classeCombate}");
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
                                aventureiro.IdGuilda = reader.IsDBNull(12) ? null : reader.GetInt32(12);

                                listaAventureiros.Add(aventureiro);

                            }
                        }
                    }
                    return listaAventureiros;
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao buscar aventureiros no banco de dados: {ex.Message}", ex);
            }
        }

        public async Task AdicionarAventureiroAsync(Aventureiro aventureiro)
        {
            if (aventureiro == null)
                throw new ArgumentNullException(nameof(aventureiro), "O aventureiro não pode ser nulo.");

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = @"
                        INSERT INTO aventureiros (nome, cargo, classe_de_combate, nivel, experiencia, vida, forca, mana, energia, ouro, habilidade_especial) 
                        VALUES (@nome, @cargo, @classe_de_combate, @nivel, @experiencia, @vida, @forca, @mana, @energia, @ouro, @habilidade_especial) 
                        RETURNING id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", aventureiro.Nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@cargo", aventureiro.Cargo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@classe_de_combate", aventureiro.ClasseDeCombate ?? string.Empty);
                        cmd.Parameters.AddWithValue("@nivel", aventureiro.Nivel);
                        cmd.Parameters.AddWithValue("@experiencia", aventureiro.Experiencia);
                        cmd.Parameters.AddWithValue("@vida", aventureiro.Vida);
                        cmd.Parameters.AddWithValue("@forca", aventureiro.Forca);
                        cmd.Parameters.AddWithValue("@mana", aventureiro.Mana);
                        cmd.Parameters.AddWithValue("@energia", aventureiro.Energia);
                        cmd.Parameters.AddWithValue("@ouro", aventureiro.Ouro);
                        cmd.Parameters.AddWithValue("@habilidade_especial", aventureiro.HabilidadeEspecial ?? string.Empty);

                        var resultado = await cmd.ExecuteScalarAsync();

                        if (resultado == null)
                            throw new InvalidOperationException("Falha ao obter o ID do novo aventureiro. A inserção pode não ter funcionado.");

                        aventureiro.Id = Convert.ToInt32(resultado);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao adicionar aventureiro no banco de dados: {ex.Message}", ex);
            }
        }


        public async Task<Aventureiro> BuscarAventureiroPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID do aventureiro deve ser válido.", nameof(id));

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"
                    SELECT 
                    a.id, a.nome, a.cargo, a.classe_de_combate, a.nivel, 
                    a.experiencia, a.vida, a.forca, a.mana, a.energia, a.ouro, a.habilidade_especial,
                    g.id AS guilda_id, g.nome AS guilda_nome 
                    FROM aventureiros a
                    LEFT JOIN guildas g ON a.id_guilda = g.id
                    WHERE a.id = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
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
                                aventureiro.Cargo = cargo;
                                aventureiro.Nivel = reader.GetInt32(4);
                                aventureiro.Experiencia = reader.GetFloat(5);
                                aventureiro.Vida = reader.GetInt32(6);
                                aventureiro.Forca = reader.GetInt32(7);
                                aventureiro.Mana = reader.GetInt32(8);
                                aventureiro.Energia = reader.GetInt32(9);
                                aventureiro.Ouro = reader.GetDecimal(10);
                                aventureiro.HabilidadeEspecial = reader.GetString(11);
                                aventureiro.IdGuilda = reader.IsDBNull(12) ? null : reader.GetInt32(12);
                                if (aventureiro.IdGuilda != null)
                                {
                                    aventureiro.Guilda = new Guilda(
                                        reader.GetString(13), 
                                        1,
                                        "Descrição não carregada nesta tela."
                                    );
                                    aventureiro.Guilda.Id = aventureiro.IdGuilda.Value;
                                }
                                return aventureiro;
                            }
                            else
                            {
                                throw new KeyNotFoundException($"Aventureiro com ID {id} não encontrado no banco de dados.");
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao buscar aventureiro por ID no banco de dados: {ex.Message}", ex);
            }
        }
        public async Task EvoluirAventureiroAsync(Aventureiro aventureiro)
        {
            if (aventureiro == null)
                throw new ArgumentNullException(nameof(aventureiro), "O aventureiro não pode ser nulo.");

            if (aventureiro.Id <= 0)
                throw new ArgumentException("O ID do aventureiro deve ser válido.", nameof(aventureiro));

            try
            {
                // Verifica e aplica o level up (aumenta atributos se houver XP suficiente)
                aventureiro.VerificarNivelUp();

                // Atualiza o aventureiro no banco de dados com os novos atributos
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = @"
                        SELECT 
                            av.nivel, 
                            g.nivelRequerido
                        FROM aventureiros av, guildas g 
                        WHERE av.id = @id_aventureiro AND g.id = @id_guilda";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", aventureiro.Id);
                        cmd.Parameters.AddWithValue("@nivel", aventureiro.Nivel);
                        cmd.Parameters.AddWithValue("@experiencia", aventureiro.Experiencia);
                        cmd.Parameters.AddWithValue("@vida", aventureiro.Vida);
                        cmd.Parameters.AddWithValue("@forca", aventureiro.Forca);
                        cmd.Parameters.AddWithValue("@mana", aventureiro.Mana);
                        cmd.Parameters.AddWithValue("@energia", aventureiro.Energia);

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        if (linhasAfetadas == 0)
                            throw new KeyNotFoundException($"Aventureiro com ID {aventureiro.Id} não encontrado. Não foi possível evoluir.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao evoluir aventureiro no banco de dados: {ex.Message}", ex);
            }
        }
        public async Task EntrarEmUmaGuildaAsync(int idAventureiro, int idGuilda)
        {
            if (idAventureiro <= 0)
                throw new ArgumentException("O ID do aventureiro deve ser válido.", nameof(idAventureiro));

            if (idGuilda <= 0)
                throw new ArgumentException("O ID da guilda deve ser válido.", nameof(idGuilda));

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    // Verifica se o aventureiro já está em uma guilda
                    bool temGuilda = await VerificarStatusGuildaAsync(conn, idAventureiro);

                    if (temGuilda)
                        throw new InvalidOperationException("O aventureiro já está em uma guilda. Saia da guilda atual antes de entrar em outra.");

                    // Verifica se o aventureiro atende ao nível mínimo requerido
                    string sqlVerificarNivel = @"
                            SELECT 
                                av.nivel, 
                                g.nivelRequerido
                            FROM aventureiros av
                            INNER JOIN guildas g 
                                ON g.id = @id_guilda
                            WHERE av.id = @id_aventureiro";

                    using (var cmd = new NpgsqlCommand(sqlVerificarNivel, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_aventureiro", idAventureiro);
                        cmd.Parameters.AddWithValue("@id_guilda", idGuilda);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int nivelAventureiro = reader.GetInt32(0);
                                int nivelRequerido = reader.GetInt32(1);

                                if (nivelAventureiro < nivelRequerido)
                                    throw new InvalidOperationException($"O aventureiro precisa ser nível {nivelRequerido} ou superior para ingressar nesta guilda. Nível atual: {nivelAventureiro}");
                            }
                            else
                            {
                                throw new KeyNotFoundException("Aventureiro ou guilda não encontrados no banco de dados.");
                            }
                        }
                    }

                    // Atualiza o aventureiro para associá-lo à guilda
                    string sqlAtualizar = "UPDATE aventureiros SET id_guilda = @id_guilda WHERE id = @id_aventureiro";
                    using (var cmd = new NpgsqlCommand(sqlAtualizar, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_guilda", idGuilda);
                        cmd.Parameters.AddWithValue("@id_aventureiro", idAventureiro);

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        if (linhasAfetadas == 0)
                            throw new InvalidOperationException("Falha ao atualizar o aventureiro na guilda. O aventureiro pode ter sido removido.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao acessar o banco de dados durante entrada em guilda: {ex.Message}", ex);
            }
        }

        public async Task SairDaGuildaAsync(int idAventureiro)
        {
            if (idAventureiro <= 0)
                throw new ArgumentException("O ID do aventureiro deve ser válido.", nameof(idAventureiro));

            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    bool temGuilda = await VerificarStatusGuildaAsync(conn, idAventureiro);

                    if (!temGuilda)
                        throw new InvalidOperationException("O aventureiro não está em nenhuma guilda.");

                    string sqlAtualizar = "UPDATE aventureiros SET id_guilda = NULL WHERE id = @id_aventureiro";
                    using (var cmd = new NpgsqlCommand(sqlAtualizar, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_aventureiro", idAventureiro);

                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

                        if (linhasAfetadas == 0)
                            throw new InvalidOperationException("Falha ao remover o aventureiro da guilda. O aventureiro pode ter sido removido.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao acessar o banco de dados durante saída de guilda: {ex.Message}", ex);
            }
        }
        public async Task ExcluirAventureiroPorId(int idAventureiro)
        {
            if (idAventureiro <= 0)
                throw new ArgumentException("O ID do aventureiro deve ser válido.", nameof(idAventureiro));
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();

                    string sql = "DELETE FROM aventureiros WHERE id = @id_aventureiro";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_aventureiro", idAventureiro);
                        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                        if (linhasAfetadas == 0)
                            throw new KeyNotFoundException($"Aventureiro com ID {idAventureiro} não encontrado. Não foi possível excluir.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao excluir aventureiro no banco de dados: {ex.Message}", ex);
            }
        }

      
        private async Task<bool> VerificarStatusGuildaAsync(NpgsqlConnection conn, int idAventureiro)
        {
            string sql = "SELECT id_guilda FROM aventureiros WHERE id = @id_aventureiro";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_aventureiro", idAventureiro);
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado != null && resultado != DBNull.Value;
            }
        }
        public async Task NomearLiderDaGuildaAsync(int idAventureiro, int idGuilda)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = "UPDATE aventureiros SET id_guilda = @idGuilda, cargo = 'Líder' WHERE id = @idAventureiro";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@idGuilda", idGuilda);
                        cmd.Parameters.AddWithValue("@idAventureiro", idAventureiro);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao coroar o líder: {ex.Message}");
            }
        }
    }
}
