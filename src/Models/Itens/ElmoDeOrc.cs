namespace RPGConsole.Models.Itens;

public class ElmoDeOrc : Item
{
    public int AtaqueExtra { get; private set; } = 10;

    public ElmoDeOrc() 
        : base(
            nome: "Elmo de Orc",
            consumivel: false,
            empilhavel: false,
            quantidade: 1,
            raridade: Raridade.Raro
        ) { }

    public override void Usar(Jogador jogador)
    {
        // Nada ainda ..
        Console.WriteLine($"🪓 {jogador.Nome} equipa {Nome}, mas ainda não tem efeito programado!");
    }
}
