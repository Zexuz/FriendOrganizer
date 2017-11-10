using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class ProgramminLanguageWrapper : ModelWrapper<ProgramminLanguage>
    {
        public int Id
        {
            get { return Model.Id; }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ProgramminLanguageWrapper(ProgramminLanguage model) : base(model)
        {
        }
    }
}