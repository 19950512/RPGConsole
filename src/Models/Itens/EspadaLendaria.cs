namespace RPGConsole.Models.Itens;

public class EspadaLendaria : Item
{
    public int AtaqueExtra { get; private set; } = 10;

    public EspadaLendaria() 
        : base(
            nome: "Espada Lendária",
            consumivel: false,
            empilhavel: false,
            quantidade: 1,
            raridade: Raridade.Lendário
        ) { }

    public override void Usar(Jogador jogador)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚔ {jogador.Nome} equipa {Nome} e ganha +{AtaqueExtra} de ataque!");
        Console.ResetColor();

        jogador.Vocacao.AtaqueExtraPorItens += AtaqueExtra;
    }
}
