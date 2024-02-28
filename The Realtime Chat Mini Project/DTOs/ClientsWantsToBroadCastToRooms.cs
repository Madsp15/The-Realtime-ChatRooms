using System.Text.Json;
using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToBroadCastToRoomsDto : BaseDto
{
    public string message { get; set; }
}
public class ClientWantsToBroadCastToRooms : BaseEventHandler<ClientWantsToBroadCastToRoomsDto>
{
    public override Task Handle(ClientWantsToBroadCastToRoomsDto dto, IWebSocketConnection socket)
    {
        var message = new ServerBroadcastsMessagesWithUsername()
        {
            message = dto.message, 
            username = StateService.Connections[socket.ConnectionInfo.Id].username

        };
        var uRooms = StateService.UserRooms[socket.ConnectionInfo.Id];
        var allRooms = StateService.Rooms;
        Console.WriteLine(JsonSerializer.Serialize(uRooms.ToList()));
        Console.WriteLine(JsonSerializer.Serialize(uRooms.ToList().Count));
        Console.WriteLine(JsonSerializer.Serialize(allRooms.ToList()));
        Console.WriteLine(JsonSerializer.Serialize(allRooms.ToList().Count));
        
        //Sends message to all rooms the user is in
        foreach (var room in uRooms)
        {

            StateService.BroadcastToRoom(room, JsonSerializer.Serialize(message), socket);
        }
        socket.Send(JsonSerializer.Serialize(message));
        
        
        return Task.CompletedTask;
    }
}

public class ServerBroadcastsMessagesWithUsername : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
}