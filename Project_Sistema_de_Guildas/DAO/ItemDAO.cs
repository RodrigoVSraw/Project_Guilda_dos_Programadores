using Npgsql;
using SistemaDeGuildas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BuilderConnections.DAO;
using System.Security.Cryptography;
using System.Data;


namespace ItensDATA.DAO
{
    public class ItemDAO
    {
        public async Task CriarItemAsync(Item item)
        {
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"INSERT INTO itens (nome, nivel_requerido, preco, classe_requerida, estoque, descricao)
                                   VALUES (@nome, @nivel_requerido, @preco, @classe_requerida, @estoque, @descricao)";   
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("nome", item.Nome);
                        cmd.Parameters.AddWithValue("nivel_requerido", item.NivelRequerido);
                        cmd.Parameters.AddWithValue("preco", item.Preco);
                        cmd.Parameters.AddWithValue("classe_requerida", item.ClasseRequerida);
                        cmd.Parameters.AddWithValue("estoque", item.Estoque);
                        cmd.Parameters.AddWithValue("descricao", item.Descricao);
                        var resultado = await cmd.ExecuteScalarAsync();
                        if(resultado == null)
                            throw new Exception("Erro ao criar item");
                        item.Id = Convert.ToInt32(resultado);
                    }
                }
            }catch (NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao adicionar item no banco de dados: {ex.Message}", ex);
            }
        }
        public async Task VisualizarItensEmEstoqueAsync()
        {
            var listaItens = new List<Item>();
            try
            {
                using (var conn = new NpgsqlConnection(BuilderConnection.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    string sql = @"SELECT id, nome, nivel_requerido, preco, classe_requerida, estoque, descricao 
                                   FROM itens WHERE estoque > 0";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                
                                
                                
                            }
                        }
                    }
                    
                        
                       
                    
                    
                }
            }catch(NpgsqlException ex)
            {
                throw new InvalidOperationException($"Erro ao visualizar itens em estoque: {ex.Message}", ex);
            }
        }
    }
}
