using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;


namespace Laboration_2OOP.Services
{
    public class GameService
    {
        public List<Spel> GetGames()
        {
            using (var db = new AppDbContext())
            {
                return db.Spel.ToList();
            }
        }
        public void CreateGame(CreateGameInfo info)
        {
            using (var db = new AppDbContext())
            {
                var nyttSpel = new Spel(
                    0,
                    info.Titel,
                    info.Kategori,
                    info.MinAntalSpelare,
                    info.MaxAntalSpelare,
                    info.SpelTidMinuter,
                    info.Svårighetsgrad,
                    info.Beskrivning);

                nyttSpel.SättTillgänglighet(info.Tillgänglighet);

                db.Spel.Add(nyttSpel);
                db.SaveChanges();
            }
        }
        public void UpdateGame(int id, UpdateGameInfo info)
        {
            using (var db = new AppDbContext())
            {
                var spel = db.Spel.FirstOrDefault(s => s.SpelId == id);

                if (spel == null)
                    throw new Exception("Spelet hittades inte i databasen.");

                spel.UppdateraInfo(info);
                spel.SättTillgänglighet(info.Tillgänglighet);

                db.SaveChanges();
            }
        }
        public Spel? GetGameById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Spel.FirstOrDefault(s => s.SpelId == id);
            }
        }
    }
}