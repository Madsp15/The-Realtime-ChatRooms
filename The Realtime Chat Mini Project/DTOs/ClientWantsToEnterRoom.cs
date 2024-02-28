using System.Text.Json;
using Fleck;
using lib;
using The_Realtime_Chat_Mini_Project.Models;
using The_Realtime_Chat_Mini_Project.Service;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToEnterRoomDto : BaseDto
{
    public int roomId { get; set; }
}
public class ClientWantsToEnterRoom(MessageService messageService) : BaseEventHandler<ClientWantsToEnterRoomDto>
{
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        
        messageService.GetLast5Messages(dto.roomId);
        
        socket.Send(JsonSerializer.Serialize(new ServerMessage.ServerAddsCllientToRoom()
        {
            eventType = "ServerAddsClientToRoom",
            lastMessages = messageService.GetLast5Messages(dto.roomId),
            roomId = dto.roomId
        }));
        var isSuccess = StateService.AddToRoom(socket, dto.roomId);
        socket.Send(JsonSerializer.Serialize(new ServerMessage.ServerResponse()
        {
            eventType = "ServerResponseWithRoomId",
            message = "You were successfully added to room  " + dto.roomId
        }));
        
        
        return Task.CompletedTask;
    }
}
