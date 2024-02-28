using The_Realtime_Chat_Mini_Project.Models;
using The_Realtime_Chat_Mini_Project.Repositories;

namespace The_Realtime_Chat_Mini_Project.Service;


public class MessageService
{
    private readonly MessageRepository _messageRepository;
    
    public MessageService(MessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
   public MessageData SaveMessage(MessageData messageData)
      {
            return _messageRepository.SaveMessage(messageData);
      }
   
   public IEnumerable<MessageData> GetLast5Messages(int roomId)
   {
       return _messageRepository.GetLast5Messages(roomId);
   }
}

   
