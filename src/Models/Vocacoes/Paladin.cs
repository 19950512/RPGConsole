namespace RPGConsole.Models.Vocacoes;

public class Paladin : Vocacao
{
    public Paladin() : base(
        nome: "Paladin",
        vidaBase: 140,
        ataqueBase: 7,
        defesaBase: 5
    )
    { }

    public override int UsarSkill()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("🏹 Paladin disparou uma Chuva de Flechas!");
        Console.ResetColor();
        return AtaqueBase * 2 + new Random().Next(0, 5);
    }
    
    public override void AoSubirDeNivel(Jogador jogador)
    {
        base.AoSubirDeNivel(jogador); // aplica o bônus genérico da vocação

        // Bônus extra do Paladin
        AtaqueBase += 2;
        DefesaBase += 3;
        jogador.VidaMax += 7; // Exemplo de bônus adicional
        jogador.Vida = jogador.VidaMax; // Restaura vida ao subir de nível
    }
}
