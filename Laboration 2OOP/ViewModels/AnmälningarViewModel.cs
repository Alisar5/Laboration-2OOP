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
    public class AnmälningarViewModel : INotifyPropertyChanged

    {
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
            }
        }

        public string SectionTitle { get; set; } = "Anmälningar";
        public ObservableCollection<UiMember> Medlemmar { get; set; } = new ObservableCollection<UiMember>();
        public ObservableCollection<UiEvent> Spelträffar { get; set; } = new ObservableCollection<UiEvent>();
        public ObservableCollection<string> Deltagare { get; set; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand? AnmälCommand { get; set; }
        public ICommand? AvanmälCommand { get; set; }
        public void InitCommands(Action enrollAction, Action unenrollAction)
        {
            AnmälCommand = new Kommando(enrollAction);
            AvanmälCommand = new Kommando(unenrollAction);
        }


    }
}