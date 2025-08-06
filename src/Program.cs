﻿namespace RPGConsole;

using System;
using RPGConsole.Network;
using Grpc.Net.Client;
using RPGConsole.Utils;
using System.Text.Json;
using RPG.Protos;

internal class Program
{
    private static string email = "";
    private static GameService.GameServiceClient? client;

    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        UIHelper.MostrarTitulo("Bem-vindo ao RPG Console");

        client = GameClient.Connect();

        Console.Write("Digite seu e-mail: ");
        email = Console.ReadLine() ?? "teste@teste.com";

        Console.Write("Digite sua senha: ");
        string senha = Console.ReadLine() ?? "123";

        var loginResponse = await client.LoginAsync(new LoginRequest { Email = email, Password = senha });

        if (!loginResponse.Success)
        {
            await CriarNovaConta();
        }
        else
        {
            Console.WriteLine($"✅ Login realizado com sucesso! Bem-vindo de volta!");
        }

        await MenuPrincipal();
    }

    private static async Task CriarNovaConta()
    {
        Console.WriteLine("📝 Conta não encontrada. Criando nova conta...");
        
        Console.Write("Digite o nome do seu personagem: ");
        string nome = Console.ReadLine() ?? "Herói";

        if (string.IsNullOrWhiteSpace(nome))
        {
            nome = "Herói";
        }

        string[] vocacoes = { "Knight", "Mage", "Assassin", "Paladin" };
        int escolhaVocacao = UIHelper.MenuInterativo("Escolha sua vocação", vocacoes);
        string vocacaoEscolhida = vocacoes[escolhaVocacao];

        var createRequest = new CreatePlayerRequest
        {
            Email = email,
            PlayerName = nome,
            VocationName = vocacaoEscolhida
        };

        var createResponse = await client!.CreatePlayerAsync(createRequest);
        
        if (createResponse.Success)
        {
            Console.WriteLine($"✅ Personagem {nome} ({vocacaoEscolhida}) criado com sucesso!");
        }
        else
        {
            Console.WriteLine($"❌ Erro ao criar personagem: {createResponse.Message}");
        }
    }

    private static async Task MenuPrincipal()
    {
        bool jogando = true;

        while (jogando)
        {
            string[] opcoes = { 
                "📊 Ver Status do Personagem", 
                "⚔️ Explorar Área", 
                "🏪 Visitar Cidade", 
                "💾 Salvar Progresso",
                "🚪 Sair do Jogo" 
            };

            int escolha = UIHelper.MenuInterativo("Menu Principal", opcoes);

            switch (escolha)
            {
                case 0:
                    await MostrarStatusPersonagem();
                    break;
                case 1:
                    await ExplorarArea();
                    break;
                case 2:
                    await VisitarCidade();
                    break;
                case 3:
                    await SalvarProgresso();
                    break;
                case 4:
                    jogando = false;
                    Console.WriteLine("👋 Obrigado por jogar!");
                    break;
            }
        }
    }

    private static async Task MostrarStatusPersonagem()
    {
        var request = new GetPlayerStatusRequest { Email = email };
        var response = await client!.GetPlayerStatusAsync(request);

        if (response.Success)
        {
            Console.Clear();
            UIHelper.MostrarTitulo("Status do Personagem");
            
            Console.WriteLine($"📛 Nome: {response.PlayerName}");
            Console.WriteLine($"🎭 Vocação: {response.VocationName}");
            Console.WriteLine($"⭐ Nível: {response.Level}");
            Console.WriteLine($"❤️ Vida: {response.CurrentHp}/{response.MaxHp}");
            Console.WriteLine($"⚔️ Ataque: {response.TotalAttack}");
            Console.WriteLine($"🛡️ Defesa: {response.TotalDefense}");
            Console.WriteLine($"🎯 Experiência: {response.Experience}");
            Console.WriteLine($"💰 Moedas: {response.Coins}");
            
            Console.WriteLine("\n📦 Inventário:");
            if (response.Inventory.Count == 0)
            {
                Console.WriteLine("  (Vazio)");
            }
            else
            {
                foreach (var item in response.Inventory)
                {
                    Console.WriteLine($"  • {item}");
                }
            }
        }
        else
        {
            Console.WriteLine($"❌ Erro ao obter status: {response.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private static async Task ExplorarArea()
    {
        Console.WriteLine("🗺️ Explorando a área...");
        
        var request = new StartBattleRequest { Email = email };
        var response = await client!.StartBattleAsync(request);

        if (response.Success)
        {
            Console.WriteLine($"⚔️ Batalha iniciada contra {response.MonsterCount} monstro(s)!");
            Console.WriteLine($"🎯 Resultado: {response.BattleResult}");
            
            if (response.Victory)
            {
                Console.WriteLine($"🎉 Vitória! Você ganhou {response.ExpGained} XP e {response.CoinsGained} moedas!");
                
                if (response.ItemsLooted.Count > 0)
                {
                    Console.WriteLine("📦 Itens obtidos:");
                    foreach (var item in response.ItemsLooted)
                    {
                        Console.WriteLine($"  • {item}");
                    }
                }
            }
            else
            {
                Console.WriteLine("💀 Derrota! Você foi derrotado...");
            }
        }
        else
        {
            Console.WriteLine($"❌ Erro na batalha: {response.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private static Task VisitarCidade()
    {
        Console.WriteLine("🏘️ Visitando a cidade...");
        Console.WriteLine("🚧 Funcionalidade será implementada em breve!");
        UIHelper.EsperarTecla();
        return Task.CompletedTask;
    }

    private static async Task SalvarProgresso()
    {
        var request = new SaveProgressRequest { Email = email };
        var response = await client!.SaveProgressAsync(request);

        if (response.Success)
        {
            Console.WriteLine("💾 Progresso salvo com sucesso!");
        }
        else
        {
            Console.WriteLine($"❌ Erro ao salvar: {response.Message}");
        }

        UIHelper.EsperarTecla();
    }
}
