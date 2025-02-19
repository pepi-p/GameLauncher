using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Launcher.Interfaces;
using Launcher.Models;

namespace Launcher.Services
{
    public class WebSocketClient : IWebSocketHandler
    {
        private readonly ClientWebSocket _webSocket = new();
        private readonly Uri _uri;
        private Dictionary<string, Action<int>> _actDict = new();

        public WebSocketClient(IServerAddressProvider serverAddress)
        {
            _uri = new Uri(serverAddress.GetWebSocketAddress());
        }

        public void RegistrarAction(string key, Action<int> method)
        {
            _actDict.Add(key, method);
        }

        public async Task ConnectAsync()
        {
            System.Diagnostics.Debug.WriteLine(_uri);
            try
            {
                await _webSocket.ConnectAsync(_uri, CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("Connected to WebSocket server.");

                // メッセージの受信を開始
                _ = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WebSocket connection error: {ex.Message}");
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    System.Diagnostics.Debug.WriteLine("WebSocket connection closed.");
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonData = JsonSerializer.Deserialize<UpdateData>(message, options);                

                if (_actDict.ContainsKey(jsonData.Command))
                {
                    _actDict[jsonData.Command].Invoke(jsonData.Id);
                    System.Diagnostics.Debug.WriteLine($"{jsonData.Command}: {jsonData.Id}");
                }

                System.Diagnostics.Debug.WriteLine($"Received: {message}");
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task DisconnectAsync()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("Disconnected from WebSocket server.");
            }
        }
    }
}
