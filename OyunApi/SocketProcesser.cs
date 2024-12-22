using GameCore.Services;
using Model;
using Model.UnityOyun.Assets.Model;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

public class SocketProcesser
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

public enum RequestActionType
{
    Login,
    GetObjects
}

// --- Yardımcı Sınıflar ---

// Oyuncu sınıfı (Actor-Like)
public class PlayerSocket
{
    public bool LoggedIn { get; set; } = false;
    private WebSocket webSocket { get; }
    private List<string> OwnedObjects { get; } = new List<string>();

    public PlayerBase Player { get; set; }

    public PlayerSocket(WebSocket webSocket)
    {
        this.webSocket = webSocket;

        // Oyuncunun sahip olduğu varsayılan nesneler
        OwnedObjects.Add("Kale");
        OwnedObjects.Add("Asker");
        OwnedObjects.Add("Maden");
    }

    public async Task ListenConnection()
    {
        try
        {
            // WebSocket bağlantısı açık olduğu sürece mesajları dinle
            while (webSocket.State == WebSocketState.Open)
            {
                var message = await ReceiveMessage(webSocket);
                if (message != null)
                {
                    Console.WriteLine($"İstemciden mesaj alındı: {message}");
                    await HandleMessage(message);
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
            // activeConnections.TryRemove(playerId, out _);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bağlantı kapatıldı", CancellationToken.None);
            Console.WriteLine($"Oyuncu {Player?.ToString()} bağlantısını kapattı.");
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

    // Mesajları işleyen metot
    async Task HandleMessage(string message)
    {
        try
        {
            // Mesajı JSON olarak çözümle
            var request = JsonSerializer.Deserialize<SocketMessage>(message);

            if (request != null)
            {
                if(!LoggedIn && request.Action != nameof(RequestActionType.Login))
                {
                    await SendMessage("Önce giriş yapmalısınız!");
                    return;
                }

                switch (request.Action)
                {
                    case nameof(RequestActionType.Login):
                        await HandleLoginRequest(message);
                        break;

                    case nameof(RequestActionType.GetObjects):
                        await HandleGetObjectsRequest();
                        break;

                    default:
                        await SendMessage("Bilinmeyen bir eylem!");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mesaj işlenirken hata oluştu: {ex.Message}");
        }
    }


    private async Task HandleLoginRequest(string message)
    {
        var loginReq = JsonSerializer.Deserialize<LoginRequest>(message);
        var user = await GameServiceStatic.PlayerService.Login(loginReq.Email, loginReq.Password);

        if (user == null)
        {
            await SendLoginResponse(false, "Kullanıcı adı veya parola yanlış!");
            return;
        }

        Player = user;

        Console.WriteLine($"Oyuncu giriş yaptı: {Player.Email}");

        await SendLoginResponse(true, $"Hoş geldin {Player.Name}!");
    }

    private async Task HandleGetObjectsRequest()
    {
        var objects = GetOwnedObjects(); // Oyuncunun sahip olduğu nesneler
        var response = new ResponseMessage
        {
            Action = "object_list",
            Data = JsonSerializer.Serialize(objects)
        };
        await SendMessage(JsonSerializer.Serialize(response));
    }



    public async Task SendLoginResponse(bool success, string message)
    {
        var loginResponse = new LoginResponse
        {
            Success = success,
            Message = message
        };
        await SendMessage(JsonSerializer.Serialize(loginResponse));
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
        await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<bool> LogIn(LoginRequest loginRequest)
    {
        Player = await GameServiceStatic.PlayerService.Login(loginRequest.Email, loginRequest.Password);
        return Player != null;
    }

}

