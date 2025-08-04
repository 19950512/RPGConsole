namespace RPGConsole.Models.Monstros;

using RPGConsole.Models.Itens;

public class Goblin : Monstro
{
    public Goblin() : base(
        nomeBase: "Goblin",
        vidaBase: 50,
        ataqueBase: 4
    )
    {
        LootPossivel.Add((
            fabrica: () => new PocaoVida(),
            chance: 50,
            minQtd: 1,
            maxQtd: 2
        ));
    }
}
