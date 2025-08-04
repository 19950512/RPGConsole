namespace RPGConsole.Models.Itens;

public class ColarDaAmazonas : Item
{
    public int AtaqueExtra { get; private set; } = 10;

    public ColarDaAmazonas() 
        : base(
            nome: "Colar da Amazonas",
            consumivel: false,
            empilhavel: false,
            quantidade: 1,
            raridade: Raridade.Comum
        ) { }

    public override void Usar(Jogador jogador)
    {
        // Nada ainda ..
    }
}
