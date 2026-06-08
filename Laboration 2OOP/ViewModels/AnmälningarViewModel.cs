using Laboration_2OOP;
using Laboration_2OOP.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using  System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Laboration_2OOP.ViewModels
{
    public class AnmälningarViewModel : INotifyPropertyChanged
    {
        private EnrollmentService? _enrollmentService;
        private Action<string>? _logAction;
        private Action? _refreshEventsAction;

        public string SectionTitle { get; set; } = "Anmälningar";

        public ObservableCollection<UiMember> Medlemmar { get; set; } = new ObservableCollection<UiMember>();
        public ObservableCollection<UiEvent> Spelträffar { get; set; } = new ObservableCollection<UiEvent>();
        public ObservableCollection<string> Deltagare { get; set; } = new ObservableCollection<string>();

        private UiMember? _valdMedlem;
        public UiMember? ValdMedlem
        {
            get => _valdMedlem;
            set
            {
                _valdMedlem = value;
                OnPropertyChanged();
            }
        }

        private UiEvent? _valdSpelträff;
        public UiEvent? ValdSpelträff
        {
            get => _valdSpelträff;
            set
            {
                _valdSpelträff = value;
                OnPropertyChanged();
                LoadParticipants();
            }
        }

        public ICommand? AnmälCommand { get; set; }
        public ICommand? AvanmälCommand { get; set; }

        public void Init(EnrollmentService enrollmentService, Action<string> logAction, Action refreshEventsAction)
        {
            _enrollmentService = enrollmentService;
            _logAction = logAction;
            _refreshEventsAction = refreshEventsAction;

            AnmälCommand = new Kommando(EnrollMember);
            AvanmälCommand = new Kommando(UnenrollMember);

            LoadParticipants();
        }

        public void LoadParticipants()
        {
            Deltagare.Clear();

            if (_enrollmentService == null)
                return;

            if (ValdSpelträff == null)
            {
                Deltagare.Add("Välj en spelträff för att se deltagare.");
                return;
            }

            try
            {
                var deltagare = _enrollmentService.GetParticipantsForEvent(ValdSpelträff.Id);

                if (deltagare.Count == 0)
                {
                    Deltagare.Add("Inga anmälda deltagare ännu.");
                    return;
                }

                foreach (var text in deltagare)
                {
                    Deltagare.Add(text);
                }
            }
            catch (Exception ex)
            {
                Deltagare.Add("Fel: " + ex.Message);
            }
        }

        private void EnrollMember()
        {
            if (_enrollmentService == null)
                return;

            if (ValdMedlem == null || ValdSpelträff == null)
            {
                _logAction?.Invoke("Välj både en medlem och en spelträff.");
                return;
            }

            try
            {
                int selectedEventId = ValdSpelträff.Id;

                _enrollmentService.EnrollMember(ValdMedlem.Id, ValdSpelträff.Id);

                _logAction?.Invoke("OK: Medlem anmäld till spelträff.");

                _refreshEventsAction?.Invoke();

                ValdSpelträff = Spelträffar.FirstOrDefault(e => e.Id == selectedEventId);
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void UnenrollMember()
        {
            if (_enrollmentService == null)
                return;

            if (ValdMedlem == null || ValdSpelträff == null)
            {
                _logAction?.Invoke("Välj både en medlem och en spelträff.");
                return;
            }

            try
            {
                int selectedEventId = ValdSpelträff.Id;

                _enrollmentService.UnenrollMember(ValdMedlem.Id, ValdSpelträff.Id);

                _logAction?.Invoke("OK: Medlem avanmäld från spelträff.");

                _refreshEventsAction?.Invoke();

                ValdSpelträff = Spelträffar.FirstOrDefault(e => e.Id == selectedEventId);
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

