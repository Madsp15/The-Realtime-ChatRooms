using System.Text.Json;
using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToSignInDto : BaseDto
{
    public string username { get; set; }
}
public class ClientWantsToSignIn : BaseEventHandler<ClientWantsToSignInDto>
{

    public override Task Handle(ClientWantsToSignInDto dto, IWebSocketConnection socket)
    {
        StateService.Connections[socket.ConnectionInfo.Id].username = dto.username;
        socket.Send(JsonSerializer.Serialize(new ServerMessage.ServerResponse()
        {
            eventType = "ServerResponseWithLoggedInUsername",
            message = "Welcome   " + dto.username
        }));
        return Task.CompletedTask;
    }
}