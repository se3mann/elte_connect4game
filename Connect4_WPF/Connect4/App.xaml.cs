using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Connect4.View;
using Connect4.ViewModel;
using Connect4.Persistence;
using Connect4.Model;
using System.Windows.Threading;
using System.ComponentModel;
using Microsoft.Win32;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private MainWindow _view;
        private Connect4GameModel _model;
        private Connect4ViewModel _viewModel;
        private DispatcherTimer _timer;
        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new Connect4GameModel(new Connect4FileDataAccess());
            _model.GameOver += new EventHandler<Connect4EventArgs>(Model_GameOver);
            _model.NewGame();

            // nézemodell létrehozása
            _viewModel = new Connect4ViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new MainWindow
            {
                DataContext = _viewModel
            };
            _view.Closing += new CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();


            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
        }
        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Potyogós amőba", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                    _timer.Start();
            }
        }

        #endregion


        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _model.NewGame();
            _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object sender, System.EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Potyogós amőba tábla betöltése";
                openFileDialog.Filter = "Potyogós amőba tábla|*.ctl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);

                    _timer.Start();
                }
            }
            catch (Connect4DataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Potyogós amőba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Potyogós amőba tábla betöltése";
                saveFileDialog.Filter = "Potyogós amőba tábla|*.ctl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Connect4DataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Potyogós amőba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, Connect4EventArgs e)
        {
            _timer.Stop();

            String strPlayer;
            if (e.CurrentPlayer == Players.Player1) strPlayer = "Játékos 1";
            else strPlayer = "Játékos 2";

            if (e.CurrentPlayerWin) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("Gratulálok, győztél " + strPlayer + "!" + Environment.NewLine +
                                TimeSpan.FromSeconds(e.CurrentPlayerTime).ToString("g") + " ideig játszottál.",
                                "Potyogós amőba játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            if (e.IsDraw)
            {
                MessageBox.Show("Sajnálom, döntetlen lett, betelt a pálya!",
                                "Potyogós amőba játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
        }            

        #endregion

    }


}
