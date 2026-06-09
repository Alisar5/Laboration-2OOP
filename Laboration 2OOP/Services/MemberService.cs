using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;


namespace Laboration_2OOP.Services
{
    public class MemberService
    {
        public List<Medlem> GetMembers(bool onlyActive)
        {
            using (var db = new AppDbContext())
            {
                return onlyActive
                    ? db.Medlemmar.Where(m => m.Status == MedlemsStatus.Aktiv).ToList()
                    : db.Medlemmar.ToList();
            }
        }

        public void CreateMember(RegisterMemberInfo info)
        {
            using (var db = new AppDbContext())
            {
                db.Medlemmar.Add(new Medlem(
                    0,
                    info.Förnamn,
                    info.Efternamn,
                    info.Email,
                    info.Telefon,
                    info.Roll,
                    info.Status,
                    info.RegistreradDatum));

                db.SaveChanges();
            }
        }
        public void UpdateMember(int id, UpdateMemberInfo info)
        {
            using (var db = new AppDbContext())
            {
                var medlem = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == id);

                if (medlem == null)
                    throw new ObjektHittasInteException("Medlem hittades inte i databasen.");

                medlem.UppdateraNamn(info.Förnamn, info.Efternamn);
                medlem.UppdateraKontakt(info.Email, info.Telefon);

                db.SaveChanges();
            }
        }
        public Medlem? GetMemberById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Medlemmar.FirstOrDefault(m => m.MedlemsId == id);
            }
        }
       public void DeactivateMember(int id)
        {
            using (var db = new AppDbContext())
            {
                var medlem = db.Medlemmar.FirstOrDefault(m =>m.MedlemsId == id);
                if (medlem == null)
                    throw new ObjektHittasInteException("Medlem hittades inte i databasen.");
                medlem.SattStatus(MedlemsStatus.Inaktiv);
                db.SaveChanges();
            }
        }
    }
}