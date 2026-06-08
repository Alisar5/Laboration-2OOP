using Laboration_2OOP.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboration_2OOP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MemberService _memberService = new MemberService();
        private readonly GameService _gameService = new GameService();
        private readonly EventService _eventService = new EventService();
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();
        private readonly AppStartupService _appStartupService = new AppStartupService();

        private string _windowTitle = "Laboration 2 OOP";
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        private string _testText = "";
        public string TestText
        {
            get => _testText;
            set
            {
                _testText = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage = "";
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public MembersViewModel Members { get; set; } = new MembersViewModel();
        public GamesViewModel Games { get; set; } = new GamesViewModel();
        public EventsViewModel Events { get; set; } = new EventsViewModel();
        public AnmälningarViewModel Anmälningar { get; set; } = new AnmälningarViewModel();

        public MainViewModel()
        {
            _appStartupService.InitializeDatabase();

            Members.Init(_memberService, Log, SyncMembersToAnmalningar);
            Events.Init(_eventService, Log, SyncEventsToUc1);
            Games.Init(_gameService, Log, Events.LoadAvailableGames);
            Anmälningar.Init(_enrollmentService, Log, Events.LoadEvents);

            Events.SelectedEventType = Events.AvailableEventTypes[0];

            StatusMessage = "Fönstret är startat." + System.Environment.NewLine +
                "Startad. UC1: välj medlem + spelträff och klicka Anmäl/Avanmäl." + System.Environment.NewLine;

        }

        public void SyncMembersToAnmalningar()
        {
            Anmälningar.Medlemmar.Clear();

            foreach (var uiMember in Members.MemberTexts)
            {
                Anmälningar.Medlemmar.Add(uiMember);
            }
        }

        public void SyncEventsToUc1()
        {
            Anmälningar.Spelträffar.Clear();

            foreach (var uiEvent in Events.EventTexts)
            {
                Anmälningar.Spelträffar.Add(uiEvent);
            }
        }

        public void Log(string msg)
        {
            StatusMessage += msg + System.Environment.NewLine;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}