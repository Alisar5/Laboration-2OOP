using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Services
{
    public class SpelträffRegistrering
    {
        private readonly List<Spelträff> _träffar = new List<Spelträff>();
        private readonly IdGenerator _träffIdGen;
        private readonly IdGenerator _anmälanIdGen;

        public SpelträffRegistrering(IdGenerator träffIsGen, IdGenerator anmälanIdGen)
        {
            _träffIdGen = träffIsGen;
            _anmälanIdGen = anmälanIdGen;
        }
        public IReadOnlyList<Spelträff> Alla => _träffar;
        // US2 Skapa ny spelträff 
        public Spelträff SkapaNyTräff(CreateEventInfo info)
        {

            var träff = new Spelträff(
            _träffIdGen.NextId(),
            info.StartTid,
            info.Plats,
            info.AktivitetTyp,
            info.Tema,
            info.MaxAntalDeltagare,
            info.MinAntalDeltagare,
            info.AnsvarigArrangorId,
           _anmälanIdGen);

            foreach (var spel in info.ValdaSpel)
            {
                träff.LäggTillSpel(spel);
            }

            _träffar.Add(träff);
            return träff;

        }
        public Spelträff Hämta(int träffId)
        {
            var t = _träffar.FirstOrDefault(x => x.TräffId == träffId);
            if (t == null) throw new ObjektHittasInteException("Spelträff hittades inte ");
            return t;

        }
        //US1 kommande träffer sortera efter datum (LINQ)
        public IEnumerable<Spelträff> KommandeSorteradeEfterDatum()
        {
            return _träffar
                .Where(t => t.StartTid >= DateTime.Now)
                .OrderBy(t => t.StartTid);
        }

        // UC2: kommande träffar sorterade efter lediga platser (flest kvar först)
        public IEnumerable<Spelträff> KommandeSorteradeEfterLedigaPlatser()
        {
            return _träffar
                .Where(t => t.StartTid >= DateTime.Now)
                .OrderByDescending(t => t.PlatserKvar())  // flest lediga först
                .ThenBy(t => t.StartTid);                 // vid lika: tidigast datum först
        }
    }
}
