using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Services
{
    public class Medlemregistrering
    {
        private readonly List<Medlem> _medlemar = new List<Medlem>();
        private readonly IdGenerator _idGen;

        public Medlemregistrering(IdGenerator idGen)
        {
            _idGen = idGen;
        }
        public IReadOnlyList<Medlem> Alla => _medlemar;

        // US3 registrera ny 


        public Medlem RegistreraNy(RegisterMemberInfo info)
        {
            var medlem = new Medlem(
                _idGen.NextId(),   // använd ditt riktiga metodnamn här
                info.Förnamn,
                info.Efternamn,
                info.Email,
                info.Telefon,
                info.Roll,
                info.Status,
                info.RegistreradDatum);

            _medlemar.Add(medlem);
            return medlem;
        }
        public Medlem Hämta(int medlemId)
        {
            var medlem = _medlemar.FirstOrDefault(m => m.MedlemsId == medlemId);
            if (medlem == null) throw new ObjektHittasInteException("Medlem hittades inte ");
            return medlem;
        }
        public void UppdateraMedlem(int medlemId, UpdateMemberInfo info)
        {
            UppdateraNamn(medlemId, info.Förnamn, info.Efternamn);           // nytt
            UppdateraKontakt(medlemId, info.Email, info.Telefon);
        }
        public void UppdateraKontakt(int medlemId, string email, string telefon)
        {
            var medlem = Hämta(medlemId);
            string norm = (email ?? "").Trim().ToLowerInvariant();
            if (_medlemar.Any(m => m.MedlemsId != medlemId && (m.Email ?? "").Trim().ToLowerInvariant() == norm))
                throw new DubblettException("E-postadressen används redan av en annan medlem.");
            medlem.UppdateraKontakt(email!, telefon);

        }
        public void UppdateraNamn(int medlemId, string förnamn, string efternamn)
        {
            var medlem = Hämta(medlemId);
            medlem.UppdateraNamn(förnamn, efternamn);

        }
        public void SattStatus(int medlemId, MedlemsStatus status)
        {
            Hämta(medlemId).SattStatus(status);
        }
        // LINQ filtrering 
        public IEnumerable<Medlem> HämtaAktiva()
        {
            return _medlemar.Where(m => m.ArAktiv());
        }
        // LINQ sortering 
        public IEnumerable<Medlem> sorteraEfterNamn()
        {
            return _medlemar.OrderBy(m => m.Efternamn).ThenBy(m => m.Förnamn);
        }
    }
}
