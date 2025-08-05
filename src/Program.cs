﻿namespace RPGConsole;

using System;
using RPGConsole.Network;
using Grpc.Net.Client;

using RPGConsole.Models;
using RPGConsole.Services;
using RPGConsole.Utils;
using RPGConsole.Models.Vocacoes;
using System.Text.Json;
using RPGConsole.Models.Itens;
using RPGConsole.Models.Monstros;
using RPGConsole.Models.NPCs;
using RPGConsole.Models.NPCs.Mercadores;
using RPG.Protos;

internal class Program
{

    private static Jogador jogador = new Jogador("Herói", new Knight());

    private static string email = "";

    private static async Task Main(string[] args)
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Bem-vindo ao RPG Console ===");

        var client = GameClient.Connect(); // client é GameService.GameServiceClient

        Console.Write("Digite seu e-mail: ");
        email = Console.ReadLine() ?? "teste@teste.com";

        Console.Write("Digite sua senha: ");
        string senha = Console.ReadLine() ?? "123";

        var loginResponse = await client.LoginAsync(new LoginRequest { Email = email, Password = senha });

        if (!loginResponse.Success)
        {
            Console.WriteLine("Conta não encontrada. Criando nova conta...");
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

            RegisterResponse registerResponse = await client.RegisterAsync(new RegisterRequest
            {
                Email = email,
                Password = senha,
                JsonData = JsonSerializer.Serialize(new Jogador(nome, vocacao))
            });

            Console.WriteLine(registerResponse.Message);

            jogador = new Jogador(nome, vocacao);

        }
        else
        {
            Console.WriteLine("Login realizado com sucesso!");
            PlayerData p = loginResponse.Player;

            string json = loginResponse.Player.JsonData;
            jogador = JsonSerializer.Deserialize<Jogador>(json)!;

            jogador.Vocacao = jogador.NomeVocacao switch
            {
                "Knight" => new Knight(),
                "Mage" => new Mage(),
                "Paladin" => new Paladin(),
                "Assassin" => new Assassin(),
                _ => new Knight()
            };

        }


        // Equipamentos iniciais só para Knight (exemplo)
        /*
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
        */

        MenuPrincipal(client);
    }

    private static void MenuPrincipal(GameService.GameServiceClient client)
    {
        while (jogador.EstaVivo())
        {
            string[] opcoesMenu = { "Caçar (Hunt)", "Meu Personagem", "Cidade", "Sair" };
            int escolha = UIHelper.MenuInterativo("Menu Principal", opcoesMenu);

            SalvarProgresso(client, jogador);

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
    
    private static void SalvarProgresso(GameService.GameServiceClient client, Jogador jogador)
    {
        PlayerData playerData = new PlayerData
        {
            Email = email,
            JsonData = JsonSerializer.Serialize(jogador)
        };

        SaveResponse response = client.SavePlayer(playerData);
        Console.WriteLine(response.Message);
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
