using System.Diagnostics;
using System.Reflection;
using Fleck;
using lib;
using The_Realtime_Chat_Mini_Project;
using The_Realtime_Chat_Mini_Project.Repositories;
using The_Realtime_Chat_Mini_Project.Service;


var server = new WebSocketServer("ws://0.0.0.0:8181");

var builder = WebApplication.CreateBuilder(args);

var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();

var wsConnections = new List<IWebSocketConnection>();

server.Start(ws =>
{
    
    ws.OnOpen = () =>
    {
        ws.Send("Connected");
        StateService.AddConnection(ws);
    };
    ws.OnClose = () => Console.WriteLine("Disconnected");
    ws.OnMessage = async message =>
    {
        try
        {
            await app.InvokeClientEventHandler(clientEventHandlers, ws, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
         
        }
    };
});

//builder.Services.AddSingleton<MessageRepository>();
//builder.Services.AddSingleton<MessageService>();

Console.ReadLine();