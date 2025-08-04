namespace RPGConsole.Models.NPCs.Mercadores;

using RPGConsole.Models.Itens;

public class Binda : Mercador
{
    public Binda() : base("Binda")
    {
        // 🔹 Itens iniciais à venda com preço em moedas
        ItensVenda.Add(new ItemVenda(new PocaoVida(), 15));          // Poção de vida por 15 moedas
        ItensVenda.Add(new ItemVenda(new EspadaDeFerro(), 50));      // Espada de ferro por 50 moedas
        ItensVenda.Add(new ItemVenda(new EscudoDeMadeira(), 40));    // Escudo de madeira por 40 moedas
        ItensVenda.Add(new ItemVenda(new ArmaduraDeCouro(), 75));    // Armadura de couro por 75 moedas
    }
}
