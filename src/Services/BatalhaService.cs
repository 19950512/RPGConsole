namespace RPGConsole.Services;

using RPGConsole.Models;
using RPGConsole.Models.Itens;
using RPGConsole.Models.NPCs;
using RPGConsole.Models.Monstros;
using RPGConsole.Models.Vocacoes;
using RPGConsole.Utils;


public static class BatalhaService
{
    public static void IniciarBatalha(Jogador jogador, List<Monstro> monstros)
    {
        while (jogador.EstaVivo() && monstros.Any(m => m.EstaVivo()))
        {
            Console.Clear();
            MostrarStatusBatalha(jogador, monstros);

            // Menu de ação do jogador
            string[] opcoes = { "Atacar", "Usar Skill", "Usar Item", "Fugir" };
            int escolha = UIHelper.MenuInterativo("Escolha sua ação", opcoes);

            Console.Clear();

            switch (escolha)
            {
                case 0: // Atacar normal
                    AtacarMonstro(jogador, monstros);
                    break;

                case 1: // Usar Skill (em área)
                    UsarSkillArea(jogador, monstros);
                    break;

                case 2: // Usar Item
                    UsarItem(jogador);
                    break;

                case 3: // Fugir
                    UIHelper.Mensagem($"{jogador.Nome} fugiu da batalha!", ConsoleColor.Yellow);
                    UIHelper.EsperarTecla();

                    // Se tiver monstros mortos, vamos pegar os loots e exp.
                    if (monstros.Any(m => !m.EstaVivo()))
                    {
                        FinalizarBatalha(jogador, monstros);
                    }
                    return;
            }

            // 🔹 Turno dos monstros
            foreach (var monstro in monstros.Where(m => m.EstaVivo()))
            {
                monstro.ExecutarTurno(jogador);
                if (!jogador.EstaVivo()) break;
            }

            // 🔹 Verifica se a batalha acabou
            if (monstros.All(m => !m.EstaVivo()))
            {
                FinalizarBatalha(jogador, monstros);
                break;
            }

            jogador.PassarTurno();
        }
    }

    // --------------------------
    //      Métodos Auxiliares
    // --------------------------

