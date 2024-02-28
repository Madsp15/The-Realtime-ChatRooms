using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using The_Realtime_Chat_Mini_Project.Models;
using The_Realtime_Chat_Mini_Project.Service;
using System.Net.Http;
using System.Net.Http.Headers;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToBroadCastToRoomDto : BaseDto
{
    public string message { get; set; }
    public int roomId { get; set; }
}
public class ClientWantsToBroadCastToRoom(MessageService messageService) : BaseEventHandler<ClientWantsToBroadCastToRoomDto>
{
    
    public override async Task Handle(ClientWantsToBroadCastToRoomDto dto, IWebSocketConnection socket)
    {
        await isMessageBad(dto.message); 
        var topic = StateService.GetRoomsForClient(socket.ConnectionInfo.Id);

        if (!topic.Contains(dto.roomId))
        {
            socket.Send(JsonSerializer.Serialize(new ServerMessage.ServerResponse()
            {
                eventType = "ServerResponse",
                message = "You are not in the room you are trying to broadcast to"
            }));
        }
            
        DateTime now = DateTime.Now;
        
        messageService.SaveMessage(new MessageData()
        {
            message = dto.message,
            username = StateService.Connections[socket.ConnectionInfo.Id].username,
            roomId = dto.roomId,
            timeStamp = now.ToString("HH:mm dd/MM/yyyy")
        });
        
        var message = new ServerBroadcastsMessageWithUsername()
        {
            message = dto.message, 
            username = StateService.Connections[socket.ConnectionInfo.Id].username
            
        };

        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(message));

        
    }

    public record RequestModel(string text, List<string> categories, string outputType)
    {
        public override string ToString()
        {
            return $"{{ text = {text}, categories = {categories}, outputType = {outputType} }}";
        }
    }

    private async Task isMessageBad(string message)
    {
        
        HttpClient client = new HttpClient();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://itsfilteringtime.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");

        request.Headers.Add("accept", "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("apikey"));
        
        var req = new RequestModel(message, new List<string>()
        {
            "Hate", "Violence", "SelfHarm", "Sexual"
        }, "FourSeverityLevels");
        
    request.Content = new StringContent(JsonSerializer.Serialize(req));
        
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponse>(responseBody);
        var isToxic = obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;
        if (isToxic)
        throw new ValidationException("Message is toxic");
    }
}

public class ServerBroadcastsMessageWithUsername : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
    
}

public class CatergoriesAnalysis
{
    public string category { get; set; }
    public int severity { get; set; }
}

public class ContentFilterResponse
{
    public List<object> blocklistsMatch { get; set; }
    public List<CatergoriesAnalysis> categoriesAnalysis { get; set; }
}