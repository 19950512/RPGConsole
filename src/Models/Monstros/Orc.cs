namespace RPGConsole.Models.Monstros;

using RPGConsole.Models.Itens;

public class Orc : Monstro
{
    public Orc() : base(
        nomeBase: "Orc",
        vidaBase: 70,
        ataqueBase: 7
    )
    {
        LootPossivel.Add((
            fabrica: () => new ElmoDeOrc(),
            chance: 90,
            minQtd: 1,
            maxQtd: 1
        ));

        LootPossivel.Add((
            fabrica: () => new PocaoVida(),
            chance: 70,
            minQtd: 1,
            maxQtd: 4
        ));
    }
}
