using Laboration_2OOP;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Laboration_2OOP.ViewModels
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        private EventService? _eventService;
        private Action<string>? _logAction;
        private Action? _syncToUc1Action;

        public string SectionTitle { get; set; } = "Spelträffar";

        private bool _isLoadingEvents;
        public bool IsLoadingEvents
        {
            get => _isLoadingEvents;
            set
            {
                _isLoadingEvents = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UiEvent> EventTexts { get; set; } = new ObservableCollection<UiEvent>();

        private UiEvent? _selectedEvent;
        public UiEvent? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
            }
        }

        private AktivitetTyp _selectedEventType;
        public AktivitetTyp SelectedEventType
        {
            get => _selectedEventType;
            set
            {
                _selectedEventType = value;
                OnPropertyChanged();
            }
        }

        public AktivitetTyp[] AvailableEventTypes { get; set; } =
        {
            AktivitetTyp.Öppenspelkväll,
            AktivitetTyp.Turnering,
            AktivitetTyp.Temakväll
        };

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        private string _timeText = "";
        public string TimeText
        {
            get => _timeText;
            set
            {
                _timeText = value;
                OnPropertyChanged();
            }
        }

        private string _placeText = "";
        public string PlaceText
        {
            get => _placeText;
            set
            {
                _placeText = value;
                OnPropertyChanged();
            }
        }

        private UiMember? _selectedOrganizer;
        public UiMember? SelectedOrganizer
        {
            get => _selectedOrganizer;
            set
            {
                _selectedOrganizer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UiMember> AvailableOrganizers { get; set; } = new ObservableCollection<UiMember>();
        public ObservableCollection<UiGame> AvailableGames { get; set; } = new ObservableCollection<UiGame>();
        public ObservableCollection<UiGame> SelectedGames { get; set; } = new ObservableCollection<UiGame>();

        public ICommand? SkapaCommand { get; set; }
        public ICommand? RefreshCommand { get; set; }
        public void Init(EventService eventService, Action<string> logAction, Action syncToUc1Action)
        {
            _eventService = eventService;
            _logAction = logAction;
            _syncToUc1Action = syncToUc1Action;

            SkapaCommand = new Kommando(CreateEvent);
            RefreshCommand = new Kommando(RefreshEvents);


            SelectedEventType = AvailableEventTypes[0];

            LoadEvents();
            LoadAvailableGames();
            LoadAvailableOrganizers();
        }
        private async void RefreshEvents()
        {
            try
            {
                await LoadEventsAsync();
                _logAction?.Invoke("Uppdaterade spelträffar asynkront från databasen.");
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel vid asynkron laddning: " + ex.Message);
            }
           
        }
        public void LoadEvents()
        {
            if (_eventService == null) return;

            var source = _eventService.GetEventsOrderedByDate();

            EventTexts.Clear();

            foreach (var t in source)
            {
                EventTexts.Add(new UiEvent(t.TräffId, _eventService.FormatEventText(t)));
            }

            _syncToUc1Action?.Invoke();
        }
        public async Task LoadEventsAsync()
        {
            if (_eventService == null) return;
            IsLoadingEvents = true;
            try
            {
                var source = await _eventService.GetEventsOrderedByDateAsync();
                EventTexts.Clear();
                foreach (var t in source)
                {
                    EventTexts.Add(new UiEvent(t.TräffId, _eventService.FormatEventText(t)));
                }
                _syncToUc1Action?.Invoke();
            }
            finally
            {
                IsLoadingEvents = false;
            }
        }

        public void LoadAvailableGames()
        {
            if (_eventService == null) return;

            AvailableGames.Clear();

            var source = _eventService.GetAvailableGamesForUc2();

            foreach (var uiGame in source)
            {
                AvailableGames.Add(uiGame);
            }
        }

        public void LoadAvailableOrganizers()
        {
            if (_eventService == null) return;

            AvailableOrganizers.Clear();

            var source = _eventService.GetAvailableOrganizers();

            foreach (var uiMember in source)
            {
                AvailableOrganizers.Add(uiMember);
            }

            SelectedOrganizer = null;
        }

        private void CreateEvent()
        {
            if (_eventService == null) return;

            try
            {
                if (SelectedDate == null)
                    throw new Exception("Datum måste väljas.");

                DateTime date = SelectedDate.Value.Date;

                if (!DateTime.TryParseExact(TimeText.Trim(), "HH:mm",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out DateTime timePart))
                    throw new Exception("Tid måste vara i format HH:mm (t.ex. 18:00).");

                DateTime start = date.AddHours(timePart.Hour).AddMinutes(timePart.Minute);

                string plats = (PlaceText ?? "").Trim();
                if (string.IsNullOrWhiteSpace(plats))
                    throw new Exception("Plats måste anges.");

                if (SelectedOrganizer == null)
                    throw new Exception("Välj en arrangör.");

                int ansvarigArrangorId = SelectedOrganizer.Id;

                int max = 4;
                int minAntal = 0;
                string tema = "";

                _eventService.CreateEvent(start, plats, SelectedEventType, max, minAntal, tema, ansvarigArrangorId);

                _logAction?.Invoke("OK: Spelträff skapad.");

                _ = LoadEventsAsync();

                // Rensa formuläret lite lagom
                SelectedDate = null;
                TimeText = "";
                PlaceText = "";
                SelectedOrganizer = null;
                SelectedGames.Clear();
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}