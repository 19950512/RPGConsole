﻿namespace RPGConsole;

using System;
using RPGConsole.Models;
using RPGConsole.Services;
using RPGConsole.Utils;
using RPGConsole.Models.Vocacoes;
using RPGConsole.Models.Itens;
using RPGConsole.Models.Monstros;
using RPGConsole.Models.NPCs;
using RPGConsole.Models.NPCs.Mercadores;

internal class Program
{

    private static Jogador jogador = new Jogador("Herói", new Knight());

    private static void Main(string[] args)
    {
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Bem-vindo ao RPG Console ===");
        Console.Write("Digite o nome do seu personagem: ");
        string nome = Console.ReadLine() ?? "Herói";

        if (string.IsNullOrWhiteSpace(nome))
        {
            nome = "Herói";
        }

        Console.WriteLine("Escolha sua vocação: 1-Knight 2-Mage 3-Paladin 4-Assassin");
        int escolha = int.Parse(Console.ReadLine() ?? "1");

        Vocacao vocacao = escolha switch
        {
            2 => new Mage(),
            3 => new Paladin(),
            4 => new Assassin(),
            _ => new Knight()
        };

        jogador = new Jogador(nome, vocacao);

        // Equipamentos iniciais só para Knight (exemplo)
        if (vocacao is Knight)
        {
            Equipamento escudo = new EscudoDeMadeira();
            jogador.Equipar(escudo);

            Equipamento armadura = new ArmaduraDeCouro();
            jogador.Equipar(armadura);

            Equipamento espada = new EspadaDeFerro();
            jogador.Equipar(espada);

            Equipamento capacete = new CapaceteDeAco();
            jogador.Equipar(capacete);

            Equipamento botas = new BotasDeCouro();
            jogador.Equipar(botas);

            Equipamento calcas = new CalcaDeLona();
            jogador.Equipar(calcas);

            Equipamento anel = new AnelDePrata();
            jogador.Equipar(anel);

            Equipamento anel2 = new AnelDePrata();
            jogador.Equipar(anel2);

            Item pocaoDeVida = new PocaoVida();
            pocaoDeVida.AdicionarQuantidade(9); // Adiciona 9 poções de vida, totalizando 10
            jogador.Inventario.Add(pocaoDeVida);
        }

        MenuPrincipal();
    }

    private static void MenuPrincipal()
    {
        while (jogador.EstaVivo())
        {
            string[] opcoesMenu = { "Caçar (Hunt)", "Meu Personagem", "Cidade", "Sair" };
            int escolha = UIHelper.MenuInterativo("Menu Principal", opcoesMenu);

            switch (escolha)
            {
                case 0: // Caçar / Batalha
                    IniciarBatalha();
                    break;
                case 1: // Meu Personagem
                    UIHelper.MostrarInformacoesDoPersonagem(jogador);
                    break;

                case 2: // Cidade
                    VisitarCidade();
                    break;
                case 3: // Sair
                    Console.WriteLine("Saindo do jogo...");
                    return;
            }
        }

        Console.WriteLine("💀 Game Over!");
    }

    private static void IniciarBatalha()
    {
        Random rand = new();
        int qtdMonstros = rand.Next(1, 4);
        List<Monstro> monstros = new();

        for (int i = 0; i < qtdMonstros; i++)
        {
            int tipo = rand.Next(0, 3);
            monstros.Add(tipo switch
            {
                0 => new Goblin(),
                1 => new Orc(),
                2 => new Amazon(),
                _ => new Slime()
            });
        }

        Console.Clear();
        Console.WriteLine($"Batalha iniciada contra {monstros.Count} monstros!");
        UIHelper.MostrarMonstros(monstros);
        Console.WriteLine("Pressione ENTER para começar a batalha...");
        Console.ReadLine();

        BatalhaService.IniciarBatalha(jogador, monstros);

        if (jogador.EstaVivo())
        {
            UIHelper.MostrarInformacoesDoPersonagem(jogador);
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }
    }

    // Converte string para TipoEquipamento
    private static TipoEquipamento TipoEquipamentoParse(string texto)
    {
        return texto switch
        {
            "Cabeca" => TipoEquipamento.Cabeca,
            "Corpo" => TipoEquipamento.Corpo,
            "MaoDireita" => TipoEquipamento.MaoDireita,
            "MaoEsquerda" => TipoEquipamento.MaoEsquerda,
            "Pernas" => TipoEquipamento.Pernas,
            "Anel" => TipoEquipamento.Anel,
            "Amuleto" => TipoEquipamento.Amuleto,
            "Bracelete" => TipoEquipamento.Bracelete,
            "Cinto" => TipoEquipamento.Cinto,
            "Botas" => TipoEquipamento.Botas,
            _ => throw new ArgumentException($"TipoEquipamento inválido: {texto}")
        };
    }

    private static void VisitarCidade()
    {
        List<NPC> npcs = new()
        {
            new Binda(),
        };

        string[] opcoes = npcs.Select(n => $"{n.Tipo} | {n.Nome}").Append("Voltar").ToArray();
        int escolha = UIHelper.MenuInterativo("Cidade", opcoes);

        if (escolha == npcs.Count) return;

        npcs[escolha].Interagir(jogador!);
    }
}
