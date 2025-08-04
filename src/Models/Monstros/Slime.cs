namespace RPGConsole.Models.Monstros;

using RPGConsole.Models.Itens;

public class Slime : Monstro
{
    public Slime() : base(
        nomeBase: "Slime",
        vidaBase: 30,
        ataqueBase: 3
    )
    {
        LootPossivel.Add((
            fabrica: () => new PocaoVida(),
            chance: 70,
            minQtd: 1,
            maxQtd: 2
        ));
    }
}
