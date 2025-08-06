namespace RPGConsole.Utils;

public static class UIHelper
{
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

    // 🔹 Barra de progresso simples
    public static void MostrarBarraProgresso(string label, int atual, int maximo, int largura = 20)
    {
        int preenchido = (int)(atual / (maximo * 1.0) * largura);
        
        Console.ForegroundColor = atual > (maximo * 0.5) ? ConsoleColor.Green :
                                atual > (maximo * 0.25) ? ConsoleColor.Yellow :
                                ConsoleColor.Red;

        string barra = new string('█', preenchido).PadRight(largura, '░');
        Console.Write($"{label}: |{barra}| {atual}/{maximo}");
        
        Console.ResetColor();
        Console.WriteLine();
    }

    // 🔹 Animação de loading
    public static void MostrarLoading(string mensagem, int duracao = 2000)
    {
        Console.Write(mensagem);
        
        for (int i = 0; i < duracao / 500; i++)
        {
            Console.Write(".");
            Thread.Sleep(500);
        }
        
        Console.WriteLine(" ✓");
    }

    // 🔹 Separador visual
    public static void PrintSeparator()
    {
        Console.WriteLine(new string('=', 60));
    }

    // 🔹 Cabeçalho formatado
    public static void PrintHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"🎮 {title.ToUpper()}");
        Console.ResetColor();
        Console.WriteLine(new string('-', title.Length + 4));
    }
}
