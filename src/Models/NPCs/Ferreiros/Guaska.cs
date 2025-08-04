namespace RPGConsole.Models.NPCs.Ferreiros;

public class Guaska : Ferreiro
{
    public Guaska() : base(
        nome: "Guaska"
    ) { }

    public override void Interagir(Jogador jogador)
    {

        // Exemplo de interação com o ferreiro
        Console.Clear();
        Console.WriteLine($"🔨 {Nome} - Ferreiro");

        Console.WriteLine("Você pode aprimorar seus equipamentos aqui, quando eu tiver os materiais necessários.");
        Console.WriteLine("Infelizmente, ainda não tenho nada para oferecer no momento.");

        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
    
}