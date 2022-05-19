using System;
using System.Collections.ObjectModel;
using Connect4.Model;

namespace Connect4.ViewModel
{
    /// <summary>
    /// Sudoku nézetmodell típusa.
    /// </summary>
    public class Connect4ViewModel : ViewModelBase
    {
        #region Fields
        
        private Connect4GameModel _model; // modell

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<Connect4Field> Fields { get; set; }


        /// <summary>
        /// Első játékos idejének a lekérdezése.
        /// </summary>
        public String Player1Time { get { return TimeSpan.FromSeconds(_model.Player1Time).ToString("g"); } }

        /// <summary>
        /// Második játékos idejének a lekérdezése.
        /// </summary>
        public String Player2Time { get { return TimeSpan.FromSeconds(_model.Player2Time).ToString("g"); } }

        /// <summary>
        /// 10x10 táblaméret lekérdezése.
        /// </summary>
        public Boolean IsTableSize10
        {
            get { return _model.Table.TableSize == 10; }
            set
            {
                if (_model.Table.TableSize == 10)
                    return;

                _model.Table.TableSize = 10;
                OnPropertyChanged("IsTableSize10");
                OnPropertyChanged("IsTableSize20");
                OnPropertyChanged("IsTableSize30");
            }
        }

        /// <summary>
        /// 20x20 táblaméret lekérdezése.
        /// </summary>
        public Boolean IsTableSize20
        {
            get { return _model.Table.TableSize == 20; }
            set
            {
                if (_model.Table.TableSize == 20)
                    return;

                _model.Table.TableSize = 20;
                OnPropertyChanged("IsTableSize10");
                OnPropertyChanged("IsTableSize20");
                OnPropertyChanged("IsTableSize30");
            }
        }

        /// <summary>
        /// 30x30 táblaméret lekérdezése.
        /// </summary>
        public Boolean IsTableSize30
        {
            get { return _model.Table.TableSize == 30; }
            set
            {
                if (_model.Table.TableSize == 30)
                    return;

                _model.Table.TableSize = 30;
                OnPropertyChanged("IsTableSize10");
                OnPropertyChanged("IsTableSize20");
                OnPropertyChanged("IsTableSize30");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Connect4 nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public Connect4ViewModel(Connect4GameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.GameAdvanced += new EventHandler<Connect4EventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<Connect4EventArgs>(Model_GameOver);
			_model.GameCreated += new EventHandler<Connect4EventArgs>(Model_GameCreated);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            // játéktábla létrehozása
            Fields = new ObservableCollection<Connect4Field>();
            for (Int32 i = 0; i < _model.Table.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.TableSize; j++)
                {
                    Fields.Add(new Connect4Field
                    {
                        IsLocked = true,
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.TableSize + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }

            RefreshTable();
        }

		#endregion

		#region Private methods

		/// <summary>
		/// Tábla frissítése.
		/// </summary>
		private void RefreshTable()
        {
            foreach (Connect4Field field in Fields) // inicializálni kell a mezőket is
            {
                Int32 tmp = _model.Table[field.X, field.Y];
                switch (tmp)
                {
                    case 0:
                        field.Text = String.Empty;
                        break;
                    case 1:
                        field.Text = "X";
                        break;
                    case 2:
                        field.Text = "O";
                        break;
                    default:
                        field.Text = String.Empty;
                        break;
                }

                /*
                if (!_model.Table.IsEmpty(field.X, field.Y))
                {
                    field.Text = _model.Table[field.X, field.Y] == 1 ? "X" : "O";
                }
                else
                {
                    field.Text = String.Empty;
                }
                */

                if (!_model.Table.IsEmpty(field.X, field.Y))
                {
                    field.IsLocked = true;
                }
                //OnPropertyChanged("Text");
                //OnPropertyChanged("IsLocked");
            }

            OnPropertyChanged("Player1Time");
            OnPropertyChanged("Player2Time");
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(Int32 index)
        {
            Connect4Field field = Fields[index];

            _model.Step(field.X, field.Y);

            Int32 tmp = _model.Table[field.X, field.Y];
            switch (tmp)
            {
                case 0:
                    field.Text = String.Empty;
                    break;
                case 1:
                    field.Text = "X";
                    field.IsLocked = true;
                    break;
                case 2:
                    field.Text = "O";
                    field.IsLocked = true;
                    break;
                default:
                    field.Text = String.Empty;
                    break;
            }

        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, Connect4EventArgs e)
        {
            foreach (Connect4Field field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
            }
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object sender, Connect4EventArgs e)
        {
            OnPropertyChanged("Player1Time");
            OnPropertyChanged("Player2Time");
        }

	    /// <summary>
	    /// Játék létrehozásának eseménykezelője.
	    /// </summary>
		private void Model_GameCreated(object sender, Connect4EventArgs e)
	    {

            Fields.Clear();
            for (Int32 i = 0; i < _model.Table.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.TableSize; j++)
                {
                    Fields.Add(new Connect4Field
                    {
                        IsLocked = true,
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.TableSize + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            RefreshTable();

        }

		#endregion

		#region Event methods

		/// <summary>
		/// Új játék indításának eseménykiváltása.
		/// </summary>
		private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        #endregion
    }
}
