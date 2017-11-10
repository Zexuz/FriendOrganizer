using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class OpenDetialViewEvent:PubSubEvent<OpenDetialViewEventArgs>
    {
        
    }

    public class OpenDetialViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}