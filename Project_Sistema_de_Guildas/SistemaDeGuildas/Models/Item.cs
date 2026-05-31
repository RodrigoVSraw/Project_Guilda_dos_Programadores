using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeGuildas.Models
{
    public abstract class Item
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public int NivelRequerido { get; set; }

        public decimal Preco { get; set; }

        public string ClasseRequerida { get; set; }

        public int Estoque { get; set; }

        public string Descricao { get; set; }

        protected Item() { }

        public Item(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string descricao)
        {
            if(string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O Nome do item não pode ser nulo ou vazio.", nameof(nome));

            if(preco < 0)
                throw new ArgumentException("O Preço do item não pode ser negativo.", nameof(preco));

            if(estoque < 0)
                throw new ArgumentException("O Estoque do item não pode ser negativo.", nameof(estoque));

            if(string.IsNullOrWhiteSpace(classeRequerida))
                throw new ArgumentException("A Classe Requerida do item não pode ser nula ou vazia.", nameof(classeRequerida));

            if(string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A Descrição do item não pode ser nula ou vazia.", nameof(descricao));

            Id = id;
            Nome = nome;
            NivelRequerido = nivelRequerido;
            Preco = preco;
            ClasseRequerida = classeRequerida;
            Estoque = estoque;
            Descricao = descricao;
        }
    }

    public class Equipamento : Item
    {
        public string TipoDeEquipamento { get; set; }

        public int Atributos { get; set; }

        protected Equipamento() { }

        public Equipamento(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string descricao, string tipoDeEquipamento, int atributos)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque, descricao)
        {
            if (string.IsNullOrWhiteSpace(tipoDeEquipamento))
                throw new ArgumentException("O Tipo de Equipamento do item não pode ser nulo ou vazio.", nameof(tipoDeEquipamento));

            if(tipoDeEquipamento != "Armadura" && tipoDeEquipamento != "Arma")
                throw new ArgumentException("O Tipo de Equipamento deve ser 'Armadura' ou 'Arma'.", nameof(tipoDeEquipamento));

            if (atributos <= 0)
                throw new ArgumentException("Os Atributos do equipamento devem ser maiores que zero.", nameof(atributos));

            TipoDeEquipamento = tipoDeEquipamento;
            Atributos = atributos;
        }

    }

    public class Consumivel : Item
    {
        public string Efeito { get; set; }

        public int Duracao { get; set; }

        protected Consumivel() { }

        public Consumivel(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string descricao, string efeito, int duracao)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque, descricao)
        {
            if(string.IsNullOrWhiteSpace(efeito))
                throw new ArgumentException("O Efeito do item não pode ser nulo ou vazio.", nameof(efeito));

            if (duracao <= 0)
                throw new ArgumentException("A Duração do efeito do consumível deve ser maior que zero.", nameof(duracao));

            Efeito = efeito;
            Duracao = duracao;
        }
    }

    public class Material : Item
    {
        public string TipoDeMaterial { get; set; }

        protected Material() { }

        public Material(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string descricao, string tipoDeMaterial)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque, descricao)
        {
            if (string.IsNullOrWhiteSpace(tipoDeMaterial))
                throw new ArgumentException("O Tipo de Material do item não pode ser nulo ou vazio.", nameof(tipoDeMaterial));

            if(tipoDeMaterial != "Comum" && tipoDeMaterial != "Raro" && tipoDeMaterial != "Épico" && tipoDeMaterial != "Lendário")
                throw new ArgumentException("O Tipo de Material deve ser 'Comum', 'Raro', 'Épico', ou 'Lendário'.", nameof(tipoDeMaterial));

            TipoDeMaterial = tipoDeMaterial;
        }
    }

    public class Habilidade : Item
    {
        public string TipoDeHabilidade { get; set; }

        public string EfeitoDeHabilidade { get; set; }
        protected Habilidade() { }

        public Habilidade(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string descricao, string tipoDeHabilidade, string efeitoDeHabilidade)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque, descricao)
        {
            if(string.IsNullOrWhiteSpace(tipoDeHabilidade))
                throw new ArgumentException("O Poder da habilidade não pode ser nulo ou vazio.", nameof(tipoDeHabilidade));

            if (tipoDeHabilidade != "Ataque" && tipoDeHabilidade != "Defesa" && tipoDeHabilidade != "Suporte")
                throw new ArgumentException("O Poder da habilidade deve ser 'Ataque', 'Defesa', ou 'Suporte'.", nameof(tipoDeHabilidade));

            if(string.IsNullOrWhiteSpace(efeitoDeHabilidade))
                throw new ArgumentException("O Efeito da habilidade não pode ser nulo ou vazio.", nameof(efeitoDeHabilidade));

            EfeitoDeHabilidade = efeitoDeHabilidade;
            TipoDeHabilidade = tipoDeHabilidade;
        }
    }
}
