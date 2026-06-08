using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laboration_2OOP.Domän;

namespace Laboration_2OOP.Domän
{
    public class Spelträff
    {
        
        public int TräffId { get; private set; }
        public DateTime StartTid { get; private set; }
        public string Plats { get; private set; }
        public AktivitetTyp Typ { get; private set; }
        public string Tema { get; private set; }

        public int MaxAntalDeltagare { get; private set; }
        public int MinAntalDeltagare { get; private set; }

        public int AnsvarigArrangorId { get; private set; }


        private List<Anmälan> _anmälningar = new List<Anmälan>();
        private readonly List<Spel> _spel = new List<Spel>();
        private IdGenerator _anmälanIdGen = new IdGenerator(1);


        private Spelträff()
        {
        }

        


        public Spelträff(int träffId, DateTime startTid, string plats, AktivitetTyp typ, string tema, int maxAntalDeltagare, int minAntalDeltagare, int ansvarigArrangorId, IdGenerator anmälanIdGen)
        {

            TräffId = träffId;
            StartTid = startTid;
            Plats = (plats ?? "").Trim();
            Typ = typ;
            Tema = (tema ?? "").Trim();
            MaxAntalDeltagare = maxAntalDeltagare;
            MinAntalDeltagare = minAntalDeltagare;
            AnsvarigArrangorId = ansvarigArrangorId;
            _anmälanIdGen = anmälanIdGen;

            Validera();
        }


        //UC: Anmäl
        public void BokaPlats(Medlem medlem)
        {
            

            if (medlem == null) throw new ValideringsException("Medlem saknas.");

            if (!medlem.ArAktiv())
                throw new InaktivMedlemException("Endast aktiva medlemmar kan anmälas.");

            if (ArMedlemAnmald(medlem))
                throw new DubbelAnmalanException("Medlemmen är redan anmäld till denna träff.");

            if (!LedigaPlatser())
                throw new FullbokadException("Träffen är fullbokad.");

            _anmälningar.Add(new Anmälan(_anmälanIdGen.NextId(), medlem.MedlemsId, TräffId));
        }

        //UC: Avanmäl

        public void AvbokaPlats(Medlem medlem)
        {
            if (medlem == null)
                throw new ValideringsException("Medlem saknas.");
            var aktiv = _anmälningar.FirstOrDefault(a => a.MedlemId == medlem.MedlemsId && a.ArAktiv);
            if (aktiv == null)
                throw new ValideringsException("Medlemmen är inte anmäld (aktivt) till denna träff.");
            aktiv.Avanmälan();
        }

        public bool ArMedlemAnmald(Medlem medlem)
        {
            return _anmälningar.Any(a => a.MedlemId == medlem.MedlemsId && a.ArAktiv);
        }

        public bool LedigaPlatser()
        {
            return AntalAnmälda() < MaxAntalDeltagare;
        }

        public int AntalAnmälda()
        {
            return _anmälningar.Count(a => a.ArAktiv);
        }

        public int PlatserKvar()
        {
            return Math.Max(0, MaxAntalDeltagare - AntalAnmälda());
        }
        public List<int> HämtaDeltagareIds()
        {
            return _anmälningar.Where(a => a.ArAktiv).Select(a => a.MedlemId).ToList();
        }
        private readonly List<Spel> _planeradeSpel = new();



        public List<Spel> HämtaSpel()
        {
            return new List<Spel>(_spel);
        }

        public void LäggTillSpel(Spel spel)
        {
            if (spel == null)
                throw new ValideringsException("Spel saknas.");
            if (_spel.Any(s => s.SpelId == spel.SpelId)) return;
            _spel.Add(spel);
        }

        //Uppdatera grundinfo
        public void UppdateraDetaljer(DateTime nyStartTid, string nyPlats, AktivitetTyp nyTyp, string nyttTema)
        {
            StartTid = nyStartTid;
            Plats = (nyPlats ?? "").Trim();
            Typ = nyTyp;
            Tema = (nyttTema ?? "").Trim();

            Validera();
        }

        private void Validera()
        {
            if (StartTid == default(DateTime))
                throw new ValideringsException("Datum/Tid måste anges.");
            if (string.IsNullOrWhiteSpace(Plats))
                throw new ValideringsException("Plats är obligatorisk.");
            if (MaxAntalDeltagare <= 0)
                throw new ValideringsException("Max antal deltagare måste vara större än 0.");
            if (MinAntalDeltagare < 0 || MinAntalDeltagare > MaxAntalDeltagare)
                throw new ValideringsException("Minnimun antal deltagare måste vara 0 och över samt inte överstiger Max antal.");
        }

        public override string ToString()
        {
            // HH = 24-timmarsformat (18:00 visas som 18:00)
            var spelText = (_spel.Count > 0)
            ? " | Spel: " + string.Join(", ", _spel.Select(s => s.Titel)) : "";

            return $"#{TräffId} {Typ} {StartTid:yyyy-MM-dd HH:mm} @ {Plats}{spelText} (Max {MaxAntalDeltagare}, kvar {PlatserKvar()})";
        }
    }
}
