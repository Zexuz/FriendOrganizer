using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend {FirstName = "Kalle", LastName = "Anka"};
            yield return new Friend {FirstName = "Alexander", LastName = "Anka"};
            yield return new Friend {FirstName = "Knatte", LastName = "Anka"};
            yield return new Friend {FirstName = "Kajsa", LastName = "Anka"};
        }
    }
}