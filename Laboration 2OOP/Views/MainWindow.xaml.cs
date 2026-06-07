using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using Laboration_2OOP.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laboration_2OOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Demo _state;

        private readonly ObservableCollection<UiMember> _members = new ObservableCollection<UiMember>();
        private readonly ObservableCollection<UiEvent> _events = new ObservableCollection<UiEvent>();

        private readonly MainViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            _vm = new MainViewModel();
            DataContext = _vm;

            _vm.Members.RensaCommand = new Kommando(() => ClearMemberForm());
            _vm.Members.UppdateraCommand = new Kommando(() => OnUpdateMemberClick(this, new RoutedEventArgs()));
            _vm.Members.RegistreraCommand = new Kommando(() => OnRegisterMemberClick(this, new RoutedEventArgs()));
            _vm.Games.RensaCommand = new Kommando(() => ClearGameForm());
            _vm.Games.UppdateraCommand = new Kommando(() => OnUpdateGameClick(this, new RoutedEventArgs()));
            _vm.Games.RegistreraCommand = new Kommando(() => OnRegisterGameClick(this, new RoutedEventArgs()));
            _vm.Events.SkapaCommand = new Kommando(() => OnCreateEventClick(this, new RoutedEventArgs()));

            _state = Laboration_2OOP.DemoData.Data.Skapa();

           



            _vm.Anmälningar.Medlemmar.Clear();

            foreach (var medlem in _state.Medlemmar.Alla)
            {
                var uiMember = new UiMember(medlem.MedlemsId, medlem.ToString());
                _vm.Anmälningar.Medlemmar.Add(uiMember);
            }


            _vm.Anmälningar.Spelträffar.Clear();

            foreach (var träff in _state.Träffar.KommandeSorteradeEfterDatum())
            {
                var uiEvent = new UiEvent(träff.TräffId, FormateraSpelträffText(träff));
                _vm.Anmälningar.Spelträffar.Add(uiEvent);
            }



            _vm.Events.EventTexts.Clear();

            foreach (var träff in _state.Träffar.KommandeSorteradeEfterLedigaPlatser())
            {
                var uiEvent = new UiEvent(träff.TräffId, FormateraSpelträffText(träff));
                _vm.Events.EventTexts.Add(uiEvent);
            }


            _vm.Games.GameTexts.Clear();

            foreach (var spel in _state.Spel.Alla)
            {
                var uiGame = new UiGame(spel.SpelId, spel.ToString());
                _vm.Games.GameTexts.Add(uiGame);
            }


            Loaded += (s, e) =>
            {
                _vm.StatusMessage = "Fönstret är startat.";
            };

            // resten av din gamla kod ska vara kvar här under





            //  använd Data + Demo


            _vm.Events.SelectedEventType = _vm.Events.AvailableEventTypes[0];

            GameCategoryCombo.ItemsSource = Enum.GetValues(typeof(Spelkategori));
            GameCategoryCombo.SelectedIndex = 0;

            GameDifficultyCombo.ItemsSource = Enum.GetValues(typeof(Svårighetsgrad));
            GameDifficultyCombo.SelectedIndex = 0;

            GameAvailabilityCombo.ItemsSource = Enum.GetValues(typeof(TillgänglighetStatus));
            GameAvailabilityCombo.SelectedItem = TillgänglighetStatus.Tillgänglig;

            _vm.Members.SelectedRole = Roll.Medlem;


            ReloadMembers();
            ReloadEvents_UC1();
            ReloadEvents_UC2();
            ReloadGames();
            ReloadSpelLista_UC2();
            ReloadArrangorCombo_UC2();

            Log("Startad. UC1: välj medlem + spelträff och klicka Anmäl/Avanmäl.");
        }
        // UC1: Anmäl / Avanmäl

        private void OnEnrollClick(object sender, RoutedEventArgs e)
        {
            var m = _vm.Anmälningar.ValdMedlem;
            var t = EventsList_UC1.SelectedItem as UiEvent;

            if (m == null || t == null)
            {
                Log("Välj både en medlem och en spelträff.");
                return;
            }

            try
            {
                var medlem = _state.Medlemmar.Hämta(m.Id);
                var traff = _state.Träffar.Hämta(t.Id);

                traff.BokaPlats(medlem);

                Log($"OK: {medlem} anmäld till {traff}. (kvar {traff.PlatserKvar()})");

                // Spara vilken träff som var vald
                int selectedTraffId = t.Id;

                // Ladda om listan
                ReloadEvents_UC1();

                // Välj samma träff igen
                EventsList_UC1.SelectedItem = _events.FirstOrDefault(x => x.Id == selectedTraffId);

                // Uppdatera deltagare direkt
                ReloadParticipantsForSelectedEvent();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }


        private void OnEventSelected_UC1(object sender, SelectionChangedEventArgs e)
        {
            ReloadParticipantsForSelectedEvent();
        }

        private void ReloadParticipantsForSelectedEvent()
        {
            _vm.Anmälningar.Deltagare.Clear();

            var selectedEvent = _vm.Anmälningar.ValdSpelträff;
            if (selectedEvent == null)
            {
                _vm.Anmälningar.Deltagare.Add("Välj en spelträff för att se deltagare.");
                return;
            }

            try
            {
                var traff = _state.Träffar.Hämta(selectedEvent.Id);
                var deltagarIds = traff.HämtaDeltagareIds();

                if (deltagarIds.Count == 0)
                {
                    _vm.Anmälningar.Deltagare.Add("Inga anmälda deltagare ännu.");
                    return;
                }

                foreach (var medlemId in deltagarIds)
                {
                    var medlem = _state.Medlemmar.Hämta(medlemId);
                    _vm.Anmälningar.Deltagare.Add(medlem.ToString());
                }
            }
            catch (Exception ex)
            {
                _vm.Anmälningar.Deltagare.Add("Fel: " + ex.Message);
            }
        }

        private void OnUnenrollClick(object sender, RoutedEventArgs e)
        {
            var m = _vm.Anmälningar.ValdMedlem;
            var t = _vm.Anmälningar.ValdSpelträff;

            if (m == null || t == null)
            {
                Log("Välj både en medlem och en spelträff.");
                return;
            }

            try
            {
                var medlem = _state.Medlemmar.Hämta(m.Id);
                var traff = _state.Träffar.Hämta(t.Id);

                traff.AvbokaPlats(medlem);

                Log($"OK: {medlem} avanmäld från {traff}. (kvar {traff.PlatserKvar()})");

                int selectedTraffId = t.Id;

                ReloadEvents_UC1();
                EventsList_UC1.SelectedItem = _events.FirstOrDefault(x => x.Id == selectedTraffId);
                ReloadParticipantsForSelectedEvent();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }

        // UC3: Registrera / Uppdatera medlem

        private void OnRegisterMemberClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var roll = _vm.Members.SelectedRole;

                var info = new RegisterMemberInfo
                {
                    Förnamn = _vm.Members.FirstName,
                    Efternamn = _vm.Members.LastName,
                    Email = _vm.Members.Email,
                    Telefon = _vm.Members.Phone,
                    Roll = roll,
                    Status = MedlemsStatus.Aktiv,
                    RegistreradDatum = DateTime.Now
                };

                using (var db = new AppDbContext())
                {
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

                Log("OK: Ny medlem registrerad.");
                ClearMemberForm();
                ReloadMembers();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }


        private void OnMemberSelectedForEdit(object sender, SelectionChangedEventArgs e)
        {
            var uiMember = _vm.Members.SelectedMember;
            if (uiMember == null) return;

            try
            {
                var medlem = _state.Medlemmar.Hämta(uiMember.Id);


                _vm.Members.FirstName = medlem.Förnamn;
                _vm.Members.LastName = medlem.Efternamn;
                _vm.Members.Email = medlem.Email;
                _vm.Members.Phone = medlem.Telefon;
                _vm.Members.SelectedRole = medlem.Roll;

                Log($"Redigerar medlem {medlem.MedlemsId}.");
            }
            catch (Exception ex)
            {
                Log("Fel: " + ex.Message);
            }
        }


        private void OnUpdateMemberClick(object sender, RoutedEventArgs e)
        {
            if (_vm.Members.SelectedMember == null)
            {
                Log("Välj en medlem i listan innan du uppdaterar.");
                return;
            }

            try
            {
                int id = _vm.Members.SelectedMember.Id;

                using (var db = new AppDbContext())
                {
                    var medlem = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == id);

                    if (medlem == null)
                        throw new Exception("Medlem hittades inte i databasen.");

                    medlem.UppdateraNamn(_vm.Members.FirstName, _vm.Members.LastName);
                    medlem.UppdateraKontakt(_vm.Members.Email, _vm.Members.Phone);

                    db.SaveChanges();
                }

                Log($"OK: Medlem {id} uppdaterad.");
                ReloadMembers();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }


        private void OnClearMemberFormClick(object sender, RoutedEventArgs e)
        {
            ClearMemberForm();
            Log("Formulär rensat.");
        }

        private void OnActiveFilterChanged(object sender, RoutedEventArgs e)
        {
            ReloadMembers();
        }
        // UC2: Skapa spelträff

        private void OnCreateEventClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_vm.Events.SelectedDate == null)
                    throw new ValideringsException("Datum måste väljas.");

                DateTime date = _vm.Events.SelectedDate.Value.Date;

                if (!DateTime.TryParseExact(_vm.Events.TimeText.Trim(), "HH:mm",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out DateTime timePart))
                    throw new ValideringsException("Tid måste vara i format HH:mm (t.ex. 18:00).");

                DateTime start = date.AddHours(timePart.Hour).AddMinutes(timePart.Minute);

                string plats = (_vm.Events.PlaceText ?? "").Trim();
                if (string.IsNullOrWhiteSpace(plats))
                    throw new ValideringsException("Plats måste anges.");


                var valdArrangor = _vm.Events.SelectedOrganizer;

                if (valdArrangor == null)
                    throw new ValideringsException("Välj en arrangör.");

                int ansvarigArrangorId = valdArrangor.Id;

                // Valda spel
                var valdaSpel = _vm.Events.SelectedGames
                   .Select(ui => _state.Spel.Hämta(ui.Id))
                   .ToList();

                int max = 4;
                int minAntal = 0;

                // Aktivitetstyp väljs av arrangören

                var typ = _vm.Events.SelectedEventType;



                // Ingen tema alls
                // Ingen tema alls
                string tema = "";

                // Skapa träff

                using (var db = new AppDbContext())
                {
                    var ny = new Spelträff(
                        0,
                        start,
                        plats,
                        typ,
                        tema,
                        max,
                        minAntal,
                        ansvarigArrangorId,
                        new IdGenerator());

                    db.Träffar.Add(ny);
                    db.SaveChanges();

                    Log($"OK: Spelträff skapad. (id={ny.TräffId}, start={ny.StartTid:yyyy-MM-dd HH:mm})");
                }

                ReloadEvents_UC2();
                ReloadEvents_UC1();

            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }


        private void ReloadSpelLista_UC2()
        {
            _vm.Events.AvailableGames.Clear();

            foreach (var s in _state.Spel.Alla)
            {
                var uiGame = new UiGame(s.SpelId, s.ToString());
                _vm.Events.AvailableGames.Add(uiGame);
            }
        }
        private void OnSelectedGamesChanged(object sender, SelectionChangedEventArgs e)     // nytt 
        {
            _vm.Events.SelectedGames.Clear();

            foreach (UiGame uiGame in SpelList_UC2.SelectedItems)
            {
                _vm.Events.SelectedGames.Add(uiGame);
            }
        }



        private void OnRefreshEventsClick(object sender, RoutedEventArgs e)
        {
            ReloadEvents_UC2();
            Log("Uppdaterade spelträffar (LINQ-sort lediga platser).");
        }

        // Spel: Registrera / Uppdatera
        private void OnGameSelectedForEdit(object sender, SelectionChangedEventArgs e)
        {
            var uiGame = _vm.Games.SelectedGame;
            if (uiGame == null) return;

            try
            {
                var spel = _state.Spel.Hämta(uiGame.Id);


                _vm.Games.GameTitle = spel.Titel;
                _vm.Games.SelectedCategory = spel.Kategori;
                _vm.Games.MinPlayers = spel.MinAntalSpelare.ToString();
                _vm.Games.MaxPlayers = spel.MaxAntalSpelare.ToString();
                _vm.Games.GameTime = spel.SpelTidMinuter.ToString();

                _vm.Games.SelectedDifficulty = spel.Svårighetsgrad;
                _vm.Games.SelectedAvailability = spel.Tillgänglig;

                _vm.Games.Description = spel.Beskrivning;

                Log($"Redigerar spel #{spel.SpelId}.");
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void OnRegisterGameClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int min = int.Parse(_vm.Games.MinPlayers.Trim());
                int max = int.Parse(_vm.Games.MaxPlayers.Trim());
                int tid = int.Parse(_vm.Games.GameTime.Trim());

                var info = new CreateGameInfo

                {
                    Titel = _vm.Games.GameTitle,
                    Kategori = _vm.Games.SelectedCategory,
                    MinAntalSpelare = min,
                    MaxAntalSpelare = max,
                    SpelTidMinuter = tid,
                    Svårighetsgrad = _vm.Games.SelectedDifficulty,
                    Beskrivning = _vm.Games.Description,
                    Tillgänglighet = _vm.Games.SelectedAvailability
                };

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

                Log("OK: Spel registrerat.");
                ClearGameForm();
                ReloadGames();
                ReloadSpelLista_UC2();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void OnUpdateGameClick(object sender, RoutedEventArgs e)
        {
            if (_vm.Games.SelectedGame == null)
            {
                Log("Välj ett spel i listan innan du uppdaterar.");
                return;
            }

            try
            {
                int id = _vm.Games.SelectedGame.Id;

                int min = int.Parse(_vm.Games.MinPlayers.Trim());
                int max = int.Parse(_vm.Games.MaxPlayers.Trim());
                int tid = int.Parse(_vm.Games.GameTime.Trim());


                using (var db = new AppDbContext())
                {
                    var spel = db.Spel.FirstOrDefault(s => s.SpelId == id);

                    if (spel == null)
                        throw new Exception("Spelet hittades inte i databasen.");

                    var info = new UpdateGameInfo
                    {
                        Titel = _vm.Games.GameTitle,
                        Kategori = _vm.Games.SelectedCategory,
                        MinAntalSpelare = min,
                        MaxAntalSpelare = max,
                        SpelTidMinuter = tid,
                        Svårighetsgrad = _vm.Games.SelectedDifficulty,
                        Beskrivning = _vm.Games.Description,
                        Tillgänglighet = _vm.Games.SelectedAvailability
                    };

                    spel.UppdateraInfo(info);
                    spel.SättTillgänglighet(info.Tillgänglighet);

                    db.SaveChanges();
                }

                Log($"OK: Spel #{id} uppdaterat.");
                ReloadGames();
                ReloadSpelLista_UC2();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
        }


        private void OnClearGameFormClick(object sender, RoutedEventArgs e)
        {
            ClearGameForm();
            Log("Spelformulär rensat.");
        }

        private void ClearGameForm()
        {
            AllGamesList.SelectedItem = null;

            _vm.Games.GameTitle = "";
            _vm.Games.MinPlayers = "";
            _vm.Games.MaxPlayers = "";
            _vm.Games.GameTime = "";
            _vm.Games.Description = "";

            _vm.Games.SelectedCategory = _vm.Games.AvailableCategories[0];
            _vm.Games.SelectedDifficulty = _vm.Games.AvailableDifficulties[0];
            _vm.Games.SelectedAvailability = _vm.Games.AvailableAvailabilities[0];
        }


        // LINQ group: spel per kategori
        private void OnGroupGamesClick(object sender, RoutedEventArgs e)
        {
            GroupedGamesList.Items.Clear();

            foreach (var grp in _state.Spel.GrupperaEfterKategori())
            {
                GroupedGamesList.Items.Add("== " + grp.Key + " ==");
                foreach (var s in grp)
                    GroupedGamesList.Items.Add("  - " + s);
            }

            Log("Visar spel grupperade efter kategori (LINQ-group).");
        }

        // Reload helpers
        private void ReloadMembers()
        {
            using (var db = new AppDbContext())
            {
                var source = _vm.Members.OnlyActiveMembers
                    ? db.Medlemmar.Where(m => m.Status == MedlemsStatus.Aktiv).ToList()
                    : db.Medlemmar.ToList();

                _members.Clear();
                _vm.Members.MemberTexts.Clear();
                _vm.Anmälningar.Medlemmar.Clear();

                foreach (var m in source)
                {
                    var uiMember = new UiMember(m.MedlemsId, m.ToString());
                    _members.Add(uiMember);
                    _vm.Members.MemberTexts.Add(uiMember);
                    _vm.Anmälningar.Medlemmar.Add(uiMember);
                }
            }
        }


        private void ReloadEvents_UC1()
        {
            using (var db = new AppDbContext())
            {
                var source = db.Träffar
                    .OrderBy(t => t.StartTid)
                    .ToList();

                _events.Clear();
                _vm.Anmälningar.Spelträffar.Clear();

                foreach (var t in source)
                {
                    var uiEvent = new UiEvent(t.TräffId, FormateraSpelträffText(t));
                    _events.Add(uiEvent);
                    _vm.Anmälningar.Spelträffar.Add(uiEvent);
                }
            }
        }



        private void ReloadEvents_UC2()
        {
            using (var db = new AppDbContext())
            {
                var source = db.Träffar
                    .OrderBy(t => t.StartTid)
                    .ToList();

                _vm.Events.EventTexts.Clear();

                foreach (var t in source)
                {
                    var uiEvent = new UiEvent(t.TräffId, FormateraSpelträffText(t));
                    _vm.Events.EventTexts.Add(uiEvent);
                }
            }
        }



        private void ReloadGames()
        {
            using (var db = new AppDbContext())
            {
                _vm.Games.GameTexts.Clear();

                foreach (var s in db.Spel.ToList())
                {
                    var uiGame = new UiGame(s.SpelId, s.ToString());
                    _vm.Games.GameTexts.Add(uiGame);
                }
            }
        }


        private void ReloadArrangorCombo_UC2()
        {
            _vm.Events.AvailableOrganizers.Clear();

            foreach (var medlem in _state.Medlemmar.Alla)
            {
                if (medlem.Roll == Roll.Arrangör)
                {
                    var uiMember = new UiMember(medlem.MedlemsId, medlem.ToString());
                    _vm.Events.AvailableOrganizers.Add(uiMember);
                }
            }

            _vm.Events.SelectedOrganizer = null;
        }



        private string FormateraSpelträffText(Spelträff spelträff)
        {
            using (var db = new AppDbContext())

            {
                var arrangör = db.Medlemmar.FirstOrDefault(m => m.MedlemsId == spelträff.AnsvarigArrangorId);


                string arrangörText = arrangör != null
                           ? $" | Arrangör: {arrangör}"
                           : " | Arrangör: okänd";

                return spelträff.ToString() + arrangörText;
            }
        }


        private void ClearMemberForm()
        {

            _vm.Members.SelectedMember = null;

            _vm.Members.FirstName = "";
            _vm.Members.LastName = "";
            _vm.Members.Email = "";
            _vm.Members.Phone = "";
            _vm.Members.SelectedRole = Roll.Medlem;
        }



        private void Log(string msg)
        {
            StatusBox.AppendText(msg + Environment.NewLine);
            StatusBox.ScrollToEnd();
        }
    }

    public class UiMember
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiMember(int id, string text) { Id = id; DisplayText = text; }
    }

    public class UiEvent
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiEvent(int id, string text) { Id = id; DisplayText = text; }
    }

    public class UiGame
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiGame(int id, string text) { Id = id; DisplayText = text; }
        public override string ToString() => DisplayText;
    }
}
        
    
