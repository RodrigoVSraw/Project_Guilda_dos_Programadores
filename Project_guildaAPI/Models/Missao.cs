using System;
using System.Collections.Generic;
using System.Linq;

namespace Missoes.Models
{
    public class Missao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal OuroRecompensa { get; set; } 
        public int ExperienciaRecompensa { get; set; }

        public int NivelRecomendado { get; set; }

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
        }
    }
}
