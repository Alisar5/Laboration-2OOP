 using global::Laboration_2OOP.DemoData;


    namespace Laboration_2OOP.Services
    {
        public class AppStartupService
        {
            public void InitializeDatabase()
            {
                using (var db = new AppDbContext())
                {
                   db.Database.EnsureCreated();

                    var state = Data.Skapa();

                    SeedData.SeedMembers(db, state);
                    SeedData.SeedGames(db, state);
                    SeedData.SeedEvents(db, state);
                }
            }
        }
    }
