using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using Laboration_2OOP.Services;
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

        private readonly ObservableCollection<UiEvent> _events = new ObservableCollection<UiEvent>();
        private readonly MemberService _memberService = new MemberService();
        private readonly GameService _gameService = new GameService();
        private readonly EventService _eventService = new EventService();
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();


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

            _vm.Members.Init(_memberService, Log, SyncMembersToAnmalningar);


            _vm.Games.Init(_gameService, Log, _vm.Events.LoadAvailableGames);


            _vm.Events.Init(_eventService, Log, SyncEventsToUc1);


            _vm.Anmälningar.InitCommands(
    () => OnEnrollClick(this, new RoutedEventArgs()),
    () => OnUnenrollClick(this, new RoutedEventArgs()));


            _state = Laboration_2OOP.DemoData.Data.Skapa();

            using (var db = new AppDbContext())
            {
                SeedData.SeedMembers(db, _state);
                SeedData.SeedGames(db, _state);
                SeedData.SeedEvents(db, _state);
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


            
            
            
            Log("Startad. UC1: välj medlem + spelträff och klicka Anmäl/Avanmäl.");
        }


        // UC1: Anmäl / Avanmäl
        private void SyncMembersToAnmalningar()
        {
            _vm.Anmälningar.Medlemmar.Clear();

            foreach (var uiMember in _vm.Members.MemberTexts)
            {
                _vm.Anmälningar.Medlemmar.Add(uiMember);
            }
        }
        private void SyncEventsToUc1()
        {
            _events.Clear();
            _vm.Anmälningar.Spelträffar.Clear();

            foreach (var uiEvent in _vm.Events.EventTexts)
            {
                _events.Add(uiEvent);
                _vm.Anmälningar.Spelträffar.Add(uiEvent);
            }
        }

        private void OnEventSelected_UC1(object sender, SelectionChangedEventArgs e)
        {
            ReloadParticipantsForSelectedEvent();
        }
        private void OnEnrollClick(object sender, RoutedEventArgs e)
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
                _enrollmentService.EnrollMember(m.Id, t.Id);

                Log("OK: Medlem anmäld till spelträff.");

                _vm.Events.LoadEvents();

                ReloadParticipantsForSelectedEvent();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
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
                _enrollmentService.UnenrollMember(m.Id, t.Id);

                Log("OK: Medlem avanmäld från spelträff.");

                _vm.Events.LoadEvents();

                ReloadParticipantsForSelectedEvent();
            }
            catch (Exception ex)
            {
                Log("Fel (kontrollerat): " + ex.Message);
            }
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
                var deltagare = _enrollmentService.GetParticipantsForEvent(selectedEvent.Id);

                if (deltagare.Count == 0)
                {
                    _vm.Anmälningar.Deltagare.Add("Inga anmälda deltagare ännu.");
                    return;
                }

                foreach (var text in deltagare)
                {
                    _vm.Anmälningar.Deltagare.Add(text);
                }
            }
            catch (Exception ex)
            {
                _vm.Anmälningar.Deltagare.Add("Fel: " + ex.Message);
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
            _vm.Events.LoadEvents();
            Log("Uppdaterade spelträffar.");
        }


        // Spel: Registrera / Uppdatera

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
        
    
