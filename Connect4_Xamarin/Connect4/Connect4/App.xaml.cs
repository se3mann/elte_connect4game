using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Connect4.Model;
using Connect4.Persistence;
using Connect4.View;
using Connect4.ViewModel;
using System.Threading.Tasks;

namespace Connect4
{
    public partial class App : Application
    {
        #region Fields

        private IConnect4DataAccess _connect4DataAccess;
        private Connect4GameModel _connect4GameModel;
        private Connect4ViewModel _connect4ViewModel;
        private GamePage _gamePage;
        private SettingsPage _settingsPage;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private LoadGamePage _loadGamePage;
        private SaveGamePage _saveGamePage;

        private Boolean _advanceTimer;
        private NavigationPage _mainPage;

        #endregion

        #region Application methods

        public App()
        {
            // játék összeállítása
            _connect4DataAccess = DependencyService.Get<IConnect4DataAccess>(); // az interfész megvalósítását automatikusan megkeresi a rendszer

            _connect4GameModel = new Connect4GameModel(_connect4DataAccess);
            _connect4GameModel.GameOver += new EventHandler<Connect4EventArgs>(Connect4GameModel_GameOver);

            _connect4ViewModel = new Connect4ViewModel(_connect4GameModel);
            _connect4ViewModel.NewGame += new EventHandler(Connect4ViewModel_NewGame);
            _connect4ViewModel.LoadGame += new EventHandler(Connect4ViewModel_LoadGame);
            _connect4ViewModel.SaveGame += new EventHandler(Connect4ViewModel_SaveGame);
            _connect4ViewModel.ExitGame += new EventHandler(Connect4ViewModel_ExitGame);

            _gamePage = new GamePage();
            _gamePage.BindingContext = _connect4ViewModel;

            _settingsPage = new SettingsPage();
            _settingsPage.BindingContext = _connect4ViewModel;

            // a játékmentések kezelésének összeállítása
            _store = DependencyService.Get<IStore>(); // a perzisztencia betöltése az adott platformon
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            _storedGameBrowserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            _loadGamePage = new LoadGamePage();
            _loadGamePage.BindingContext = _storedGameBrowserViewModel;

            _saveGamePage = new SaveGamePage();
            _saveGamePage.BindingContext = _storedGameBrowserViewModel;

            // nézet beállítása
            _mainPage = new NavigationPage(_gamePage); // egy navigációs lapot használunk fel a három nézet kezelésére

            MainPage = _mainPage;
        }

        protected override void OnStart()
        {
            _connect4GameModel.NewGame();
            _connect4ViewModel.RefreshTable();
            _advanceTimer = true; // egy logikai értékkel szabályozzuk az időzítőt
            Device.StartTimer(TimeSpan.FromSeconds(1), () => { _connect4GameModel.AdvanceTime(); return _advanceTimer; }); // elindítjuk az időzítőt
        }

        protected override void OnSleep()
        {
            _advanceTimer = false;

            // elmentjük a jelenleg folyó játékot
            try
            {
                Task.Run(async () => await _connect4GameModel.SaveGameAsync("SuspendedGame"));
            }
            catch { }
        }

        protected override void OnResume()
        {
            // betöltjük a felfüggesztett játékot, amennyiben van
            try
            {
                Task.Run(async () =>
                {
                    await _connect4GameModel.LoadGameAsync("SuspendedGame");
                    _connect4ViewModel.RefreshTable();

                    // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                    _advanceTimer = true;
                    Device.StartTimer(TimeSpan.FromSeconds(1), () => { _connect4GameModel.AdvanceTime(); return _advanceTimer; });
                });
            }
            catch { }

        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void Connect4ViewModel_NewGame(object sender, EventArgs e)
        {
            _connect4GameModel.NewGame();

            if (!_advanceTimer)
            {
                // ha nem fut az időzítő, akkor elindítjuk
                _advanceTimer = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () => { _connect4GameModel.AdvanceTime(); return _advanceTimer; });
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void Connect4ViewModel_LoadGame(object sender, System.EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_loadGamePage); // átnavigálunk a lapra
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void Connect4ViewModel_SaveGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_saveGamePage); // átnavigálunk a lapra
        }

        private async void Connect4ViewModel_ExitGame(object sender, EventArgs e)
        {
            await _mainPage.PushAsync(_settingsPage); // átnavigálunk a beállítások lapra
        }


        /// <summary>
        /// Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _connect4GameModel.LoadGameAsync(e.Name);
                _connect4ViewModel.RefreshTable();

                // sikeres betöltés
                await _mainPage.PopAsync(); // visszanavigálunk a játék táblára
                await MainPage.DisplayAlert("Potyogós amőba játék", "Sikeres betöltés.", "OK");

                // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                _advanceTimer = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () => { _connect4GameModel.AdvanceTime(); return _advanceTimer; });
            }
            catch
            {
                await MainPage.DisplayAlert("Potyogós amőba játék", "Sikertelen betöltés.", "OK");
            }
        }

        /// <summary>
        /// Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk
            _advanceTimer = false;

            try
            {
                // elmentjük a játékot
                await _connect4GameModel.SaveGameAsync(e.Name);
                await MainPage.DisplayAlert("Potyogós amőba játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await MainPage.DisplayAlert("Potyogós amőba játék", "Sikertelen mentés.", "OK");
            }
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private async void Connect4GameModel_GameOver(object sender, Connect4EventArgs e)
        {
            _advanceTimer = false;
            String strPlayer;
            if (e.CurrentPlayer == Players.Player1) strPlayer = "Játékos 1";
            else strPlayer = "Játékos 2";
            if (e.CurrentPlayerWin) // győzelemtől függő üzenet megjelenítése
            {
                await MainPage.DisplayAlert("Potyogós amőba játék", "Gratulálok, győztél " + strPlayer + "!" + Environment.NewLine +
                                             TimeSpan.FromSeconds(e.CurrentPlayerTime).ToString("g") + " ideig játszottál.",
                                            "OK");
            }
            else
            {
                await MainPage.DisplayAlert("Potyogós amőba játék", "Sajnálom, döntetlen lett!", "OK");
            }
        }

        #endregion
    }
}
