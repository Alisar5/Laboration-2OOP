
using Laboration_2OOP.Domän;
using Laboration_2OOP.Services;
using Laboration_2OOP.Requests;

namespace Laboration_2OOP.DemoData
{
    public static class Data
    {
        public static Demo Skapa()
        {
            var medlemIdGen = new IdGenerator(1);
            var spelIdGen = new IdGenerator(1);
            var träffIsGen = new IdGenerator(1);
            var anmälanIdGen = new IdGenerator(1);

            var medlemmar = new Medlemregistrering(medlemIdGen);
            var spel = new Spelbibliotek(spelIdGen);
            var träffar = new SpelträffRegistrering(träffIsGen, anmälanIdGen);

            var m1 = medlemmar.RegistreraNy(new RegisterMemberInfo
            {
                Förnamn = "Alisar",
                Efternamn = "Sido",
                Email = "alisar@example.com",
                Telefon = "0700000000",
                Roll = Roll.Medlem,
                Status = MedlemsStatus.Aktiv,
                RegistreradDatum = new DateTime(2021, 01, 01)
            }
            );

            var m2 = medlemmar.RegistreraNy(new RegisterMemberInfo
            {
                Förnamn = "Sam",
                Efternamn = "Nilsson",
                Email = "sam@example.com",
                Telefon = "0700000001",
                Roll = Roll.Medlem,
                Status = MedlemsStatus.Aktiv,
                RegistreradDatum = new DateTime(2019, 05, 05)
            }
            );

            var m3 = medlemmar.RegistreraNy(new RegisterMemberInfo
            {
                Förnamn = "Kim",
                Efternamn = "Andersson",
                Email = "kim@example.com",
                Telefon = "0700000002",
                Roll = Roll.Medlem,
                Status = MedlemsStatus.Inaktiv,
                RegistreradDatum = new DateTime(2019, 06, 25)
            }
            );

            var m4 = medlemmar.RegistreraNy(new RegisterMemberInfo
            {
                Förnamn = "Alex",
                Efternamn = "Karlsson",
                Email = "alex@example.com",
                Telefon = "0701111111",
                Roll = Roll.Arrangör,
                Status = MedlemsStatus.Aktiv,
                RegistreradDatum = new DateTime(2020, 10, 11)
            }
            );
            var ludo = spel.RegistreraNyttSpel(new CreateGameInfo
            {
                Titel = "Ludo",
                Kategori = Spelkategori.Familj,
                MinAntalSpelare = 2,
                MaxAntalSpelare = 4,
                SpelTidMinuter = 30,
                Svårighetsgrad = Svårighetsgrad.Lätt,
                Beskrivning = "Klassiskt familjespel."
            }
            );

            var catan = spel.RegistreraNyttSpel(new CreateGameInfo
            {
                Titel = "Catan",
                Kategori = Spelkategori.Strategi,
                MinAntalSpelare = 3,
                MaxAntalSpelare = 4,
                SpelTidMinuter = 90,
                Svårighetsgrad = Svårighetsgrad.Medel,
                Beskrivning = "Bygg och handla resurser."
            }
            );

            var hanabi = spel.RegistreraNyttSpel(new CreateGameInfo
            {
                Titel = "Hanabi",
                Kategori = Spelkategori.Samarbete,
                MinAntalSpelare = 2,
                MaxAntalSpelare = 5,
                SpelTidMinuter = 25,
                Svårighetsgrad = Svårighetsgrad.Medel,
                Beskrivning = "Samarbetsspel med dolda kort."
            }
            );

            // Spelträffar

            // Spelträffar
            var t1 = träffar.SkapaNyTräff(new CreateEventInfo
            {
                StartTid = DateTime.Now.AddDays(7).Date.AddHours(18),
                Plats = "Föreningslokalen",
                AktivitetTyp = AktivitetTyp.Öppenspelkväll,
                MaxAntalDeltagare = 2,
                MinAntalDeltagare = 0,
                Tema = "Öppet",
                AnsvarigArrangorId = m4.MedlemsId
            }
            );

            var t2 = träffar.SkapaNyTräff(new CreateEventInfo
            {
                StartTid = DateTime.Now.AddDays(14).Date.AddHours(18),
                Plats = "Biblioteket",
                AktivitetTyp = AktivitetTyp.Temakväll,
                MaxAntalDeltagare = 4,
                MinAntalDeltagare = 0,
                Tema = "Strategi",
                AnsvarigArrangorId = m4.MedlemsId
            }
            );
            t1.BokaPlats(m1);
            t1.BokaPlats(m2);
            return new Demo(medlemmar, spel, träffar);
        }
    }

    public class Demo
    {
        public Medlemregistrering Medlemmar { get; }
        public Spelbibliotek Spel { get; }
        public SpelträffRegistrering Träffar { get; }

        public Demo(Medlemregistrering medlemmar, Spelbibliotek spel, SpelträffRegistrering träffar)
        {
            Medlemmar = medlemmar;
            Spel = spel;
            Träffar = träffar;
        }
    }
}


