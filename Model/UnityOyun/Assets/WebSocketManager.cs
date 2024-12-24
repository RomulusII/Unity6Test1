using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using UnityEngine;
using Model.UnityOyun.Assets.Model;
using Newtonsoft.Json;

public class ClientSocket : MonoBehaviour
{
    private ClientWebSocket webSocket;
    private CancellationTokenSource _cancellationTokenSource;

    [Header("WebSocket Settings")]
    public string serverUri = "ws://localhost:8080/ws"; // WebSocket server address

    public bool autoConnectOnStart = true;

    public bool LoggedIn { get; private set; }

    // Define the delegates and events
    public delegate void LoginResponseReceivedEventHandler(object sender, LoginResponse loginResponse);
    public event LoginResponseReceivedEventHandler LoginResponseReceived;

    public delegate void ErrorReceivedEventHandler(object sender, string errorMessage);
    public event ErrorReceivedEventHandler ErrorReceived;

    private async void Start()
    {
        if (autoConnectOnStart)
        {
            await ConnectToWebSocket();
        }
    }

    private async Task ConnectToWebSocket()
    {
        try
        {
            webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();

            Debug.Log($"Connecting to WebSocket server: {serverUri}...");
            await webSocket.ConnectAsync(new Uri(serverUri), _cancellationTokenSource.Token);

            if (webSocket.State == WebSocketState.Open)
            {
                Debug.Log("WebSocket connection established!");
                _ = ReceiveMessages(); // Start receiving messages in the background
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"WebSocket connection failed: {ex.Message}");
        }
    }

    private async Task ReceiveMessages()
    {
        var buffer = new byte[1024 * 4]; // 4 KB buffer

        try
        {
            while (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log($"Message received: {message}");
                    HandleMessage(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.Log("WebSocket server closed the connection.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"WebSocket receiving error: {ex.Message}");
        }
    }

    public async Task SendTextMessage(string message)
    {
        try
        {
            if (webSocket == null || webSocket.State != WebSocketState.Open)
            {
                await ConnectToWebSocket();
            }



            if (webSocket == null || webSocket.State != WebSocketState.Open)
            {
                Debug.LogWarning("WebSocket is not connected. Cannot send message.");
                return;
            }
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            Debug.Log($"Message sent: {message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
        }
    }

    public async Task SendMessage(MessageContentBase message)
    {
        try
        {
            var socketMessage = new SocketMessage
            {
                MessType = message.GetType().Name,
                Data = JsonConvert.SerializeObject(message)
            };

            await SendTextMessage(JsonConvert.SerializeObject(socketMessage));
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
        }
    }

    private async void OnApplicationQuit()
    {
        if (webSocket != null)
        {
            try
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application quitting", _cancellationTokenSource.Token);
                webSocket.Dispose();
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during WebSocket shutdown: {ex.Message}");
            }
        }
    }

    private void HandleMessage(string message)
    {
        try
        {
            var socketMessage = JsonConvert.DeserializeObject<SocketMessage>(message);
            var x = nameof(SocketMessageType.LoginResponse);
            switch (socketMessage.MessType)
            {
                case nameof(SocketMessageType.LoginResponse):
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(socketMessage.Data);
                    Debug.Log($"Login response received: {loginResponse.Success}");
                    if (loginResponse.Success)
                    {
                        LoggedIn = true;
                    }
                    // Raise the LoginResponseReceived event
                    LoginResponseReceived?.Invoke(this, loginResponse);
                    break;
                case nameof(SocketMessageType.ErrorResponse):
                    Debug.Log($"Server reports error: {socketMessage.Data.ToString()}");
                    // Raise the ErrorReceived event
                    ErrorReceived?.Invoke(this, socketMessage.Data);
                    break;

                default:
                    Debug.LogWarning($"Unknown message type: {socketMessage.MessType}");
                    break;
            }
        }
        catch (Exception ex)
        {
            // show in log
            Debug.LogError($"Failed to handle message: {ex.Message}");
            throw;
        }
    }
}
