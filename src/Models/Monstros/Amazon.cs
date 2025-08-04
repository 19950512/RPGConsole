namespace RPGConsole.Models.Monstros;

using RPGConsole.Models.Itens;

public class Amazon : Monstro
{
    public Amazon() : base(
        nomeBase: "Amazon",
        vidaBase: 30,
        ataqueBase: 5
    )
    {
        LootPossivel.Add((
            fabrica: () => new ColarDaAmazonas(),
            chance: 75,
            minQtd: 1,
            maxQtd: 1
        ));

        LootPossivel.Add((
            fabrica: () => new PocaoVida(),
            chance: 50,
            minQtd: 1,
            maxQtd: 3
        ));
    }
}
