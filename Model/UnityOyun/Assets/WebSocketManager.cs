using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket _webSocket;
    private CancellationTokenSource _cancellationTokenSource;

    [Header("WebSocket Settings")]
    public string serverUri = "ws://localhost:8080/ws"; // WebSocket server address
    public bool autoConnectOnStart = true;

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
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();

            Debug.Log($"Connecting to WebSocket server: {serverUri}...");
            await _webSocket.ConnectAsync(new Uri(serverUri), _cancellationTokenSource.Token);

            if (_webSocket.State == WebSocketState.Open)
            {
                Debug.Log("WebSocket connection established!");
                _ = ReceiveMessages(); // Start receiving messages in the background

                // Send initial message
                SendTextMessage("Hello from Unity!");
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
            while (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log($"Message received: {message}");
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.Log("WebSocket server closed the connection.");
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"WebSocket receiving error: {ex.Message}");
        }
    }

    public async void SendTextMessage(string message)
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket is not connected. Cannot send message.");
            return;
        }

        try
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            Debug.Log($"Message sent: {message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
        }
    }

    private async void OnApplicationQuit()
    {
        if (_webSocket != null)
        {
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application quitting", _cancellationTokenSource.Token);
                _webSocket.Dispose();
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during WebSocket shutdown: {ex.Message}");
            }
        }
    }
}