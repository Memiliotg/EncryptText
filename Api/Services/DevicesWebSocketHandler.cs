using System.Net.WebSockets;

namespace Api.Services
{
    public class DevicesWebSocketHandler
    {
        public async Task Handle(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new System.ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
               
                await webSocket.SendAsync(new System.ArraySegment<byte>(buffer, 0, result.Count),
                    result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new System.ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
