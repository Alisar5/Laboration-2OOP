

namespace Laboration_2OOP.Domän
{
    public class Anmälan
    {
        public int AnmälanId { get; private set; }

        public int MedlemId { get; private set; }

        public int SpelträffId { get; private set; }

        public DateTime Datum { get; private set; }

        public AnmälanStatus status { get; private set; }


        public bool ArAktiv => status == AnmälanStatus.Aktiv;

        public Anmälan(int anmälanId, int medlemId, int spelträffId)
        {
            AnmälanId = anmälanId;
            MedlemId = medlemId;
            SpelträffId = spelträffId;
            Datum = DateTime.Now;
            status = AnmälanStatus.Aktiv;
        }
        public void Avanmälan()
        {
            status = AnmälanStatus.Avanmäld;
        }
    }
}
