using RPG.Protos;
using RPGConsole.Utils;

namespace RPGConsole;

public class MultiplayerCommands
{
    private readonly GameService.GameServiceClient _client;
    private readonly string _userEmail;

    public MultiplayerCommands(GameService.GameServiceClient client, string userEmail)
    {
        _client = client;
        _userEmail = userEmail;
    }

    public async Task ShowMenu()
    {
        bool voltarMenu = false;

        while (!voltarMenu)
        {
            string[] opcoes = {
                "🌐 Ver Status do Servidor",
                "👥 Ver Jogadores Online",
                "💬 Enviar Mensagem",
                "📨 Ver Mensagens Recebidas",
                "📍 Atualizar Status Online",
                "🔙 Voltar ao Menu Principal"
            };

            int escolha = UIHelper.MenuInterativo("Sistema Multiplayer", opcoes);

            switch (escolha)
            {
                case 0:
                    await ShowServerStatus();
                    break;
                case 1:
                    await ShowOnlinePlayers();
                    break;
                case 2:
                    await SendMessage();
                    break;
                case 3:
                    await ShowMessages();
                    break;
                case 4:
                    await UpdatePlayerStatus();
                    break;
                case 5:
                    voltarMenu = true;
                    break;
            }
        }
    }

    private async Task ShowServerStatus()
    {
        try
        {
            var response = await _client.GetServerStatusAsync(new ServerStatusRequest());

            UIHelper.PrintSeparator();
            UIHelper.PrintHeader("STATUS DO SERVIDOR");
            
            if (response.ServerOnline)
            {
                Console.WriteLine("🟢 Servidor ONLINE");
                Console.WriteLine($"📊 Jogadores conectados: {response.PlayersOnline}");
                Console.WriteLine($"📈 Total de jogadores: {response.TotalPlayers}");
                Console.WriteLine($"⚔️ Jogadores em batalha: {response.PlayersInBattle}");
                Console.WriteLine($"⏱️ Uptime: {response.Uptime}");
                Console.WriteLine($"🏷️ Versão: {response.ServerVersion}");
            }
            else
            {
                Console.WriteLine("🔴 Servidor OFFLINE");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao verificar status do servidor: {ex.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private async Task ShowOnlinePlayers()
    {
        try
        {
            var response = await _client.GetOnlinePlayersAsync(new GetOnlinePlayersRequest());

            UIHelper.PrintSeparator();
            UIHelper.PrintHeader("JOGADORES ONLINE");

            if (response.Players.Count == 0)
            {
                Console.WriteLine("😔 Nenhum jogador online no momento.");
            }
            else
            {
                Console.WriteLine($"👥 {response.Players.Count} jogador(es) online:\n");

                foreach (var player in response.Players)
                {
                    string statusIcon = player.Status switch
                    {
                        "online" => "🟢",
                        "in_battle" => "⚔️",
                        "idle" => "�",
                        _ => "⚪"
                    };

                    Console.WriteLine($"{statusIcon} {player.PlayerName} (Nível {player.Level})");
                    Console.WriteLine($"   🎯 Vocação: {player.VocationName}");
                    Console.WriteLine($"   � Status: {player.Status}");
                    Console.WriteLine($"   🕐 Última atividade: {DateTimeOffset.FromUnixTimeSeconds(player.LastSeen):HH:mm:ss}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao buscar jogadores online: {ex.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private async Task SendMessage()
    {
        try
        {
            Console.Write("📧 Digite o nome do destinatário: ");
            string? recipientName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(recipientName))
            {
                Console.WriteLine("❌ Nome inválido!");
                UIHelper.EsperarTecla();
                return;
            }

            Console.Write("💬 Digite sua mensagem: ");
            string? messageText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(messageText))
            {
                Console.WriteLine("❌ Mensagem não pode estar vazia!");
                UIHelper.EsperarTecla();
                return;
            }

            var request = new SendMessageRequest
            {
                FromEmail = _userEmail,
                ToPlayerName = recipientName,
                Message = messageText
            };

            var response = await _client.SendMessageAsync(request);

            if (response.Success)
            {
                Console.WriteLine("✅ Mensagem enviada com sucesso!");
            }
            else
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem: {response.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao enviar mensagem: {ex.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private async Task ShowMessages()
    {
        try
        {
            var response = await _client.GetMessagesAsync(new GetMessagesRequest 
            { 
                Email = _userEmail 
            });

            UIHelper.PrintSeparator();
            UIHelper.PrintHeader("SUAS MENSAGENS");

            if (response.Messages.Count == 0)
            {
                Console.WriteLine("📭 Você não tem mensagens.");
            }
            else
            {
                Console.WriteLine($"📮 Você tem {response.Messages.Count} mensagem(s):\n");

                foreach (var message in response.Messages.OrderByDescending(m => m.Timestamp))
                {
                    Console.WriteLine($"� De: {message.FromPlayer}");
                    Console.WriteLine($"🕐 Em: {DateTimeOffset.FromUnixTimeSeconds(message.Timestamp):dd/MM/yyyy HH:mm:ss}");
                    Console.WriteLine($"� Mensagem: {message.Message}");
                    Console.WriteLine(new string('-', 50));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao buscar mensagens: {ex.Message}");
        }

        UIHelper.EsperarTecla();
    }

    private async Task UpdatePlayerStatus()
    {
        try
        {
            string[] statusOptions = { "🟢 Online", "⚔️ Em Batalha", "🟡 Ausente", "⚪ Offline" };
            string[] statusValues = { "online", "in_battle", "idle", "offline" };

            int escolha = UIHelper.MenuInterativo("Escolha seu status", statusOptions);
            
            if (escolha >= 0 && escolha < statusValues.Length)
            {
                var request = new UpdatePlayerStatusRequest
                {
                    Email = _userEmail,
                    Status = statusValues[escolha]
                };

                var response = await _client.UpdatePlayerStatusAsync(request);

                if (response.Success)
                {
                    Console.WriteLine($"✅ Status atualizado para: {statusOptions[escolha]}");
                }
                else
                {
                    Console.WriteLine($"❌ Erro ao atualizar status: {response.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao atualizar status: {ex.Message}");
        }

        UIHelper.EsperarTecla();
    }
}
