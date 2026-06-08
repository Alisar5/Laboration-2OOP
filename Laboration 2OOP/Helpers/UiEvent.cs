namespace Laboration_2OOP
{
    public class UiEvent
    {
        public int Id { get; }
        public string DisplayText { get; }

        public UiEvent(int id, string text)
        {
            Id = id;
            DisplayText = text;
        }
    }
}