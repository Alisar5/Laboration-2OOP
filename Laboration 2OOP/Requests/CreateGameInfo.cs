using Laboration_2OOP.Domän;


namespace Laboration_2OOP.Requests
{
    public class CreateGameInfo
    {
        public string Titel { get; set; } = "";
        public Spelkategori Kategori { get; set; }
        public int MinAntalSpelare { get; set; }
        public int MaxAntalSpelare { get; set; }
        public int SpelTidMinuter { get; set; }
        public Svårighetsgrad Svårighetsgrad { get; set; }
        public string Beskrivning { get; set; } = "";
        public TillgänglighetStatus Tillgänglighet { get; set; }

    }
}
