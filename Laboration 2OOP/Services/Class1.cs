
using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using System;
using System.Linq;
using System.Windows;

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
                    throw new Exception("Medlem hittades inte i databasen.");

                if (träff == null)
                    throw new Exception("Spelträffen hittades inte i databasen.");

                träff.BokaPlats(medlem);
                db.SaveChanges();
            }
        }

        public void UnenrollMember(int memberId, int eventId)
        {
            using (var db = new AppDbContext())
            {
                var medlem = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == memberId);
                var träff = db.Träffar.FirstOrDefault(t => t.TräffId == eventId);

                if (medlem == null)
                    throw new Exception("Medlem hittades inte i databasen.");

                if (träff == null)
                    throw new Exception("Spelträffen hittades inte i databasen.");

                träff.AvbokaPlats(medlem);
                db.SaveChanges();
            }
        }
        public List<string> GetParticipantsForEvent(int eventId)
        {
            using (var db = new AppDbContext())
            {
                var träff = db.Träffar.FirstOrDefault(t => t.TräffId == eventId);

                if (träff == null)
                    throw new Exception("Spelträffen hittades inte i databasen.");

                var deltagarIds = träff.HämtaDeltagareIds();

                return db.Medlemmar
                    .Where(m => deltagarIds.Contains(m.MedlemsId))
                    .ToList()
                    .Select(m => m.ToString())
                    .ToList();
            }
        }



    }
}

