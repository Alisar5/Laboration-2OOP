using Laboration_2OOP.Domän;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Laboration_2OOP;

namespace Laboration_2OOP.ViewModels
{
    public class MembersViewModel : INotifyPropertyChanged
    {
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
            }
        }

        public ICommand? RensaCommand { get; set; }
        public ICommand? UppdateraCommand { get; set; }
        public ICommand? RegistreraCommand { get; set; }

        public void InitCommands(Action registerAction, Action updateAction, Action clearAction)
        {
            RegistreraCommand = new Kommando(registerAction);
            UppdateraCommand = new Kommando(updateAction);
            RensaCommand = new Kommando(clearAction);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}