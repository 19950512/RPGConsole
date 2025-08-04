namespace RPGConsole.Models;

public enum Raridade
{
    Comum,
    Incomum,
    Raro,
    Épico,
    Lendário
}

public abstract class Item
{
    public string Nome { get; }
    public bool Consumivel { get; }
    public bool Empilhavel { get; }
    public int Quantidade { get; set; } = 1;
    public Raridade Raridade { get; }

    protected Item(string nome, bool consumivel, bool empilhavel, int quantidade, Raridade raridade)
    {
        Nome = nome;
        Consumivel = consumivel;
        Empilhavel = empilhavel;
        Quantidade = quantidade;
        Raridade = raridade;
    }

    public abstract void Usar(Jogador jogador);

    // 🔹 Usar 1 unidade do item
    public void Consumir(Jogador jogador)
    {
        Usar(jogador);

        if (Consumivel && Quantidade > 0)
        {
            Quantidade--;
        }
    }

    // 🔹 Aumentar pilha de itens iguais
    public void AdicionarQuantidade(int quantidade)
    {
        Quantidade += quantidade;
    }

    public void RemoverQuantidade(int quantidade)
    {
        Quantidade -= quantidade;
    }

    public ConsoleColor GetColor()
    {
        return Raridade switch
        {
            Raridade.Comum => ConsoleColor.Gray,
            Raridade.Incomum => ConsoleColor.Green,
            Raridade.Raro => ConsoleColor.Blue,
            Raridade.Épico => ConsoleColor.Magenta,
            Raridade.Lendário => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };
    }

    public Item Clone()
    {
        return (Item) MemberwiseClone();
    }
}

