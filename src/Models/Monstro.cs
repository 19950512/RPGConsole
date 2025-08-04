namespace RPGConsole.Models;

using RPGConsole.Models;
using RPGConsole.Services;
using RPGConsole.Models.Itens;


public abstract class Monstro : Combatente
{
    public string Nome { get; set; }
    public int VidaMax { get; protected set; }
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defesa { get; set; }
    public int Experiencia { get; protected set; } = 20;

    // (Item, chance %, minQtd, maxQtd)
    public List<(Func<Item> fabrica, int chance, int minQtd, int maxQtd)> LootPossivel { get; set; } = new();

    public bool Elite { get; private set; } = false;
    public bool Fugiu { get; private set; } = false;

    protected Monstro(string nomeBase, int vidaBase, int ataqueBase, int defesaBase = 0)
    {
        Random rand = new();

        // 🔹 Variação de status: -10% a +20%
        double modificadorVida = 0.9 + rand.NextDouble() * 0.3;
        VidaMax = (int)(vidaBase * modificadorVida);
        Vida = VidaMax;

        double modificadorAtk = 0.9 + rand.NextDouble() * 0.3;
        Ataque = Math.Max(1, (int)(ataqueBase * modificadorAtk));

        double modificadorDef = 0.9 + rand.NextDouble() * 0.3;
        Defesa = Math.Max(0, (int)(defesaBase * modificadorDef));

        // 🔹 Se for elite (> 115% do HP base)
        if (modificadorVida >= 1.15)
        {
            Elite = true;
            string[] prefixos = { "Forte", "Enfurecido", "Gigante", "Sombrio" };
            string prefixo = prefixos[rand.Next(prefixos.Length)];
            Nome = $"{nomeBase} {prefixo}";

            Experiencia = (int)(Experiencia * 1.5); // elites dão mais XP
            Ataque = (int)(Ataque * 1.2);          // elites batem mais
            Defesa = (int)(Defesa * 1.5);          // elites resistem mais
        }
        else
        {
            Nome = nomeBase;
        }

        // 🔹 Loot padrão
        LootPossivel.Add((() => new PocaoVida(), 50, 1, 2));
        LootPossivel.Add((() => new EspadaLendaria(), 30, 1, 1));
        LootPossivel.Add((() => new Moeda(rand.Next(3, 16)), 100, 1, 1));
    }

    public void ReceberDano(int dano)
    {
        int danoFinal = Math.Max(1, dano - Defesa);
        Vida -= danoFinal;
        if (Vida < 0) Vida = 0;
    }

    public bool EstaVivo() => Vida > 0;

    public List<Item> DroparLoot()
    {
        if (Fugiu) return new List<Item>();

        Random rand = new();
        List<Item> drops = new();

        foreach (var (fabrica, chance, minQtd, maxQtd) in LootPossivel)
        {
            if (rand.Next(0, 100) < chance)
            {
                int qtd = rand.Next(minQtd, maxQtd + 1);
                Item item = fabrica.Invoke();

                if (item.Consumivel)
                    item.AdicionarQuantidade(qtd - 1);
                drops.Add(item);
            }
        }

        return drops;
    }

    public int Atacar()
    {
        Random rand = new();
        // Dano aleatório baseado no ataque
        int dano = rand.Next(Math.Max(1, Ataque - 2), Ataque + 3); 
        return dano;
    }

    public void ExecutarTurno(Jogador jogador)
    {
        if (!EstaVivo()) return;

        Random rand = new();
        int acao = rand.Next(0, 100);

        if (acao < 60)
        {
            // Ataque normal
            if (rand.Next(0, 100) < 10) // 10% chance de miss
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"💨 {jogador.Nome} esquivou do ataque de {Nome}!");
                Console.ResetColor();
                return;
            }

            int dano = Atacar();
            BatalhaService.ExecutarAtaque(this, jogador, dano);
        }
        else if (acao < 80)
        {
            // Ataque especial (mais dano, mas sem crítico)
            int dano = (int)(Atacar() * (Elite ? 2.0 : 1.5));
            Console.ForegroundColor = Elite ? ConsoleColor.Magenta : ConsoleColor.Red;
            Console.WriteLine(Elite
                ? $"🔥 {Nome} (ELITE) liberou um ATAQUE DEVASTADOR!"
                : $"🔥 {Nome} usou ATAQUE ESPECIAL!");
            Console.ResetColor();

            BatalhaService.ExecutarAtaque(this, jogador, dano);
        }
        else if (acao < 95)
        {
            // Buff temporário
            Ataque += Elite ? 4 : 2;
            Defesa += Elite ? 2 : 1; 
            Console.ForegroundColor = Elite ? ConsoleColor.Magenta : ConsoleColor.Yellow;
            Console.WriteLine(Elite
                ? $"💪 {Nome} (ELITE) entrou em fúria extrema! ATK e DEF aumentados!"
                : $"💪 {Nome} entrou em fúria! ATK e DEF aumentados!");
            Console.ResetColor();
        }
        else
        {
            // Fugir
            Fugiu = true;
            Vida = 0; 
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"💨 {Nome} fugiu da batalha!");
            Console.ResetColor();
        }

        Thread.Sleep(Elite ? 800 : 600);
    }

}
