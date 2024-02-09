using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class ClientWantsToSignInDto : BaseDto
{
    public string Username { get; set; }
}
public class ClientWantsToSignIn : BaseEventHandler<ClientWantsToSignInDto>
{

    public override Task Handle(ClientWantsToSignInDto dto, IWebSocketConnection socket)
    {
        StateService.Connections[socket.ConnectionInfo.Id].Username = dto.Username;
        socket.Send("signed in successfully");
        return Task.CompletedTask;
    }
}