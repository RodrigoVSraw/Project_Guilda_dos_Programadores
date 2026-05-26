namespace ItemM.Models
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

        public Item(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque)
        {
            Id = id;
            Nome = nome;
            NivelRequerido = nivelRequerido;
            Preco = preco;
            ClasseRequerida = classeRequerida;
            Estoque = estoque;
        }
    }

    public class Equipamento : Item
    {
        public string TipoDeEquipamento { get; set; }

        public int Atributos { get; set; }

        protected Equipamento() { }

        public Equipamento(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string tipoDeEquipamento, int atributos)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque)
        {
            TipoDeEquipamento = tipoDeEquipamento;
            Atributos = atributos;
        }

    }

    public class Consumivel : Item
    {
        public string Efeito { get; set; }

        public int Duracao { get; set; }

        protected Consumivel() { }

        public Consumivel(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string efeito, int duracao)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque)
        {
            Efeito = efeito;
            Duracao = duracao;
        }
    }

    public class Material : Item
    {
        public string TipoDeMaterial { get; set; }

        protected Material() { }

        public Material(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string tipoDeMaterial)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque)
        {
            TipoDeMaterial = tipoDeMaterial;
        }
    }

    public class Habilidade : Item
    {
        public string Poder { get; set; }

        protected Habilidade() { }

        public Habilidade(string id, string nome, int nivelRequerido, decimal preco, string classeRequerida, int estoque, string poder)
            : base(id, nome, nivelRequerido, preco, classeRequerida, estoque)
        {
            Poder = poder;
        }
    }
}
