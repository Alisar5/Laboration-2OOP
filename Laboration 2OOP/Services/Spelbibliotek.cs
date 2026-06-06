using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Services
{
    public class Spelbibliotek
    {
        private readonly List<Spel> _spel = new List<Spel>();
        private readonly IdGenerator _idGen;

        public Spelbibliotek(IdGenerator idGen)
        {
            _idGen = idGen;
        }

        public IReadOnlyList<Spel> Alla => _spel;

        //Registrera nytt spel
        public Spel RegistreraNyttSpel(CreateGameInfo info)
        {
            var nyttSpel = new Spel(
                _idGen.NextId(),
                info.Titel,
                info.Kategori,
                info.MinAntalSpelare,
                info.MaxAntalSpelare,
                info.SpelTidMinuter,
                info.Svårighetsgrad,
                info.Beskrivning);

            nyttSpel.SättTillgänglighet(info.Tillgänglighet);

            _spel.Add(nyttSpel);
            return nyttSpel;
        }
        // Hämta spel (för uppdatering)
        public Spel Hämta(int spelId)
        {
            var spel = _spel.FirstOrDefault(s => s.SpelId == spelId);
            if (spel == null) throw new ObjektHittasInteException("Spel hittades inte.");
            return spel;
        }

        // Uppdatera spel
        public void UppdateraSpel(int spelId, UpdateGameInfo info)
        {
            var spel = Hämta(spelId);

            spel.UppdateraInfo(info);
            spel.SättTillgänglighet(info.Tillgänglighet);
        }
        // LINQ group
        public IEnumerable<IGrouping<Spelkategori, Spel>> GrupperaEfterKategori()
        {
            return _spel.GroupBy(s => s.Kategori);
        }
    }
}
