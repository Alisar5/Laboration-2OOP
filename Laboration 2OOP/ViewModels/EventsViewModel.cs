using Laboration_2OOP.Domän;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Laboration_2OOP.ViewModels
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        public string SectionTitle { get; set; } = "Spelträffar";
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

        private ObservableCollection<UiGame> _selectedGames = new ObservableCollection<UiGame>();

        public ObservableCollection<UiGame> SelectedGames
        {
            get => _selectedGames;
            set
            {
                _selectedGames = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand? SkapaCommand { get; set; }



    }
}