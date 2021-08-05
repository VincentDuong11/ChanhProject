using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChanhProject.Models
{
    public enum EventType
    {
        TakeIn,
        TakeOut,
    }

    public class EventModel
    {
        public EventType eventType { get; set; }
        public DateTime timestamp { get; set; }
        public UserModel user  { get; set; }
        public BookModel book { get; set; }


}
}
