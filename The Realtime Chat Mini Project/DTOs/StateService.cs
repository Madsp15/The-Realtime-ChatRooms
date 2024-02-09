using Fleck;

namespace The_Realtime_Chat_Mini_Project;

public class WebSocketWithMetadata(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;

    public string Username { get; set; }
}

public static class StateService
{
    public static Dictionary<Guid, WebSocketWithMetadata> Connections = new();

    public static Dictionary<int, HashSet<Guid>> Rooms = new(); //What connections are inside int room
    public static void AddConnection(IWebSocketConnection ws)
    {
        Connections.TryAdd(ws.ConnectionInfo.Id, 
            new WebSocketWithMetadata(ws));
    }

    public static bool AddToRoom(IWebSocketConnection ws, int room)
    {
        if (!Rooms.ContainsKey(room))
            Rooms.Add(room, new HashSet<Guid>());
        
        Rooms[room].Add(ws.ConnectionInfo.Id);

        return Rooms[room].Add(ws.ConnectionInfo.Id);
    }

    public static void BroadcastToRoom(int room, string message)
    {
        var doesRoomExist = Rooms.TryGetValue(room, out var guids);
        if (doesRoomExist)
        {
            foreach (var guid in guids)
            {
                if (Connections.TryGetValue(guid, out var ws))
                {
                    ws.Connection.Send(message);
                }
                
            }
        }
    }
}