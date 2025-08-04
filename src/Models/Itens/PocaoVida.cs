namespace RPGConsole.Models.Itens;

public class PocaoVida : Item
{
    public int Cura { get; private set; } = 30;

    public PocaoVida() 
        : base(
            nome: "Poção de Vida",
            consumivel: true,
            quantidade: 1,
            empilhavel: true,
            raridade: Raridade.Comum
        ) { }

    public override void Usar(Jogador jogador)
    {
        int vidaAntes = jogador.Vida;
        jogador.Vida = Math.Min(jogador.Vida + Cura, jogador.VidaMax);

        int recuperado = jogador.Vida - vidaAntes;
        if (recuperado > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"💖 {jogador.Nome} usou {Nome} e recuperou {recuperado} de HP!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"💔 {jogador.Nome} usou {Nome}, mas já estava com vida cheia!");
            Console.ResetColor();
        }
    }
}
