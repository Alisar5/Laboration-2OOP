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
    public class GamesViewModel : INotifyPropertyChanged
    {
        private GameService? _gameService;
        private Action<string>? _logAction;
        private Action? _refreshUc2GamesAction;

        public string SectionTitle { get; set; } = "Spel";

        public ObservableCollection<UiGame> GameTexts { get; set; } = new ObservableCollection<UiGame>();
        public ObservableCollection<string> GroupedGames { get; set; } = new ObservableCollection<string>();
        

        private UiGame? _selectedGame;
        public UiGame? SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged();

                if (_selectedGame != null)
                {
                    LoadSelectedGameIntoForm(_selectedGame.Id);
                }
            }
        }

        private string _gameTitle = "";
        public string GameTitle
        {
            get => _gameTitle;
            set
            {
                _gameTitle = value;
                OnPropertyChanged();
            }
        }

        private string _minPlayers = "";
        public string MinPlayers
        {
            get => _minPlayers;
            set
            {
                _minPlayers = value;
                OnPropertyChanged();
            }
        }

        private string _maxPlayers = "";
        public string MaxPlayers
        {
            get => _maxPlayers;
            set
            {
                _maxPlayers = value;
                OnPropertyChanged();
            }
        }

        private string _gameTime = "";
        public string GameTime
        {
            get => _gameTime;
            set
            {
                _gameTime = value;
                OnPropertyChanged();
            }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private Spelkategori _selectedCategory;
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

        private Svårighetsgrad _selectedDifficulty;
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

        private TillgänglighetStatus _selectedAvailability;
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

        public ICommand? RensaCommand { get; set; }
        public ICommand? UppdateraCommand { get; set; }
        public ICommand? RegistreraCommand { get; set; }
        public ICommand? GrupperaCommand { get; set; }

        public void Init(GameService gameService, Action<string> logAction, Action refreshUc2GamesAction)
        {
            _gameService = gameService;
            _logAction = logAction;
            _refreshUc2GamesAction = refreshUc2GamesAction;

            RegistreraCommand = new Kommando(RegisterGame);
            UppdateraCommand = new Kommando(UpdateGame);
            RensaCommand = new Kommando(ClearForm);
            GrupperaCommand = new Kommando(GroupGamesByCategory);

            SelectedCategory = AvailableCategories[0];
            SelectedDifficulty = AvailableDifficulties[0];
            SelectedAvailability = AvailableAvailabilities[0];

            LoadGames();
        }

        public void LoadGames()
        {
            if (_gameService == null) return;

            var source = _gameService.GetGames();

            GameTexts.Clear();

            foreach (var s in source)
            {
                GameTexts.Add(new UiGame(s.SpelId, s.ToString()));
            }
        }
        private void GroupGamesByCategory()
        {
            if (_gameService == null) return;

            var source = _gameService.GetGames();

            GroupedGames.Clear();

            var grupper = source
                .GroupBy(s => s.Kategori)
                .OrderBy(g => g.Key.ToString());

            foreach (var grp in grupper)
            {
                GroupedGames.Add("== " + grp.Key + " ==");

                foreach (var spel in grp)
                {
                    GroupedGames.Add("  - " + spel);
                }
            }

            _logAction?.Invoke("Visar spel grupperade efter kategori (LINQ-group).");
        }

        private void LoadSelectedGameIntoForm(int id)
        {
            if (_gameService == null) return;

            var spel = _gameService.GetGameById(id);
            if (spel == null) return;

            GameTitle = spel.Titel;
            SelectedCategory = spel.Kategori;
            MinPlayers = spel.MinAntalSpelare.ToString();
            MaxPlayers = spel.MaxAntalSpelare.ToString();
            GameTime = spel.SpelTidMinuter.ToString();
            SelectedDifficulty = spel.Svårighetsgrad;
            SelectedAvailability = spel.Tillgänglig;
            Description = spel.Beskrivning;

            _logAction?.Invoke($"Redigerar spel #{spel.SpelId}.");
        }

        private void RegisterGame()
        {
            if (_gameService == null) return;

            try
            {
                int min = int.Parse(MinPlayers.Trim());
                int max = int.Parse(MaxPlayers.Trim());
                int tid = int.Parse(GameTime.Trim());

                var info = new CreateGameInfo
                {
                    Titel = GameTitle,
                    Kategori = SelectedCategory,
                    MinAntalSpelare = min,
                    MaxAntalSpelare = max,
                    SpelTidMinuter = tid,
                    Svårighetsgrad = SelectedDifficulty,
                    Beskrivning = Description,
                    Tillgänglighet = SelectedAvailability
                };

                _gameService.CreateGame(info);

                _logAction?.Invoke("OK: Spel registrerat.");
                ClearForm();
                LoadGames();
                _refreshUc2GamesAction?.Invoke();
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void UpdateGame()
        {
            if (_gameService == null) return;

            if (SelectedGame == null)
            {
                _logAction?.Invoke("Välj ett spel i listan innan du uppdaterar.");
                return;
            }

            try
            {
                int min = int.Parse(MinPlayers.Trim());
                int max = int.Parse(MaxPlayers.Trim());
                int tid = int.Parse(GameTime.Trim());

                var info = new UpdateGameInfo
                {
                    Titel = GameTitle,
                    Kategori = SelectedCategory,
                    MinAntalSpelare = min,
                    MaxAntalSpelare = max,
                    SpelTidMinuter = tid,
                    Svårighetsgrad = SelectedDifficulty,
                    Beskrivning = Description,
                    Tillgänglighet = SelectedAvailability
                };

                _gameService.UpdateGame(SelectedGame.Id, info);

                _logAction?.Invoke($"OK: Spel #{SelectedGame.Id} uppdaterat.");
                LoadGames();
                _refreshUc2GamesAction?.Invoke();
            }
            catch (Exception ex)
            {
                _logAction?.Invoke("Fel (kontrollerat): " + ex.Message);
            }
        }

        private void ClearForm()
        {
            SelectedGame = null;
            GameTitle = "";
            MinPlayers = "";
            MaxPlayers = "";
            GameTime = "";
            Description = "";
            SelectedCategory = AvailableCategories[0];
            SelectedDifficulty = AvailableDifficulties[0];
            SelectedAvailability = AvailableAvailabilities[0];

            _logAction?.Invoke("Spelformulär rensat.");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}