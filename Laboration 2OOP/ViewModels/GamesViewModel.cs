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
    public class GamesViewModel : INotifyPropertyChanged
    {
        private UiGame? _selectedGame;
        private string _gameTitle = "";
        private string _minPlayers = "";
        private string _maxPlayers = "";
        private string _gameTime = "";
        private string _description = "";
        private Spelkategori _selectedCategory;
        private Svårighetsgrad _selectedDifficulty;
        private TillgänglighetStatus _selectedAvailability;

        public string SectionTitle { get; set; } = "Spel";

        public ObservableCollection<UiGame> GameTexts { get; set; } = new ObservableCollection<UiGame>();

        public UiGame? SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged();
            }
        }

        public string GameTitle
        {
            get => _gameTitle;
            set
            {
                _gameTitle = value;
                OnPropertyChanged();
            }
        }

        public string MinPlayers
        {
            get => _minPlayers;
            set
            {
                _minPlayers = value;
                OnPropertyChanged();
            }
        }

        public string MaxPlayers
        {
            get => _maxPlayers;
            set
            {
                _maxPlayers = value;
                OnPropertyChanged();
            }
        }

        public string GameTime
        {
            get => _gameTime;
            set
            {
                _gameTime = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public Spelkategori SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public Spelkategori[] AvailableCategories { get; set; } =
        {
            Spelkategori.Familj,
            Spelkategori.Strategi,
            Spelkategori.Samarbete,
            Spelkategori.Party,
            Spelkategori.Kortspel
        };

        public Svårighetsgrad SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged();
            }
        }

        public Svårighetsgrad[] AvailableDifficulties { get; set; } =
        {
            Svårighetsgrad.Lätt,
            Svårighetsgrad.Medel,
            Svårighetsgrad.Svår
        };

        public TillgänglighetStatus SelectedAvailability
        {
            get => _selectedAvailability;
            set
            {
                _selectedAvailability = value;
                OnPropertyChanged();
            }
        }

        public TillgänglighetStatus[] AvailableAvailabilities { get; set; } =
        {
            TillgänglighetStatus.Tillgänglig,
            TillgänglighetStatus.Reserverad,
            TillgänglighetStatus.Otillgänglig
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
