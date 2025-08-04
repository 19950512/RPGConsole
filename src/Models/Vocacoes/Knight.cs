namespace RPGConsole.Models.Vocacoes;

public class Knight : Vocacao
{
    public Knight() : base(
        nome: "Knight",
        vidaBase: 150,
        defesaBase: 20,
        ataqueBase: 5
    ) { }

    public override int UsarSkill()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("⚔️  Knight usou Golpe Poderoso!");
        Console.ResetColor();
        return CalcularDano() * 2;
    }

    public override void AoSubirDeNivel(Jogador jogador)
    {
        base.AoSubirDeNivel(jogador); // aplica o bônus genérico da vocação

        // Bônus extra do Knight
        DefesaBase += 2;
        jogador.VidaMax += 10; // Exemplo de bônus adicional
        jogador.Vida = jogador.VidaMax; // Restaura vida ao subir de nível
    }
}
