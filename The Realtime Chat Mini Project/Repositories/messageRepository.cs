using The_Realtime_Chat_Mini_Project.Models;
using Dapper;

namespace The_Realtime_Chat_Mini_Project.Repositories;

public class MessageRepository
{

    public MessageData SaveMessage(MessageData messageData)
    {
        var sql = $@"INSERT INTO messages (message, nickname, room) 
                VALUES (@message, @nickname, @room) 
                RETURNING 
                message as {nameof(messageData.Message)},
                nickname as {nameof(messageData.Nickname)},
                room as {nameof(messageData.Room)};";
        using (var conn = DataConnection.DataSource.OpenConnection())
        {
            return conn.QueryFirst<MessageData>(sql, new
            {
                message = messageData.Message,
                nickname = messageData.Nickname,
                room = messageData.Room
            });
        }
    }
    
}