using Newtonsoft.Json;
using System.Net.WebSockets;
using Newtonsoft.Json.Linq;

public class SocketProcessor
{
    public List<PlayerSocket> activeConnections = new List<PlayerSocket>();

    // Oyuncu bağlantısını yöneten metot
    public async Task HandleNewConnection(WebSocket webSocket)
    {
        var playerId = Guid.NewGuid().ToString(); // Benzersiz bir oyuncu ID'si
        var playerSocket = new PlayerSocket(webSocket);
        activeConnections.Add(playerSocket);

        try
        {
            // WebSocket bağlantısı açık olduğu sürece mesajları dinle
            await playerSocket.ListenConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bağlantı hatası: {ex.Message}");
        }
        finally
        {
            // Bağlantıyı temizle
            activeConnections.Remove(playerSocket);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bağlantı kapatıldı", CancellationToken.None);
            Console.WriteLine($"Oyuncu {playerId} bağlantısını kapattı.");
        }
    }
}
