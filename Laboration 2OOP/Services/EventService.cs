using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
    using global::Laboration_2OOP.DemoData;
    using global::Laboration_2OOP.Domän;


    namespace Laboration_2OOP.Services
    {
    public class EventService
    {
        public List<Spelträff> GetEventsOrderedByDate()
        {
            using (var db = new AppDbContext())
            {
                return db.Träffar
                    .OrderBy(t => t.StartTid)
                    .ToList();
            }
        }

        public List<UiGame> GetAvailableGamesForUc2()
        {
            using (var db = new AppDbContext())
            {
                return db.Spel
                    .ToList()
                    .Select(s => new UiGame(s.SpelId, s.ToString()))
                    .ToList();
            }
        }

        public List<UiMember> GetAvailableOrganizers()
        {
            using (var db = new AppDbContext())
            {
                return db.Medlemmar
                    .Where(m => m.Roll == Roll.Arrangör)
                    .ToList()
                    .Select(m => new UiMember(m.MedlemsId, m.ToString()))
                    .ToList();
            }
        }

        public string FormatEventText(Spelträff spelträff)
        {
            using (var db = new AppDbContext())
            {
                var arrangör = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == spelträff.AnsvarigArrangorId);

                string arrangörText = arrangör != null
                    ? $" | Arrangör: {arrangör}"
                    : " | Arrangör: okänd";

                return spelträff.ToString() + arrangörText;
            }
        }

        public void CreateEvent(DateTime start, string plats, AktivitetTyp typ, int max, int minAntal, string tema, int ansvarigArrangorId)
        {
            using (var db = new AppDbContext())
            {
                var ny = new Spelträff(
                    0,
                    start,
                    plats,
                    typ,
                    tema,
                    max,
                    minAntal,
                    ansvarigArrangorId,
                    new IdGenerator());

                db.Träffar.Add(ny);
                db.SaveChanges();
            }
        }
    }

    }


