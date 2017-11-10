using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class ProgramminLanguageWrapper : ModelWrapper<ProgramminLanguage>
    {
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ProgramminLanguageWrapper(ProgramminLanguage model) : base(model)
        {
        }
    }
}