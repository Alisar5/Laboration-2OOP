using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Domän
{
    public class Medlem
    {
        public int MedlemsId { get; private set; }
        public string Förnamn { get; private set; }
        public string Efternamn { get; private set; }
        public string Email { get; private set; }
        public string Telefon { get; private set; }
        public MedlemsStatus Status { get; private set; }
        public Roll Roll { get; private set; }
        public DateTime BlevMedlemDatum { get; private set; }

        private Medlem()
        {
        }


        public Medlem(int medlemsId, string förnamn, string efternamn, string email, string telefon, Roll roll, MedlemsStatus status, DateTime? blevMedlemDatum = null)
        {
            MedlemsId = medlemsId;

            Förnamn = (förnamn ?? "").Trim();
            Efternamn = (efternamn ?? "").Trim();
            Email = (email ?? "").Trim();
            Telefon = (telefon ?? "").Trim();

            Roll = roll;
            Status = status;
            BlevMedlemDatum = blevMedlemDatum ?? DateTime.Now;
            Validera();
        }



        public bool ArAktiv()
        {
            return Status == MedlemsStatus.Aktiv;
        }

        public void SattStatus(MedlemsStatus nyStatus)
        {
            Status = nyStatus;
        }

        public void UppdateraKontakt(string email, string telefon)
        {
            Email = (email ?? "").Trim();
            Telefon = (telefon ?? "").Trim();
            Validera();
        }

        public void UppdateraNamn(string förnamn, string efternamn)
        {
            Förnamn = (förnamn ?? "").Trim();   // ✅ FIX
            Efternamn = (efternamn ?? "").Trim();
            Validera();
        }


        private void Validera()
        {
            if (string.IsNullOrWhiteSpace(Förnamn) || string.IsNullOrWhiteSpace(Efternamn))
                throw new ValideringsException("Förnamn och efternamn är obligatoriska");

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
                throw new ValideringsException("Telefonnummer och email måste anges samt ha giltlig format");
        }

        public override string ToString()
        {
            return $"{MedlemsId}: {Förnamn} {Efternamn} {Status} {BlevMedlemDatum}";
        }
    }
}


