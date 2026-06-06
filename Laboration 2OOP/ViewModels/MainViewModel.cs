using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _testText = "MVVM fungerar";
        private string _statusMessage = "Startar...";
        private string _windowTitle = "Brädhörnan";

        public string TestText
        {
            get => _testText;
            set
            {
                _testText = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        public MembersViewModel Members { get; set; } = new MembersViewModel();
        public GamesViewModel Games { get; set; } = new GamesViewModel();
        public EventsViewModel Events { get; set; } = new EventsViewModel();
        public AnmälningarViewModel Anmälningar { get; set; } = new AnmälningarViewModel();


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }



    }

}