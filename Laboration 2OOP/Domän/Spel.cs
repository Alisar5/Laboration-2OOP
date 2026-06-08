
using Laboration_2OOP.Requests;

namespace Laboration_2OOP.Domän
{
    public class Spel
    {
        public int SpelId { get; private set; }

        public string Titel { get; private set; }

        public Spelkategori Kategori { get; private set; }

        public int MinAntalSpelare { get; private set; }

        public int MaxAntalSpelare { get; private set; }

        public int SpelTidMinuter { get; private set; }

        public Svårighetsgrad Svårighetsgrad { get; private set; }

        public string Beskrivning { get; private set; }

        public TillgänglighetStatus Tillgänglig { get; private set; }
        private Spel()
        {
        }


        public Spel(int spelId, string titel, Spelkategori kategori, int min, int max, int tidMin,
             Svårighetsgrad grad, string beskrivning)
        {
            SpelId = spelId;
            Titel = (titel ?? "").Trim();
            Kategori = kategori;
            MinAntalSpelare = min;
            MaxAntalSpelare = max;
            SpelTidMinuter = tidMin;
            Svårighetsgrad = grad;
            Beskrivning = (beskrivning ?? "").Trim();
            Tillgänglig = TillgänglighetStatus.Tillgänglig;

            Validera();
        }
        public void SättTillgänglighet(TillgänglighetStatus status)
        {
            Tillgänglig = status;
        }


        private void Validera()
        {
            if (string.IsNullOrWhiteSpace(Titel))
                throw new ValideringsException("Speltitel är obligatorisk.");
            if (MinAntalSpelare <= 0 || MaxAntalSpelare < MinAntalSpelare)
                throw new ValideringsException("Ogiltigt intervall för antal spelare");
            if (SpelTidMinuter <= 0)
                throw new ValideringsException("Speltid måste vara större än 0");
        }
        public void UppdateraInfo(UpdateGameInfo info)
        {
            Titel = (info.Titel ?? "").Trim();
            Kategori = info.Kategori;
            MinAntalSpelare = info.MinAntalSpelare;
            MaxAntalSpelare = info.MaxAntalSpelare;
            SpelTidMinuter = info.SpelTidMinuter;
            Svårighetsgrad = info.Svårighetsgrad;
            Beskrivning = (info.Beskrivning ?? "").Trim();

            Validera();
        }
        public override string ToString()
        {
            return $"{Titel} {Kategori} {MinAntalSpelare}-{MaxAntalSpelare} spelare, ca {SpelTidMinuter} min";
        }
    }
}

