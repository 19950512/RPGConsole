namespace RPGConsole.Models.Itens;

public abstract class Equipamento : Item
{
    public TipoEquipamento Tipo { get; }
    public int BonusAtaque { get; }
    public int BonusDefesa { get; }
    public int BonusVida { get; }

    protected Equipamento(
        string nome,
        TipoEquipamento tipo,
        int bonusAtaque = 0,
        int bonusDefesa = 0,
        int bonusVida = 0,
        Raridade raridade = Raridade.Comum
    ) : base(
        nome: nome,
        consumivel: false,
        empilhavel: false,
        quantidade: 1,
        raridade: raridade
    )
    {
        Tipo = tipo;
        BonusAtaque = bonusAtaque;
        BonusDefesa = bonusDefesa;
        BonusVida = bonusVida;
    }

    public override void Usar(Jogador jogador)
    {
        if (jogador.Equipar(this))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"🛡️ {jogador.Nome} equipou {Nome} (+{BonusDefesa} DEF, +{BonusAtaque} ATK, +{BonusVida} HP)!");
            Console.ResetColor();

            // Remove do inventário após equipar
            jogador.Inventario.Remove(this);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"❌ {Nome} não pôde ser equipado (slot ocupado).");
            Console.ResetColor();
        }
    }
}
