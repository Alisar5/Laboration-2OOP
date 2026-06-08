using Laboration_2OOP;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using Laboration_2OOP.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Laboration_2OOP.ViewModels
{
    public class MembersViewModel : INotifyPropertyChanged
    {
        private MemberService? _memberService;
        private Action<string>? _logAction;
        private Action? _syncToAnmalningarAction;

        public string SectionTitle { get; set; } = "Medlemmar";

        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phone = "";
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private Roll _selectedRole;
        public Roll SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        public Roll[] AvailableRoles { get; set; } =
        {
            Roll.Medlem,
            Roll.Arrangör,
            Roll.Administratör
        };

        public ObservableCollection<UiMember> MemberTexts { get; set; } = new ObservableCollection<UiMember>();

        private UiMember? _selectedMember;
        public UiMember? SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged();

                if (_selectedMember != null)
                {
                    LoadSelectedMemberIntoForm(_selectedMember.Id);
                }
            }
        }

        private bool _onlyActiveMembers;
        public bool OnlyActiveMembers
        {
            get => _onlyActiveMembers;
            set
            {
                _onlyActiveMembers = value;
                OnPropertyChanged();
                LoadMembers();
            }
        }

        public ICommand? RensaCommand { get; set; }
        public ICommand? UppdateraCommand { get; set; }
        public ICommand? RegistreraCommand { get; set; }

        public void Init(MemberService memberService, Action<string> logAction, Action syncToAnmalningarAction)
        {
            _memberService = memberService;
            _logAction = logAction;
            _syncToAnmalningarAction = syncToAnmalningarAction;

            RegistreraCommand = new Kommando(RegisterMember);
            UppdateraCommand = new Kommando(UpdateMember);
            RensaCommand = new Kommando(ClearForm);

            SelectedRole = Roll.Medlem;
            LoadMembers();
        }

        public void LoadMembers()
        {
            if (_memberService == null) return;

            var source = _memberService.GetMembers(OnlyActiveMembers);

            MemberTexts.Clear();

            foreach (var m in source)
            {
                MemberTexts.Add(new UiMember(m.MedlemsId, m.ToString()));
            }

            _syncToAnmalningarAction?.Invoke();
        }

        private void LoadSelectedMemberIntoForm(int id)
        {
            if (_memberService == null) return;

            var medlem = _memberService.GetMemberById(id);
            if (medlem == null) return;

            FirstName = medlem.Förnamn;
            LastName = medlem.Efternamn;
            Email = medlem.Email;
            Phone = medlem.Telefon;
            SelectedRole = medlem.Roll;

            _logAction?.Invoke($"Redigerar medlem {medlem.MedlemsId}.");
        }

        private void RegisterMember()
        {
            if (_memberService == null) return;

            try
            {
                var info = new RegisterMemberInfo
                {
                    Förnamn = FirstName,
                    Efternamn = LastName,
                    Email = Email,
                    Telefon = Phone,
                    Roll = SelectedRole,
                    Status = MedlemsStatus.Aktiv,
                    RegistreradDatum = DateTime.Now
                };

                _memberService.CreateMember(info);

                _logAction?.Invoke("OK: Ny medlem registrerad.");
                ClearForm();
                LoadMembers();
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void UpdateMember()
        {
            if (_memberService == null) return;

            if (SelectedMember == null)
            {
                _logAction?.Invoke("Välj en medlem i listan innan du uppdaterar.");
                return;
            }

            try
            {
                var info = new UpdateMemberInfo
                {
                    Förnamn = FirstName,
                    Efternamn = LastName,
                    Email = Email,
                    Telefon = Phone
                };

                _memberService.UpdateMember(SelectedMember.Id, info);

                _logAction?.Invoke($"OK: Medlem {SelectedMember.Id} uppdaterad.");
                LoadMembers();
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void ClearForm()
        {
            SelectedMember = null;
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            SelectedRole = Roll.Medlem;

            _logAction?.Invoke("Formulär rensat.");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
