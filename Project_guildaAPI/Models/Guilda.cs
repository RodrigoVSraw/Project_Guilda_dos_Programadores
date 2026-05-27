using Aventureiros.Models; 
using Missoes.Models;

namespace Guildas.Models
{
    public class Guilda
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }  
        public float ExperienciaGuilda { get; set; }
        public int NivelRequerido { get; set; }
        public string Descricao { get; set; }
        public List<Aventureiro> Membros { get; set; } = new List<Aventureiro>();
        public List<Missao> MissoesDisponiveis { get; set; } = new List<Missao>();

        protected Guilda() { }

        public Guilda(int id, string nome, int nivel, int nivelRequerido, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O Nome da guilda não pode ser nulo ou vazio.", nameof(nome));

            if (nivel < 1)
                throw new ArgumentException("O Nível da guilda deve ser pelo menos 1.", nameof(nivel));

            if (nivelRequerido < 1)
                throw new ArgumentException("O Nível Requerido da guilda deve ser pelo menos 1.", nameof(nivelRequerido));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A Descrição da guilda não pode ser nula ou vazia.", nameof(descricao));

            Id = id;
            Nome = nome;
            Nivel = nivel;
            NivelRequerido = nivelRequerido;
            Descricao = descricao;
            ExperienciaGuilda = 0;  
        }

        public void AdicionarMembro(Aventureiro aventureiro)
        {
            if (aventureiro == null)
                throw new ArgumentNullException(nameof(aventureiro), "O aventureiro não pode ser nulo.");
            if (aventureiro.Nivel < NivelRequerido)
                throw new ArgumentException($"O aventureiro deve ser pelo menos nível {NivelRequerido} para ingressar na guilda.", nameof(aventureiro));

            Membros.Add(aventureiro);
        }

        public void AdicionarMissao(Missao missao)
        {
            if (missao == null)
                throw new ArgumentNullException(nameof(missao), "A missão não pode ser nula.");

            MissoesDisponiveis.Add(missao);
        }

        public void RemoverMembro(Aventureiro aventureiro)
        {
            if (aventureiro == null)
                throw new ArgumentNullException(nameof(aventureiro), "O aventureiro não pode ser nulo.");
            Membros.Remove(aventureiro);
        }

        public void RemoverMissao(Missao missao)
        {
            if (missao == null)
                throw new ArgumentNullException(nameof(missao), "A missão não pode ser nula.");
            MissoesDisponiveis.Remove(missao);
        }

        public void GanharExperiencia(float xpReceido)
        {
            ExperienciaGuilda += xpReceido;
            VerificarNivelUp();
        }

        public void VerificarNivelUp()
        {
            float xpNecessaria = Nivel * 1000;

            while (ExperienciaGuilda >= xpNecessaria)
            {
                Nivel++;
                ExperienciaGuilda -= xpNecessaria;
                

                xpNecessaria = Nivel * 1000;
            }
        }
    }
}
