using lib;
using The_Realtime_Chat_Mini_Project.Models;

namespace The_Realtime_Chat_Mini_Project;

public class ServerMessage
{
    public class ServerResponse : BaseDto
    {
        public string message { get; set; }
        public string eventType { get; set; }
    }
    
    public class ServerAddsCllientToRoom : BaseDto
    {
        public IEnumerable<MessageData> lastMessages  { get; set; }
        public string eventType { get; set; }
        public int roomId { get; set; }
        
    }
    
}