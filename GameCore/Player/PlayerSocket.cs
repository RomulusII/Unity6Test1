using GameCore.Services;
using Model.UnityOyun.Assets.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using Model;
// --- Yardımcı Sınıflar ---

// Oyuncu sınıfı (Actor-Like)
public class PlayerSocket
{
    public bool LoggedIn { get; set; } = false;
    private WebSocket webSocket { get; }
    private List<string> OwnedObjects { get; } = new List<string>();

    public PlayerBase? Player { get; set; }

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
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Add the polymorphic converter
            settings.Converters.Add(new PolymorphicJsonConverter<MessageContentBase>());

            // Mesajı JSON olarak çözümle
            var request = JsonConvert.DeserializeObject<SocketMessage>(message, settings);

            if (request != null)
            {
                if (!LoggedIn && request.MessType != nameof(SocketMessageType.LoginRequest))
                {
                    await SendErrorResponse("Önce giriş yapmalısınız!");
                    return;
                }

                switch (request.MessType)
                {
                    case nameof(SocketMessageType.LoginRequest):
                        await HandleLoginRequest(request.Data);
                        break;

                    case nameof(SocketMessageType.GetObjects):
                        await HandleGetObjectsRequest();
                        break;

                    default:
                        await SendErrorResponse("Bilinmeyen bir eylem!");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mesaj işlenirken hata oluştu: {ex.Message}");
            await SendErrorResponse("Server Error: "+ ex.Message);
        }
    }


    private async Task HandleLoginRequest(string? message)
    {
        if(message == null)
        {
            await SendLoginResponse(false, "Geçersiz giriş isteği!");
            return;
        }

        var loginReq = JsonConvert.DeserializeObject<LoginRequest>(message);

        if (loginReq == null)
        {
            await SendLoginResponse(false, "Geçersiz giriş isteği!");
            return;
        }

        var user = await PlayerService.GetPlayer(loginReq.Email, loginReq.Password);

        if (user == null)
        {
            await SendLoginResponse(false, "Kullanıcı adı veya parola yanlış!");
            return;
        }

        await GameServiceStatic.PlayerService.Login(this);

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
            Data = JsonConvert.SerializeObject(objects)
        };
        await SendMessage(response);
    }

    public async Task Logout(WebSocketCloseStatus closeStatus, string message)
    {
        LoggedIn = false;
        await webSocket.CloseAsync(closeStatus, message, CancellationToken.None);
    }

    public async Task SendLoginResponse(bool success, string message)
    {
        var loginResponse = new LoginResponse
        {
            Success = success,
            Message = message
        };
        await SendMessage(loginResponse);
    }

 

    public async Task SendErrorResponse(string message)
    {
        var loginResponse = new ErrorResponse
        {
            Message = message
        };
        await SendMessage(loginResponse);
    }

    // Oyuncunun sahip olduğu nesneleri döndür
    public List<string> GetOwnedObjects()
    {
        return OwnedObjects;
    }

    // Oyuncuya mesaj gönder
    public async Task SendMessage(MessageContentBase content)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var serializedContent = JsonConvert.SerializeObject(content, settings);
        Console.WriteLine($"Serialized Content: {serializedContent}");

        var message = new SocketMessage
        {
            MessType = content.GetType().Name, // Use Name instead of ToString()
            Data = serializedContent
        };

        var serializedMessage = JsonConvert.SerializeObject(message, settings);
        Console.WriteLine($"Serialized Message: {serializedMessage}");

        var buffer = Encoding.UTF8.GetBytes(serializedMessage);
        var segment = new ArraySegment<byte>(buffer);
        await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

}

public class ConnectedPlayer : Player 
{
    public PlayerSocket? PlayerSocket { get; set; }
}