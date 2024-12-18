using Model.UnityOyun.Assets.Model;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;


public class SocketProcesser
{
    public ConcurrentDictionary<string, PlayerSocket> activeConnections = new ConcurrentDictionary<string, PlayerSocket>();

    // Oyuncu bağlantısını yöneten metot
    public async Task HandleConnection(WebSocket webSocket)
    {
        var playerId = Guid.NewGuid().ToString(); // Benzersiz bir oyuncu ID'si
        var player = new PlayerSocket(playerId, webSocket);
        activeConnections[playerId] = player;

        try
        {
            // WebSocket bağlantısı açık olduğu sürece mesajları dinle
            while (webSocket.State == WebSocketState.Open)
            {
                var message = await ReceiveMessage(webSocket);
                if (message != null)
                {
                    Console.WriteLine($"İstemciden mesaj alındı: {message}");
                    await HandleMessage(player, message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bağlantı hatası: {ex.Message}");
        }
        finally
        {
            // Bağlantıyı temizle
            activeConnections.TryRemove(playerId, out _);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bağlantı kapatıldı", CancellationToken.None);
            Console.WriteLine($"Oyuncu {playerId} bağlantısını kapattı.");
        }
    }

    // Mesajları işleyen metot
    async Task HandleMessage(PlayerSocket player, string message)
    {
        try
        {
            // Mesajı JSON olarak çözümle
            var request = JsonSerializer.Deserialize<RequestMessage>(message);

            if (request != null)
            {
                switch (request.Action)
                {
                    case nameof(RequestActionType.Login):
                        player.Name = request.Data; // Oyuncunun adını ayarla
                        Console.WriteLine($"Oyuncu giriş yaptı: {player.Name}");
                        await player.SendMessage($"Hoş geldin {player.Name}!");
                        break;

                    case nameof(RequestActionType.GetObjects):
                        var objects = player.GetOwnedObjects(); // Oyuncunun sahip olduğu nesneler
                        var response = new ResponseMessage
                        {
                            Action = "object_list",
                            Data = JsonSerializer.Serialize(objects)
                        };
                        await player.SendMessage(JsonSerializer.Serialize(response));
                        break;

                    default:
                        await player.SendMessage("Bilinmeyen bir eylem!");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mesaj işlenirken hata oluştu: {ex.Message}");
        }
    }

    // WebSocket'ten mesaj alma metodu
    async Task<string?> ReceiveMessage(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Text)
        {
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
        else if (result.MessageType == WebSocketMessageType.Close)
        {
            Console.WriteLine("İstemci bağlantıyı kapattı.");
        }

        return null;
    }
}

public enum RequestActionType
{
    Login,
    GetObjects
}

// --- Yardımcı Sınıflar ---

// Oyuncu sınıfı (Actor-Like)
public class PlayerSocket : PlayerBase
{
    private WebSocket WebSocket { get; }
    private List<string> OwnedObjects { get; } = new List<string>();

    public PlayerSocket(string id, WebSocket webSocket)
    {
        Id = id;
        WebSocket = webSocket;

        // Oyuncunun sahip olduğu varsayılan nesneler
        OwnedObjects.Add("Kale");
        OwnedObjects.Add("Asker");
        OwnedObjects.Add("Maden");
    }

    // Oyuncunun sahip olduğu nesneleri döndür
    public List<string> GetOwnedObjects()
    {
        return OwnedObjects;
    }

    // Oyuncuya mesaj gönder
    public async Task SendMessage(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);
        await WebSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}

// İstemci tarafından gönderilen mesaj model
public class RequestMessage
{
    public string Action { get; set; } = "";
    public string Data { get; set; } = "";
}

// Sunucunun gönderdiği yanıt model
public class ResponseMessage
{
    public string Action { get; set; } = "";
    public string Data { get; set; } = "";
}