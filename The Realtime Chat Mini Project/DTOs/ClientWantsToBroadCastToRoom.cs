using System.Text.Json;
using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToBroadCastToRoomDto : BaseDto
{
    public string Message { get; set; }
    public int RoomId { get; set; }
}
public class ClientWantsToBroadCastToRoom : BaseEventHandler<ClientWantsToBroadCastToRoomDto>
{
    public override Task Handle(ClientWantsToBroadCastToRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerBroadcastsMessageWithUsername()
        {
            Message = dto.Message, 
            Username = StateService.Connections[socket.ConnectionInfo.Id].Username

        };
        StateService.BroadcastToRoom(dto.RoomId,JsonSerializer.Serialize(message));
        return Task.CompletedTask;
    }
}

public class ServerBroadcastsMessageWithUsername : BaseDto
{
    public string Message { get; set; }
    public string Username { get; set; }
}