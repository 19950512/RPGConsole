namespace RPGConsole.Models.Itens;

public class Moeda : Item
{

    public Moeda(int quantidade = 1)
        : base(
            nome: "Moeda",
            consumivel: false,
            empilhavel: false,
            quantidade: quantidade,
            raridade: Raridade.Comum
        )
    { }

    public override void Usar(Jogador jogador)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"💰 {jogador.Nome} coleta {Nome}!");
        Console.ResetColor();

        jogador.Moedas++;
    }
}
