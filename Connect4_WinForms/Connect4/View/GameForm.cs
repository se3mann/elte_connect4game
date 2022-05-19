using System;
using System.Drawing;
using System.Windows.Forms;
using ELTE.Connect4.Model;
using ELTE.Connect4.Persistence;

namespace ELTE.Connect4.View
{
    /// <summary>
    /// Játékablak típusa.
    /// </summary>
    public partial class GameForm : Form
    {
        #region Fields

        private IConnect4DataAccess _dataAccess; // adatelérés
        private Connect4GameModel _model; // játékmodell
        private Button[,] _buttonGrid; // gombrács
        private Timer _timer; // időzítő az első játékosnak

        #endregion

        #region Constructors

        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form event handlers

        /// <summary>
        /// Játékablak betöltésének eseménykezelője.
        /// </summary>
        private void GameForm_Load(Object sender, EventArgs e)
        {
            // adatelérés példányosítása
            _dataAccess = new Connect4FileDataAccess();

            // modell létrehozása és az eseménykezelők társítása
            _model = new Connect4GameModel(_dataAccess);
            _model.GameAdvanced += new EventHandler<Connect4EventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<Connect4EventArgs>(Game_GameOver);

            // időzítő létrehozása
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);


            // játéktábla és menük inicializálása
            GenerateTable();
            SetupMenus();

