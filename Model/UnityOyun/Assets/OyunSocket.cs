using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NativeWebSocket;

public class WebSocketManager : MonoBehaviour
{
    private WebSocket websocket;

    private async void Start()
    {
        // WebSocket sunucusuna ba?lanmak için URL
        string serverUrl = "ws://localhost:5000/ws"; // WebSocket Sunucu URL'si
        websocket = new WebSocket(serverUrl);

        // WebSocket olaylarini dinle
        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket baglantisi kuruldu!");

            websocket.Send(Encoding.ASCII.GetBytes("Test 1234"));
        };

        websocket.OnMessage += (bytes) =>
        {
            // Gelen mesaj? string'e dönü?tür
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Mesaj alindi: " + message);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("WebSocket baglantisi kapatildi. Msg:" + e.ToString());
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("WebSocket hatas?: " + e);
        };

        // Baglantiyi baslat
        await websocket.Connect();




    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // WebSocket baglantisini güncelle
        websocket?.DispatchMessageQueue();
#endif

        // Esc tusuna basildiginda baglantiyi kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConnection();
        }
    }

    public async void SendMessageToServer(string message)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
            Debug.Log("Mesaj gönderildi: " + message);
        }
    }

    private async void CloseConnection()
    {
        if (websocket != null)
        {
            await websocket.Close();
            Debug.Log("Baglanti kapatildi.");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
