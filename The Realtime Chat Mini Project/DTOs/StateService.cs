using Fleck;
using lib;

namespace The_Realtime_Chat_Mini_Project;

public class WebSocketWithMetadata(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;

    public string username { get; set; }
}

public static class StateService {
    
   
    public static Dictionary<Guid, WebSocketWithMetadata> Connections = new();
    
    public static Dictionary<Guid, HashSet<int>> UserRooms = new(); //What rooms is user in

    public static Dictionary<int, HashSet<Guid>> Rooms = new(); //all rooms?

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
        Console.WriteLine("created to room " + room);
        
        if (!UserRooms.ContainsKey(ws.ConnectionInfo.Id)) {
            UserRooms.Add(ws.ConnectionInfo.Id, new HashSet<int>());
            UserRooms[ws.ConnectionInfo.Id].Add(room);
            Console.WriteLine("added to room " + room);
            
        }
        else
        {
                    UserRooms[ws.ConnectionInfo.Id].Add(room);

        }

        return Rooms[room].Add(ws.ConnectionInfo.Id);
    }

    public static void BroadcastToRoom(int room, string message, IWebSocketConnection? dontSentToThis = null)
    {
        if (Rooms.TryGetValue(room, out var guids))
            foreach (var guid in guids)
            {
                if (Connections.TryGetValue(guid, out var ws) && ws != null && ws.Connection != dontSentToThis)
                    ws.Connection.Send(message);
            }
    }
    
  
    
    public static List<int> GetRoomsForClient(Guid clientId)
    {
        return UserRooms.TryGetValue(clientId, out var rooms) ? rooms.ToList() : new List<int>();
    }
    
    


}