            SetupTable();
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Game_GameAdvanced(Object sender, Connect4EventArgs e)
        {
            // játékidő frissítése
            _toolLabelPlayer1Time.Text = TimeSpan.FromSeconds(_model.Player1Time).ToString("g");
            _toolLabelPlayer2Time.Text = TimeSpan.FromSeconds(_model.Player2Time).ToString("g");
        
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Game_GameOver(Object sender, Connect4EventArgs e)
        {
            _timer.Stop();

            foreach (Button button in _buttonGrid) // kikapcsoljuk a gombokat
                button.Enabled = false;

            _menuFileSaveGame.Enabled = false;
            String strPlayer;
            if (e.CurrentPlayer == Players.Player1) strPlayer = "Játékos 1";
            else strPlayer = "Játékos 2";

            if (e.CurrentPlayerWin) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("Gratulálok, győztél " + strPlayer + "!" + Environment.NewLine +
                                TimeSpan.FromSeconds(e.CurrentPlayerTime).ToString("g") + " ideig játszottál.",
                                "Potyogós amőba játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            if (e.IsDraw)
            {
                MessageBox.Show("Sajnálom, döntetlen lett, betelt a pálya!",
                                "Potyogós amőba játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }

        #endregion

        #region Grid event handlers

        /// <summary>
        /// Gombrács eseménykezelője.
        /// </summary>
        private void ButtonGrid_MouseClick(Object sender, MouseEventArgs e)
        {
            // a TabIndex-ből megkapjuk a sort és oszlopot
            Int32 x = ((sender as Button).TabIndex - 100) / _model.Table.TableSize;
            Int32 y = ((sender as Button).TabIndex - 100) % _model.Table.TableSize;

            //_buttonGrid[x, y].Text = y.ToString();
            _model.Step(x, y); // lépés a játékban

           
            // mező frissítése
            if (_model.Table.IsEmpty(x, y))
                _buttonGrid[x, y].Text = String.Empty;
            if (_model.Table.GetValue(x, y) == 1)
            {
                _buttonGrid[x, y].Text = "X";
                _buttonGrid[x, y].Enabled = false;
            }
            if (_model.Table.GetValue(x, y) == 2)
            {
                _buttonGrid[x, y].Text = "O";
                _buttonGrid[x, y].Enabled = false;
            }
            
        }

        #endregion

        #region Menu event handlers

        /// <summary>
        /// Új játék eseménykezelője.
        /// </summary>
        private void MenuFileNewGame_Click(Object sender, EventArgs e)
        {
            _menuFileSaveGame.Enabled = true;

            _model.NewGame(_model.Table.TableSize);
            DeleteTable();
            GenerateTable();
            SetupTable();
	        SetupMenus();

			_timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (Connect4DataException ex)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum." + Environment.NewLine + ex.Path, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuFileSaveGame.Enabled = true;
                }

                DeleteTable();
                GenerateTable();
                SetupTable();
                SetupMenus();
                _timer.Start();
            }

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (Connect4DataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuFileExit_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Connect4 játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
            else
            {
                if (restartTimer)
                    _timer.Start();
            }
        }

        /// <summary>
        /// 10x10-es táblaméret eseménykezelője.
        /// </summary>
        private void Menu10TableSize_Click(Object sender, EventArgs e)
        {
            if (_model.Table.TableSize != 10)
            {
                _model.Table.TableSize = 10;
                /*
                _model.NewGame(10);
                DeleteTable();
                GenerateTable();
                SetupTable();
                SetupMenus();
                */
            }
            
        }

        /// <summary>
        /// 20x20-es táblaméret eseménykezelője.
        /// </summary>
        private void Menu20TableSize_Click(Object sender, EventArgs e)
        {
            if (_model.Table.TableSize != 20)
            {
                _model.Table.TableSize = 20;
                /*
                _model.NewGame(20);
                DeleteTable();
                GenerateTable();
                SetupTable();
                SetupMenus();
                */
            }
        }

        /// <summary>
        /// 30x30-es táblaméret eseménykezelője.
        /// </summary>
        private void Menu30TableSize_Click(Object sender, EventArgs e)
        {
            if (_model.Table.TableSize != 30)
            {
                _model.Table.TableSize = 30;
                /*
                _model.NewGame(30);
                DeleteTable();
                GenerateTable();
                SetupTable();
                SetupMenus();
                */
            }
        }


        #endregion

        #region Timer event handlers

        /// <summary>
        /// Időzítő eseménykeztelője.
        /// </summary>
        private void Timer_Tick(Object sender, EventArgs e)
        {
            _model.AdvanceTime(); // játékidő léptetése
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Új tábla létrehozása.
        /// </summary>
        private void GenerateTable()
        {
            Int32 size = _model.Table.TableSize;
            Int32 buttonSize;
            switch (size)
            {
                case 10:
                    buttonSize = 50;
                    break;
                case 20:
                    buttonSize = 40;
                    break;
                case 30:
                    buttonSize = 30;
                    break;
                default:
                    buttonSize = 50;
                    break;
            }

            _panelTable.SuspendLayout();
            _buttonGrid = new Button[size, size];
            for (Int32 i = 0; i < size; i++)
            { 
                for (Int32 j = 0; j < size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(2 + buttonSize * j, 2 + buttonSize * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(buttonSize, buttonSize); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, buttonSize / 2, FontStyle.Bold); // betűtípus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * size + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].Parent = _panelTable;
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelő hozzárendelése minden gombhoz

                    _panelTable.Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
            }
            _panelTable.ResumeLayout();
        }
        

        /// <summary>
        /// Tábla beállítása.
        /// </summary>
        private void SetupTable() 
        {
            Int32 gridRowNumber = _buttonGrid.GetLength(0);
            Int32 gridColNumber = _buttonGrid.GetLength(1);
            for (Int32 i = 0; i < gridRowNumber; i++)
            {
                for (Int32 j = 0; j < gridColNumber; j++)
                {
                    if (_model.Table.IsEmpty(i,j)) // ha még nincs jel a mezőben
                    {
						_buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    else // ha van X vagy O a mezőben
                    {
                        switch (_model.Table.GetValue(i,j))
                        {
                            case 1:
                                _buttonGrid[i, j].Text = "X";
                                _buttonGrid[i, j].Enabled = false;
                                _buttonGrid[i, j].BackColor = Color.White;
                                break;
                            case 2:
                                _buttonGrid[i, j].Text = "O";
                                _buttonGrid[i, j].Enabled = false;
                                _buttonGrid[i, j].BackColor = Color.White;
                                break;
                        }
                    }
                }
            }

            _toolLabelPlayer1Time.Text = TimeSpan.FromSeconds(_model.Player1Time).ToString("g");
            _toolLabelPlayer2Time.Text = TimeSpan.FromSeconds(_model.Player2Time).ToString("g");
        }

        /// <summary>
        /// Menük beállítása.
        /// </summary>
        private void SetupMenus()
        {
            _menu10TableSize.Checked = (_model.Table.TableSize == 10);
            _menu20TableSize.Checked = (_model.Table.TableSize == 20);
            _menu30TableSize.Checked = (_model.Table.TableSize == 30);
        }

        /// <summary>
        /// Töröljük a játéktáblát
        /// </summary>
        private void DeleteTable()
        {
            for (int ix = this._panelTable.Controls.Count - 1; ix >= 0; ix--)
            {
                if (this._panelTable.Controls[ix] is Button) this._panelTable.Controls[ix].Dispose();
            }
        }
        #endregion
    }
}
