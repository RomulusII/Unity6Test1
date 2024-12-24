
using GameCore.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var SocketProcesser = new SocketProcessor();

// Aktif bağlantıları ve oyuncuları tutan koleksiyon

// WebSocket middleware'i etkinleştir
app.UseWebSockets();

// WebSocket endpoint'i
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("Yeni bir istemci bağlandı!");

        // Oyuncu bağlantısını yönet
        await SocketProcesser.HandleNewConnection(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400; // Bad Request
    }
});

app.Run("http://localhost:8080");




////using GameCore.Map;
////using GameCore.Services;

////var builder = WebApplication.CreateBuilder(args);
////var x = GameService.Game.Harita.MaxX;

////// Add services to the container.

////builder.Services.AddControllers();
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();
////app.MapControllers();
////app.Run();


//using System.Net.WebSockets;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// WebSocket middleware'ini etkinleştir
//app.UseWebSockets();

//// WebSocket bağlantılarını işlemek için bir route tanımlayın
//app.Map("/ws", async (context) =>
//{
//    // WebSocket isteğini doğrula
//    if (context.WebSockets.IsWebSocketRequest)
//    {
//        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//        Console.WriteLine("Yeni bir WebSocket bağlantısı kuruldu.");

//        // Mesaj alışverişini işleyen bir metot çağır
//        await HandleWebSocketConnection(webSocket);
//    }
//    else
//    {
//        context.Response.StatusCode = 400; // Bad Request
//    }
//});

//// WebSocket bağlantılarını işleme metodu
//async Task HandleWebSocketConnection(WebSocket webSocket)
//{
//    try
//    {
//        var buffer = new byte[1024 * 4];
//        WebSocketReceiveResult result;

//        // Bağlantı açık olduğu sürece mesajları oku
//        while (webSocket.State == WebSocketState.Open)
//        {
//            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//            if (result.MessageType == WebSocketMessageType.Text)
//            {
//                // Gelen mesajı çözümle
//                var clientMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                Console.WriteLine($"İstemciden mesaj alındı: {clientMessage}");

//                // Cevap gönder
//                var serverMessage = Encoding.UTF8.GetBytes($"Mesajınız alındı: {clientMessage}");
//                await webSocket.SendAsync(new ArraySegment<byte>(serverMessage), WebSocketMessageType.Text, true, CancellationToken.None);
//            }
//            else if (result.MessageType == WebSocketMessageType.Close)
//            {
//                Console.WriteLine("WebSocket bağlantısı kapatıldı.");
//                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bağlantı kapatıldı", CancellationToken.None);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"WebSocket hatası: {ex.Message}");
//        throw;
//    }
//}

//app.Run("http://localhost:8080");