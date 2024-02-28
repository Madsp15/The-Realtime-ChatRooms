using System.Text.Json;
using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ReturnRoomsUserIsInDto : BaseDto
{
    public string username { get; set; }
    public int roomId { get; set; }
}

public class ReturnRoomsUserIsIn : BaseEventHandler<ReturnRoomsUserIsInDto>
{

    public override Task Handle(ReturnRoomsUserIsInDto dto, IWebSocketConnection socket)
    {
        List<int> rooms = new();
        foreach (var room in StateService.UserRooms[socket.ConnectionInfo.Id])
        {
            rooms.Add(room);
        }
        
        socket.Send(JsonSerializer.Serialize(new ServerMessage.ServerResponse()
        {
            message = "You are in rooms " + JsonSerializer.Serialize(rooms),
            eventType = "ServerResponseWithNumberRooms"
        }));
        return Task.CompletedTask;
    }
}

