
namespace RPGConsole.Utils;

using RPGConsole.Models;
using RPGConsole.Models.Itens;
using RPGConsole.Models.NPCs;
using RPGConsole.Models.Monstros;
using RPGConsole.Models.Vocacoes;

public static class UIHelper
{
    public static int MostrarMonstros(List<Monstro> monstros)
    {
        Console.WriteLine("Monstros encontrados:");
        for (int i = 0; i < monstros.Count; i++)
        {
            Console.WriteLine($"[{i}] {monstros[i].Nome} (Vida: {monstros[i].Vida}/{monstros[i].VidaMax})");
        }
        return monstros.Count;
    }
    
    // ------------------------
    //   Menu Interativo
    // ------------------------
    public static int MenuInterativo(string titulo, string[] opcoes, bool incluirVoltar = false)
    {
        int index = 0;
        ConsoleKey key;

        // 🔹 Se precisar de "Voltar" adiciona ao final das opções
        string[] menuOpcoes = incluirVoltar
            ? opcoes.Concat(new[] { "Voltar" }).ToArray()
            : opcoes;

        LimparBufferConsole();

        do
        {
            Console.Clear();
            MostrarTitulo(titulo);

            for (int i = 0; i < menuOpcoes.Length; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {menuOpcoes[i]} <");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"  {menuOpcoes[i]}");
                }
            }

            Console.ResetColor();
            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow) index = (index == 0) ? menuOpcoes.Length - 1 : index - 1;
            if (key == ConsoleKey.DownArrow) index = (index + 1) % menuOpcoes.Length;

        } while (key != ConsoleKey.Enter);

        LimparBufferConsole();

        // 🔹 Retorna -1 caso o jogador escolha "Voltar"
        if (incluirVoltar && index == menuOpcoes.Length - 1)
            return -1;

        return index;
    }

    // ------------------------
    //   Título Estilizado
    // ------------------------
    public static void MostrarTitulo(string titulo)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        int largura = Math.Max(titulo.Length + 6, 40);
        string borda = new string('═', largura);

        Console.WriteLine("╔" + borda + "╗");

        int espacos = (largura - titulo.Length) / 2;
        Console.WriteLine("║" + new string(' ', espacos) + titulo + new string(' ', largura - titulo.Length - espacos) + "║");

        Console.WriteLine("╚" + borda + "╝");

        Console.ResetColor();
        Console.WriteLine();
    }

    // 🔹 Limpar qualquer tecla que esteja no buffer
    public static void LimparBufferConsole()
    {
        if (Console.IsInputRedirected) return; // evita erro em ambiente sem console real

        while (Console.KeyAvailable)
            Console.ReadKey(true);
    }
        // 🔹 Esperar uma tecla com buffer limpo
    public static void EsperarTecla(string mensagem = "Pressione qualquer tecla para continuar...")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"\n{mensagem}");
        Console.ResetColor();

        LimparBufferConsole();
        Console.ReadKey(true);
        LimparBufferConsole();
    }
    
    // 🔹 Animação de ataque
    public static void AnimacaoAtaque(string atacante, string alvo, int dano)
    {
        EscreverLento($"{atacante} ataca {alvo}...", 30);

        // Efeito de impacto
        for (int i = 0; i < 3; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" *");
            Thread.Sleep(100);
            Console.Write("\b\b  \b\b");
            Thread.Sleep(100);
        }

        Console.ResetColor();
        Console.WriteLine();
        Mensagem($"💥 {alvo} sofreu {dano} de dano!", ConsoleColor.Red);
        Thread.Sleep(500);
    }

    // 🔹 Animação de dano com barra atualizada
    public static void MostrarDano(string nome, int vidaAntiga, int vidaNova, int vidaMax, int ataque = 0)
    {
        for (int hp = vidaAntiga; hp >= vidaNova && hp >= 0; hp--)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            MostrarBarraVida(nome, hp, vidaMax, ataque);
            Thread.Sleep(10);
        }
    }

    public static void MostrarInformacoesDoPersonagem(Jogador jogador)
    {
        bool exibirInformacoes = true;
        while (exibirInformacoes)
        {
            Console.Clear();
            MostrarTitulo("Informações do Personagem");

            Console.WriteLine($"Nome: {jogador.Nome}");
            Console.WriteLine($"Vocação: {jogador.Vocacao.Nome}");
            Console.WriteLine($"Vida: {jogador.Vida}/{jogador.VidaMax}");
            Console.WriteLine($"Ataque: {jogador.AtaqueTotal}");
            Console.WriteLine($"Defesa: {jogador.DefesaTotal}");
            Console.WriteLine($"Experiência: {jogador.Experiencia}");
            Console.WriteLine($"Moedas: {jogador.Moedas}");
            Console.WriteLine($"Nível: {jogador.Level}");

            Console.WriteLine("\n=== EQUIPAMENTOS ===");
            foreach (var slotEquipamento in jogador.Equipamentos)
            {
                string nomeSlot = slotEquipamento.Key.ToString();
                List<Equipamento> equipamentosNoSlot = slotEquipamento.Value;

                if (equipamentosNoSlot.Count == 0)
                {
                    Console.WriteLine($"- {nomeSlot}: Nenhum equipado");
                }
                else
                {
                    if (slotEquipamento.Key == TipoEquipamento.Anel)
                    {
                        string aneisEquipados = string.Join(", ",
                            equipamentosNoSlot.Select(anel =>
                                $"{anel.Nome} (+{anel.BonusAtaque} ATK, +{anel.BonusDefesa} DEF, +{anel.BonusVida} HP)"
                            )
                        );
                        Console.WriteLine($"- {nomeSlot}s: {aneisEquipados}");
                    }
                    else
                    {
                        foreach (Equipamento equipamento in equipamentosNoSlot)
                        {
                            Console.WriteLine($"- {nomeSlot}: {equipamento.Nome} (+{equipamento.BonusAtaque} ATK, +{equipamento.BonusDefesa} DEF, +{equipamento.BonusVida} HP)");
                        }
                    }
                }
            }

            Console.WriteLine("\n=== INVENTÁRIO ===");
            if (jogador.Inventario.Count == 0)
            {
                Console.WriteLine("Nenhum item no inventário.");
            }
            else
            {
                foreach (Item itemInventario in jogador.Inventario)
                {
                    string tipoItem = itemInventario.Consumivel ? "Consumível" : "Equipamento";
                    Console.WriteLine($"- {itemInventario.Nome} x{itemInventario.Quantidade} ({tipoItem})");
                }
            }

            Console.WriteLine("\n=== AÇÕES ===");
            // Opções de ações
            Console.WriteLine("1. Equipar/Desequipar Itens");
            Console.WriteLine("0. Voltar");

            Console.Write("Escolha uma ação: ");
            string? escolha = Console.ReadLine()?.Trim();
            if (escolha == "1")
            {

                string[] opcoesMenu = { "Equipar item do inventário", "Desequipar item" };
                int escolhaMenu = MenuInterativo("Ações", opcoesMenu, incluirVoltar: true);
                if (escolhaMenu == -1) break;
                if (escolhaMenu == 0) EquiparItemInventario(jogador);
                else if (escolhaMenu == 1) DesequiparItem(jogador);
            }
            else if (escolha == "0")
            {
                exibirInformacoes = false; // Voltar ao menu principal
                break; // Voltar
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
                EsperarTecla();
            }
        }
    }

    private static void EquiparItemInventario(Jogador jogador)
    {
        List<Equipamento> equipamentosNoInventario = jogador.Inventario
            .OfType<Equipamento>()
            .ToList();

        if (equipamentosNoInventario.Count == 0)
        {
            Mensagem("⚠ Você não tem equipamentos para equipar.", ConsoleColor.Yellow);
            EsperarTecla();
            return;
        }

        while (true)
        {
            string[] opcoesEquipamentos = equipamentosNoInventario
                .Select(equipamento =>
                    $"{equipamento.Nome} (+{equipamento.BonusAtaque} ATK, +{equipamento.BonusDefesa} DEF, +{equipamento.BonusVida} HP)"
                )
                .ToArray();

            int escolhaEquipamento = MenuInterativo("Escolha um item para equipar", opcoesEquipamentos, incluirVoltar: true);
            if (escolhaEquipamento == -1) break;

            Equipamento equipamentoSelecionado = equipamentosNoInventario[escolhaEquipamento];

            if (jogador.Equipar(equipamentoSelecionado))
            {
                jogador.RemoverItem(equipamentoSelecionado);
                Mensagem($"✅ {equipamentoSelecionado.Nome} equipado!", ConsoleColor.Green);
            }
            else
            {
                Mensagem($"❌ Não foi possível equipar {equipamentoSelecionado.Nome}.", ConsoleColor.Red);
            }

            EsperarTecla();
            break;
        }
    }

    private static void DesequiparItem(Jogador jogador)
    {
        List<Equipamento> equipamentosEquipados = jogador.Equipamentos
            .SelectMany(parSlot => parSlot.Value)
            .ToList();

        if (equipamentosEquipados.Count == 0)
        {
            Mensagem("⚠ Você não tem nenhum equipamento para remover.", ConsoleColor.Yellow);
            EsperarTecla();
            return;
        }

        while (true)
        {
            string[] opcoesEquipamentos = equipamentosEquipados
                .Select(equipamento =>
                    $"{equipamento.Nome} (+{equipamento.BonusAtaque} ATK, +{equipamento.BonusDefesa} DEF, +{equipamento.BonusVida} HP)"
                )
                .ToArray();

            int escolhaEquipamento = MenuInterativo("Escolha um item para desequipar", opcoesEquipamentos, incluirVoltar: true);
            if (escolhaEquipamento == -1) break;

            Equipamento equipamentoSelecionado = equipamentosEquipados[escolhaEquipamento];

            jogador.Desequipar(equipamentoSelecionado.Tipo);
            jogador.AdicionarItem(equipamentoSelecionado);

            Mensagem($"✅ {equipamentoSelecionado.Nome} foi movido para o inventário!", ConsoleColor.Green);
            EsperarTecla();
            break;
        }
    }



    // 🔹 Barra de vida colorida com ATK e DEF
    public static void MostrarBarraVida(
        string nome,
        int vida,
        int vidaMax,
        int ataque = 0,
        int defesa = 0,
        bool elite = false
    )
    {
        int largura = 20;
        int vidaAtual = Math.Max(vida, 0);
        int preenchido = (int)(vidaAtual / (vidaMax * 1.0) * largura);

        // Cor da barra de vida
        Console.ForegroundColor = vida > (vidaMax * 0.5) ? ConsoleColor.Green :
                                vida > (vidaMax * 0.25) ? ConsoleColor.Yellow :
                                ConsoleColor.Red;

        string barra = new string('█', preenchido).PadRight(largura, '░');

        Console.ResetColor();

        // Nome colorido se Elite
        if (elite)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{nome,-14}");
            Console.ResetColor();
        }
        else
        {
            Console.Write($"{nome,-14}");
        }

        // Barra de HP
        Console.Write($" |{barra}| {vidaAtual,3}/{vidaMax,-3} ");

        // 🔹 ATK em vermelho
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"ATK {ataque,2} ");
        Console.ResetColor();

        // 🔹 DEF em azul
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"DEF {defesa,2}");
        Console.ResetColor();

        Console.WriteLine();
    }

    // 🔹 Mostrar mensagem com cor
    public static void Mensagem(string texto, ConsoleColor cor = ConsoleColor.Gray)
    {
        Console.ForegroundColor = cor;
        Console.WriteLine(texto);
        Console.ResetColor();
    }

    // 🔹 Efeito de digitação
    public static void EscreverLento(string texto, int delay = 20)
    {
        foreach (char c in texto)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }
}
