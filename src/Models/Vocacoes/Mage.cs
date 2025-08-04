namespace RPGConsole.Models.Vocacoes;

public class Mage : Vocacao
{
    public Mage() : base(
        nome: "Mage",
        ataqueBase: 25,
        defesaBase: 1,
        vidaBase: 100
    ) { }

    public override int UsarSkill()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("🔥 Mage lançou Fireball!");
        Console.ResetColor();
        return AtaqueBase + new Random().Next(10, 20);
    }

    public override void AoSubirDeNivel(Jogador jogador)
    {
        base.AoSubirDeNivel(jogador); // aplica o bônus genérico da vocação

        // Bônus extra do Mage
        AtaqueBase += 5;
        DefesaBase += 1;
        jogador.VidaMax += 8; // Exemplo de bônus adicional
        jogador.Vida = jogador.VidaMax; // Restaura vida ao subir de nível
    }
}
