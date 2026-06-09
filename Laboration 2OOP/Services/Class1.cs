
using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;


namespace Laboration_2OOP.Services
{
    public class EnrollmentService
    {
        public void EnrollMember(int memberId, int eventId)
        {
            using (var db = new AppDbContext())
            {
                var medlem = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == memberId);
                var träff = db.Träffar.FirstOrDefault(t => t.TräffId == eventId);

                if (medlem == null)
                    throw new ObjektHittasInteException("Medlem hittades inte i databasen.");

                if (träff == null)
                    throw new ObjektHittasInteException("Spelträffen hittades inte i databasen.");

                // NYT TILLÄGG: medlem måste vara aktiv
                if (medlem.Status != MedlemsStatus.Aktiv)
                    throw new InaktivMedlemException("Endast aktiva medlemmar får anmälas till en spelträff.");

                // Hämta relevanta anmälningar först från databasen
                var träffensAnmälningar = db.Anmälningar
                    .Where(a => a.SpelträffId == eventId)
                    .ToList();

                // Kontrollera om medlemmen redan är anmäld
                bool redanAnmäld = träffensAnmälningar.Any(a =>
                    a.MedlemId == memberId &&
                    a.ArAktiv);

                if (redanAnmäld)
                    throw new DubbelAnmalanException("Medlemmen är redan anmäld till spelträffen.");

                // Kontrollera lediga platser
                int aktivaAnmälningar = träffensAnmälningar.Count(a => a.ArAktiv);

                if (aktivaAnmälningar >= träff.MaxAntalDeltagare)
                    throw new FullbokadException("Träffen är fullbokad.");

                db.Anmälningar.Add(new Anmälan(0, memberId, eventId));
                db.SaveChanges();
            }
        }

        public void UnenrollMember(int memberId, int eventId)
        {
            using (var db = new AppDbContext())
            {
                var relevanta = db.Anmälningar
                    .Where(a => a.MedlemId == memberId && a.SpelträffId == eventId)
                    .ToList();

                var anmälan = relevanta.FirstOrDefault(a => a.ArAktiv);

                if (anmälan == null)
                    throw new ValideringsException("Medlemmen är inte anmäld till spelträffen.");

                anmälan.Avanmälan();
                db.SaveChanges();
            }
        }

        public List<string> GetParticipantsForEvent(int eventId)
        {
            using (var db = new AppDbContext())
            {
                var deltagarIds = db.Anmälningar
                    .Where(a => a.SpelträffId == eventId)
                    .ToList()
                    .Where(a => a.ArAktiv)
                    .Select(a => a.MedlemId)
                    .ToList();

                return db.Medlemmar
                    .Where(m => deltagarIds.Contains(m.MedlemsId))
                    .ToList()
                    .Select(m => m.ToString())
                    .ToList();
            }
        }
    }
}
