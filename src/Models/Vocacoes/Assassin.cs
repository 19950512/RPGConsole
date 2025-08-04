namespace RPGConsole.Models.Vocacoes;

public class Assassin : Vocacao
{
    public Assassin() : base(
        nome: "Assassin",
        vidaBase: 120,
        ataqueBase: 12,
        defesaBase: 3
    )
    { }

    public override int UsarSkill()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("🗡️ Assassin executou Golpe Fatal!");
        Console.ResetColor();

        // 30% de chance de crítico triplo
        bool critico = new Random().Next(0, 100) < 30;
        return critico ? CalcularDano() * 3 : CalcularDano() * 2;
    }
    
    public override void AoSubirDeNivel(Jogador jogador)
    {
        base.AoSubirDeNivel(jogador); // aplica o bônus genérico da vocação

        // Bônus extra do Assassin
        AtaqueBase += 3;
        DefesaBase += 1;
        jogador.VidaMax += 5; // Exemplo de bônus adicional
        jogador.Vida = jogador.VidaMax; // Restaura vida ao subir de nível
    }
}
