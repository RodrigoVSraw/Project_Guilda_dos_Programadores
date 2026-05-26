using GuildaM.Models;

namespace AventureiroM.Models
{
    public abstract class Aventureiro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public float Experiencia { get; set; }
        public Guilda Guilda { get; set; }
        public int Vida { get; set; }   
        public int Forca { get; set; }  
        public int Mana { get; set; }
        public int Energia { get; set; }
        public string HabilidadeEspecial { get; set; }

        protected Aventureiro() { }
        public Aventureiro(int id, string nome, int nivel, Guilda guilda)
        {
            if(string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O Nome do aventureiro não pode ser nulo ou vazio.", nameof(nome));

            if(nivel < 1)
                throw new ArgumentException("O Nível do aventureiro deve ser pelo menos 1.", nameof(nivel));

            if (guilda != null && nivel < guilda.NivelRequerido)
                throw new ArgumentException($"O Nível do aventureiro deve ser pelo menos {guilda.NivelRequerido} para ingressar na guilda {guilda.Nome}.", nameof(nivel));

            Id = id;
            Nome = nome;
            Nivel = nivel;  
            Guilda = guilda;
            Experiencia = 0;
        }

        public void GanharExperiencia(float xpReceido)
        {
            Experiencia += xpReceido;
            VerificarNivelUp();
        }

        public void VerificarNivelUp()
        {
            float xpNecessaria = Nivel * 100; 

            while (Experiencia >= xpNecessaria)
            {
                Nivel++;
                Experiencia -= xpNecessaria; 
                AumentarAtributosNivelUp();

                xpNecessaria = Nivel * 100;
            }
        }

        protected abstract void AumentarAtributosNivelUp();

    }

    public class Guerreiro : Aventureiro
    {

        public const int VidaInicialBase = 565;

        public const int ForcaInicialBase = 130;

        public const int ManaInicialBase = 20;

        public const int EnergiaInicialBase = 90;

        protected Guerreiro() { }
        public Guerreiro(string nome, Guilda guilda = null)
            : base(0, nome, 1, guilda)
        {
            Forca = ForcaInicialBase;
            Vida = VidaInicialBase;
            Mana = ManaInicialBase;
            Energia = EnergiaInicialBase;
            HabilidadeEspecial = "Fúria do Berserker";
        }

        protected override void AumentarAtributosNivelUp()
        {
            Forca += 25;
            Vida += 50;
            Mana += 5;
            Energia += 10;
        }

    }

    public class Mago : Aventureiro
    {
        public const int VidaInicialBase = 300;

        public const int ForcaInicialBase = 10;

        public const int ManaInicialBase = 250;

        public const int EnergiaInicialBase = 50;

        protected Mago() { }

        public Mago(string nome, Guilda guilda = null)
            : base(0, nome, 1, guilda)
        {
            Forca = ForcaInicialBase;
            Vida = VidaInicialBase;
            Mana = ManaInicialBase;
            Energia = EnergiaInicialBase;
            HabilidadeEspecial = "Mana ilimitada";
        }

        protected override void AumentarAtributosNivelUp()
        {
            Forca += 5;
            Vida += 30;
            Mana += 30;
            Energia += 5;
        }
    }

    public class Arqueiro : Aventureiro
    {
        public const int VidaInicialBase = 360;

        public const int ForcaInicialBase = 76;

        public const int ManaInicialBase = 40;

        public const int EnergiaInicialBase = 120;

        protected Arqueiro() { }

        public Arqueiro(string nome, Guilda guilda = null)
            : base(0, nome, 1, guilda)
        {
            Forca = ForcaInicialBase;
            Vida = VidaInicialBase;
            Mana = ManaInicialBase;
            Energia = EnergiaInicialBase;
            HabilidadeEspecial = "Aljava Especial";
        }

        protected override void AumentarAtributosNivelUp()
        {
            Forca += 20;
            Vida += 25;
            Mana += 10;
            Energia += 30;
        }
    }

    public class Curandeiro : Aventureiro
    {
        public const int VidaInicialBase = 250;

        public const int ForcaInicialBase = 20;

        public const int ManaInicialBase = 180;

        public const int EnergiaInicialBase = 65;

        protected Curandeiro() { }

        public Curandeiro(string nome, Guilda guilda = null)
            : base(0, nome, 1, guilda)
        {
            Forca = ForcaInicialBase;
            Vida = VidaInicialBase;
            Mana = ManaInicialBase;
            Energia = EnergiaInicialBase;
            HabilidadeEspecial = "Cura Divina";
        }

        protected override void AumentarAtributosNivelUp()
        {
            Forca += 4;
            Vida += 25;
            Mana += 15;
            Energia += 10;
        }
    }
}
