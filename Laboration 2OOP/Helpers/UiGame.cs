namespace Laboration_2OOP
{
    public class UiGame
    {
        public int Id { get; }
        public string DisplayText { get; }

        public UiGame(int id, string text)
        {
            Id = id;
            DisplayText = text;
        }

        public override string ToString() => DisplayText;
    }
}