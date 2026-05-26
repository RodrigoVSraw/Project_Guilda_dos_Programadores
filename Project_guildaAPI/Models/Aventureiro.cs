using GuildaM.Models;

namespace Aventureiro.Models
{
    public abstract class Aventureiro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public float Vida { get; set; }
        public Guilda Guilda { get; set; }  

    }
}
