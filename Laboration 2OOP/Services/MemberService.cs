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
                string email = NormalizeEmail(info.Email);
                string phone = NormalizePhone(info.Telefon);

                bool emailExists = db.Medlemmar.Any(m =>
                    (m.Email ?? "").Trim().ToLower() == email);

                if (emailExists)
                    throw new Exception("Det finns redan en medlem med samma e-postadress.");

                bool phoneExists = db.Medlemmar.Any(m =>
                    (m.Telefon ?? "").Trim() == phone);

                if (phoneExists)
                    throw new Exception("Det finns redan en medlem med samma telefonnummer.");

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
        private string NormalizeEmail(string? email)
        {
            return (email ?? "").Trim().ToLowerInvariant();
        }

        private string NormalizePhone(string? phone)
        {
            return (phone ?? "").Trim();
        }
    }
}