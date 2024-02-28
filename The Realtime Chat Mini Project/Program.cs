using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Fleck;
using lib;
using The_Realtime_Chat_Mini_Project;
using The_Realtime_Chat_Mini_Project.Repositories;
using The_Realtime_Chat_Mini_Project.Service;


var server = new WebSocketServer("ws://0.0.0.0:8181");

var builder = WebApplication.CreateBuilder(args);

var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<MessageRepository>();
builder.Services.AddSingleton<MessageService>();
var app = builder.Build();

var wsConnections = new List<IWebSocketConnection>();



server.Start(ws =>
{
    
    ws.OnOpen = () =>
    {
        ws.Send(JsonSerializer.Serialize(new ServerMessage.ServerResponse()
        {
            eventType = "ConnectionOpened",
            message = "Connection"
        }));
        StateService.AddConnection(ws);
    };
    ws.OnClose = () => Console.WriteLine("Disconnected");
    ws.OnMessage = async message =>
    {
        try
        {
            await app.InvokeClientEventHandler(clientEventHandlers, ws, message);
            Console.WriteLine(message);
        }
        catch (Exception e)
        {
            ws.Send(JsonSerializer.Serialize(new ServerSendsError()
            {
                errorMessage = e.Message,
            }));
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
         
        }
    };
    
});
Console.ReadLine();
public class ServerSendsError : BaseDto
{
    public string errorMessage { get; set; }
   
}


