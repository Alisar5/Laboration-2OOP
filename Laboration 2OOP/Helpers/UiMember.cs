namespace Laboration_2OOP
{
    public class UiMember
    {
        public int Id { get; }
        public string DisplayText { get; }

        public UiMember(int id, string text)
        {
            Id = id;
            DisplayText = text;
        }
    }
}