    private static void MostrarStatusBatalha(Jogador jogador, List<Monstro> monstros)
    {
        UIHelper.MostrarTitulo($"{jogador.Nome} VS {string.Join(", ", monstros.Select(m => m.Nome))}");
        UIHelper.MostrarBarraVida(
            jogador.Nome,
            jogador.Vida,
            jogador.VidaMaxTotal,
            jogador.AtaqueTotal,
            jogador.DefesaTotal
        );

        foreach (Monstro monstro in monstros)
        {
            if (monstro.EstaVivo())
                UIHelper.MostrarBarraVida(
                    monstro.Nome,
                    monstro.Vida,
                    monstro.VidaMax,
                    monstro.Ataque,
                    0,                      // Monstros sem defesa por enquanto
                    monstro.Elite
                );
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"💀 {monstro.Nome} derrotado");
                Console.ResetColor();
            }
        }
    }

    private static void AtacarMonstro(Jogador jogador, List<Monstro> monstros)
    {
        List<Monstro> alvos = monstros.Where(monstro => monstro.EstaVivo()).ToList();
        string[] nomesAlvos = alvos.Select(monstro => monstro.Nome).ToArray();
        int alvoIndex = UIHelper.MenuInterativo("Escolha o inimigo para atacar", nomesAlvos);

        int danoJogador = jogador.Atacar();
        ExecutarAtaque(jogador, alvos[alvoIndex], danoJogador);
    }

    private static void UsarSkillArea(Jogador jogador, List<Monstro> monstros)
    {
        int danoSkill = jogador.UsarSkill();
        if (danoSkill <= 0) return;

        foreach (Monstro monstro in monstros.Where(m => m.EstaVivo()))
        {
            ExecutarAtaque(jogador, monstro, danoSkill);
        }
    }

    private static void UsarItem(Jogador jogador)
    {
        List<Item> itens = jogador.Inventario
            .Where(i => i.Quantidade > 0)
            .ToList();

        if (itens.Count == 0)
        {
            UIHelper.Mensagem("❌ Você não tem itens!", ConsoleColor.Red);
            Thread.Sleep(800);
            return;
        }

        string[] opcoesItens = itens
            .Select(i => $"{i.Nome} x{i.Quantidade}")
            .Append("Cancelar")
            .ToArray();

        int escolhaItem = UIHelper.MenuInterativo("Escolha um item para usar/equipar", opcoesItens);

        if (escolhaItem == opcoesItens.Length - 1)
        {
            UIHelper.Mensagem("❌ Ação cancelada.", ConsoleColor.DarkGray);
            Thread.Sleep(600);
            return;
        }

        Item item = itens[escolhaItem];

        if (item.Consumivel)
        {
            item.Consumir(jogador);
                
            // Se acabou, remove
            if (item.Quantidade <= 0)
            {
                jogador.Inventario.Remove(item);
            }
        }
        else if (item is Equipamento equipamento)
        {
            equipamento.Usar(jogador);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {item.Nome} não pode ser usado.");
            Console.ResetColor();
        }

        Thread.Sleep(800);
    }


   private static void FinalizarBatalha(Jogador jogador, List<Monstro> monstros)
    {
        List<Monstro> derrotados = monstros.Where(m => !m.Fugiu && !m.EstaVivo()).ToList();
        List<Monstro> fugidos = monstros.Where(m => m.Fugiu).ToList();

        if (derrotados.Count > 0)
        {
            UIHelper.Mensagem($"🎉 {jogador.Nome} derrotou {derrotados.Count} monstros!", ConsoleColor.Yellow);

            jogador.GanharExperiencia(derrotados.Sum(m => m.Experiencia));

            foreach (Monstro monstro in derrotados)
            {
                List<Item> loot = monstro.DroparLoot();
                if (loot.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    UIHelper.EscreverLento($"💀 {monstro.Nome} não deixou nenhum item.", 30);
                    Console.ResetColor();
                    continue;
                }

                foreach (Item item in loot)
                {
                    jogador.AdicionarItem(item);
                    Console.ForegroundColor = item.GetColor();
                    UIHelper.EscreverLento($"➜ {item.Nome} x{item.Quantidade} ({item.Raridade}) adicionado ao inventário!", 30);
                    Console.ResetColor();
                }
            }

            // Cura pós-batalha
            int cura = (int)(jogador.VidaMax * 0.20);
            int vidaAntes = jogador.Vida;
            jogador.Vida = Math.Min(jogador.Vida + cura, jogador.VidaMax);
            if (jogador.Vida > vidaAntes)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                UIHelper.EscreverLento($"💖 {jogador.Nome} recuperou {jogador.Vida - vidaAntes} de HP ao descansar!", 30);
                Console.ResetColor();
            }
        }

        if (fugidos.Count > 0)
        {
            UIHelper.Mensagem($"💨 {jogador.Nome} venceu, mas {fugidos.Count} monstro(s) fugiram!", ConsoleColor.Yellow);
        }

        UIHelper.EsperarTecla();
    }

    public static void ExecutarAtaque(Combatente atacante, Combatente alvo, int dano)
    {
        if (dano <= 0) return;

        int vidaAntes = alvo.Vida;
        UIHelper.AnimacaoAtaque(atacante.Nome, alvo.Nome, dano);
        alvo.ReceberDano(dano);

        if (alvo.Vida == vidaAntes)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"⚔️ {alvo.Nome} resistiu ao ataque!");
            Console.ResetColor();
            Thread.Sleep(800);
            return;
        }

        UIHelper.MostrarDano(alvo.Nome, vidaAntes, alvo.Vida, alvo.VidaMax, dano);
        Thread.Sleep(800);
    }
}
