using Grpc.Net.Client;

namespace RPGConsole.Network;

public static class GameClient
{
    private static GameService.GameServiceClient? _client;

    public static GameService.GameServiceClient Connect(string serverAddress = "http://localhost:5000")
    {
        if (_client != null) return _client;

        var channel = GrpcChannel.ForAddress(serverAddress);
        _client = new GameService.GameServiceClient(channel);
        return _client;
    }
}
