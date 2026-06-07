using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.DemoData
{
    public static class SeedData
    {
        public static void SeedMembers(AppDbContext db, Demo state)
        {
            if (!db.Medlemmar.Any())
            {
                foreach (var medlem in state.Medlemmar.Alla)
                {
                    db.Medlemmar.Add(new Medlem(
                        0,
                        medlem.Förnamn,
                        medlem.Efternamn,
                        medlem.Email,
                        medlem.Telefon,
                        medlem.Roll,
                        medlem.Status,
                        medlem.BlevMedlemDatum));
                }

                db.SaveChanges();
            }
        }
        public static void SeedGames(AppDbContext db, Demo state)
        {
            if (!db.Spel.Any())
            {
                foreach (var spel in state.Spel.Alla)
                {
                    var nyttSpel = new Spel(
                        0,
                        spel.Titel,
                        spel.Kategori,
                        spel.MinAntalSpelare,
                        spel.MaxAntalSpelare,
                        spel.SpelTidMinuter,
                        spel.Svårighetsgrad,
                        spel.Beskrivning);

                    nyttSpel.SättTillgänglighet(spel.Tillgänglig);

                    db.Spel.Add(nyttSpel);
                }

                db.SaveChanges();
            }
        }

        public static void SeedEvents(AppDbContext db, Demo state)
        {
            if (!db.Träffar.Any())
            {
                foreach (var träff in state.Träffar.Alla)
                {
                    var nyTräff = new Spelträff(
                        0,
                        träff.StartTid,
                        träff.Plats,
                        träff.Typ,
                        träff.Tema,
                        träff.MaxAntalDeltagare,
                        träff.MinAntalDeltagare,
                        träff.AnsvarigArrangorId,
                        new IdGenerator());

                    db.Träffar.Add(nyTräff);
                }

                db.SaveChanges();
            }
        }
    }


}

