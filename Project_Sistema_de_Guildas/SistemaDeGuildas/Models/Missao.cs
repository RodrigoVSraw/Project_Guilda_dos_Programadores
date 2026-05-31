using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaDeGuildas.Models
{
    public class Missao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal OuroRecompensa { get; set; }
        public float ExperienciaRecompensa { get; set; }

        public int NivelRecomendado { get; set; }

        public Aventureiro AventureiroResponsavel { get; set; }

        public Guilda GuildaResponsavel { get; set; }

        protected Missao() { }

        public Missao(int id, string nome, string descricao, decimal ouroRecompensa, int experienciaRecompensa, int nivelRecomendado)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O Nome da missão não pode ser nulo ou vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A Descrição da missão não pode ser nula ou vazia.", nameof(descricao));

            if (ouroRecompensa < 0)
                throw new ArgumentException("A Recompensa em Ouro da missão não pode ser negativa.", nameof(ouroRecompensa));

            if (experienciaRecompensa < 0)
                throw new ArgumentException("A Recompensa em Experiência da missão não pode ser negativa.", nameof(experienciaRecompensa));

            Id = id;
            Nome = nome;
            Descricao = descricao;
            OuroRecompensa = ouroRecompensa;
            ExperienciaRecompensa = experienciaRecompensa;
            NivelRecomendado = nivelRecomendado;
            GuildaResponsavel = null;
            AventureiroResponsavel = null;
        }

        public decimal CalcularOuroTotal(Guilda guildaDoAventureiro)
        {

            if (guildaDoAventureiro == null)
            {
                return OuroRecompensa;
            }

            decimal recompensaBonusGuilda = 1.0m + (guildaDoAventureiro.Nivel * 0.05m);
            return recompensaBonusGuilda * OuroRecompensa;
        }

        public float CalcularExperienciaTotal(Guilda guildaDoAventureiro)
        {
            if (guildaDoAventureiro == null)
            {
                return ExperienciaRecompensa;
            }

            float recompensaBonusGuilda = 1.0f + (guildaDoAventureiro.Nivel * 0.10f);
            return recompensaBonusGuilda * ExperienciaRecompensa;
        }

        public void AtribuirResponsavel(Aventureiro aventureiro)
        {
            if (AventureiroResponsavel != null)
                throw new ArgumentException("Essa missão já foi atribuida a um Aventureiro");

            if(GuildaResponsavel != null)
                throw new ArgumentException("Essa missão já foi atribuida a uma Guilda");

            AventureiroResponsavel = aventureiro;
        }

        public void AtribuirGuildaResponsavel(Guilda guilda)
        {
            if (GuildaResponsavel != null)
                throw new ArgumentException("Essa missão já foi atribuida a uma Guilda");

            if (AventureiroResponsavel != null)
                throw new ArgumentException("Essa missão já foi atribuida a um Aventureiro");

            GuildaResponsavel = guilda;
        }
    }
}
