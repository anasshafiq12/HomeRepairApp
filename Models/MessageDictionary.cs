using System.Collections.Concurrent;

namespace HouseRepairApp.Models
{
    public static class MessageDictionary
    {
       public static ConcurrentDictionary<string, List<Message>> Messagedictionary
            = new ConcurrentDictionary<string, List<Message>>();

    }
}
