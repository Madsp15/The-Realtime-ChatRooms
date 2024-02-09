using System.Text.Json;
using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToEnterRoomDto : BaseDto
{
    public int RoomId { get; set; }
}

public class ClientWantsToEnterRoom : BaseEventHandler<ClientWantsToEnterRoomDto>
{
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        StateService.AddToRoom(socket, dto.RoomId);
        socket.Send(JsonSerializer.Serialize(new ServerAddsClientToRoom()
        {
            Message = "You where added to room " + dto.RoomId
        }));
        return Task.CompletedTask;
    }
}

public class ServerAddsClientToRoom : BaseDto
{
    public string Message { get; set; }
}