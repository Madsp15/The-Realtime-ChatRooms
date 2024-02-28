using The_Realtime_Chat_Mini_Project.Models;
using Dapper;

namespace The_Realtime_Chat_Mini_Project.Repositories;

public class MessageRepository
{

    public MessageData SaveMessage(MessageData messageData)
    {
        var sql = $@"INSERT INTO messages (roomid, message, username, timestamp) 
                VALUES (@roomid, @message, @nickname, @timestamp) 
                RETURNING 
                roomid as {nameof(messageData.roomId)},
                message as {nameof(messageData.message)},
                username as {nameof(messageData.username)},
                timestamp as {nameof(messageData.timeStamp)};";
                
        using (var conn = DataConnection.DataSource.OpenConnection())
        {
            return conn.QueryFirst<MessageData>(sql, new
            {
                roomid = messageData.roomId,
                message = messageData.message,
                username = messageData.username,
                timetamp = messageData.timeStamp
            });
        }
    }
    
    public IEnumerable<MessageData> GetLast5Messages(int roomId)
    {
        var sql = $@"SELECT message, username, roomid, timestamp FROM messages WHERE roomid = @roomId ORDER BY timestamp DESC LIMIT 5;";
        
        using (var conn = DataConnection.DataSource.OpenConnection())
        {
            return conn.Query<MessageData>(sql, new {roomId});
        }
    }
    
    
    
}