using System;
using System.Threading.Tasks;
using ELTE.Connect4.Persistence;

namespace ELTE.Connect4.Model
{
    /// <summary>
    /// Játékosok száma
    /// </summary>
    public enum Players { Player1, Player2 }

    /// <summary>
    /// Connect4 játék típusa.
    /// </summary>
    public class Connect4GameModel
    {

        #region Fields

        private IConnect4DataAccess _dataAccess; // adatelérés
        private Connect4Table _table; // játéktábla
        private Int32 _player1Time; // első játékos ideje
        private Int32 _player2Time; // második játékos ideje
        private Players _currentPlayer; //aktuális játékos
        private Boolean _currentPlayerWon; //aktuális játékos megnyerte

        #endregion

        #region Properties

        /// <summary>
        /// Első játékos idejének a lekérdezése.
        /// </summary>
        public Int32 Player1Time { get { return _player1Time; } }

        /// <summary>
        /// Második játékos idejének a lekérdezése.
        /// </summary>
        public Int32 Player2Time { get { return _player2Time; } }

        /// <summary>
        /// Aktuális játékos lekérdezése.
        /// </summary>
        public Players CurrentPlayer { get { return _currentPlayer; } }

        /// <summary>
        /// Játéktábla lekérdezése.
        /// </summary>
        public Connect4Table Table { get { return _table; } }

        /// <summary>
        /// Játék végének lekérdezése.
        /// </summary>
        public Boolean IsGameOver { get { return (_currentPlayerWon || _table.IsFilled); } }


        #endregion

        #region Events

        /// <summary>
        /// Játék előrehaladásának eseménye.
        /// </summary>
        public event EventHandler<Connect4EventArgs> GameAdvanced;

        /// <summary>
        /// Játék végének eseménye.
        /// </summary>
        public event EventHandler<Connect4EventArgs> GameOver;

        #endregion

        #region Constructor

        /// <summary>
        /// Connect 4 játék példányosítása.
        /// </summary>
        /// <param name="dataAccess">Az adatelérés.</param>
        public Connect4GameModel(IConnect4DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _table = new Connect4Table();
            _player1Time = 0;
            _player2Time = 0;
            _currentPlayer = Players.Player1;
            _currentPlayerWon = false;
        }

        #endregion

        #region Public game methods

        /// <summary>
        /// Új játék kezdése.
        /// </summary>
        public void NewGame()
        {
            _table = new Connect4Table();
            _player1Time = 0;
            _player2Time = 0;
            _currentPlayer = Players.Player1;
            _currentPlayerWon = false;
        }

        /// <summary>
        /// Új játék kezdése táblaméret megadásával
        /// </summary>
        /// <param name="tableSize">Tábla mérete.</param>
        public void NewGame(Int32 tableSize)
        {
            _table = new Connect4Table(tableSize);
            _player1Time = 0;
            _player2Time = 0;
            _currentPlayer = Players.Player1;
            _currentPlayerWon = false;
        }

        /// <summary>
        /// Játékidő léptetése.
        /// </summary>
        public void AdvanceTime()
        {
            if (IsGameOver) // ha már vége, nem folytathatjuk
                return;

            if (_currentPlayer == Players.Player1)
            {
                _player1Time++;
            }
            if (_currentPlayer == Players.Player2)
            {
                _player2Time++;
            }
            OnGameAdvanced(); //játék előrehaladás eseményének a kiváltása
        }


        /// <summary>
        /// Táblabeli lépés végrehajtása.
        /// </summary>
        /// <param name="i">Sor indexe.</param>
        /// <param name="j">Oszlop indexe.</param>
        public void Step(Int32 i, Int32 j)
        {
            if (IsGameOver) // ha már vége a játéknak, nem játszhatunk
                return;

            Boolean succesStep; //sikeres lépés
            Boolean playerWon; //a lépéssel nyert a játékos
            Int32 token;

            if (_currentPlayer == Players.Player1) token = 1;
            else token = 2;

            succesStep = _table.SetValue(i, j, token);
            if (succesStep)
            {
                playerWon = IsCurrentPlayerWon();
                if (playerWon)
                {
                    _currentPlayerWon = true;
                    OnGameOver(true, false); //ha nyert az egyik játékos, kiváltjuk a játék vége eseményt
                }
                else SwapPlayer(); //csak akkor cserélünk játékost, ha nem nyert az előző
            }
            else return; //ha nem sikeres a lépés, visszatérünk, nem történt semmi
            
            if (_table.IsFilled) // ha döntetlen lett, jelezzük
            {
                OnGameOver(false, true);
            }
            OnGameAdvanced(); //a játék előrehaladt 
        }

        /// <summary>
        /// Az aktuális játékos kicserélése a másik játékosra.
        /// </summary>
        public void SwapPlayer()
        {
            if (_currentPlayer == Players.Player1)
            {
                _currentPlayer = Players.Player2;
                _table.LastPlayer = 1; //Ha játékost cserélünk, akkor az előző játékos legyen az utoljára lépett játékos
            }
            else
            {
                _currentPlayer = Players.Player1;
                _table.LastPlayer = 2;
            }
        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = await _dataAccess.LoadAsync(path);
            if (_table.LastPlayer == 1)
            {
                _currentPlayer = Players.Player2; //Ha az első játékos lépett utoljára, a második jön, máskülönben az első
            } else
            {
                _currentPlayer = Players.Player1;
            }
            _player1Time = 0;
            _player2Time = 0;

        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);
        }

        /// <summary>
        /// Nyert-e az adot lépés után az aktuális játékos.
        /// </summary>
        /// <returns></returns>
        public Boolean IsCurrentPlayerWon()
        {
            Boolean win = false;
            Int32 maxIndex = _table.TableSize - 1;
            //bejárjuk a táblát sorfolytonosan a bal alsó saroktól
            for (int i = maxIndex; i >= 0; i--)
            {
                for (int j = 0; j <= maxIndex; j++)
                {
                    Console.WriteLine(i.ToString() + " " + j.ToString());
                    //vízszintes irány
                    if (j <= maxIndex - 3) win = win || CheckHorizontal(i, j, _currentPlayer); //utolsó 3 oszlopot nem kell nézni
                    //pozitív átló irány
                    if (i >= 3 && j <= maxIndex - 3) win = win || CheckPosDiagonal(i, j, _currentPlayer); //a mátrix 3 jobb szélső oszlopa és 3 felső sora elhagyva
                    //negatív átló irány
                    if (i >= 3 && j >= 3) win = win || CheckNegDiagonal (i, j, _currentPlayer); //a mátrix felső 3 sora és baloldali 3 oszlopa elhagyva
                    
                }
            }
            if (win) return true;
            else return false;

        }
        #endregion

        #region Private event methods

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
            {
                //aktuális játékos ideje
                Int32 currentPlayerTime;
                if (_currentPlayer == Players.Player1) currentPlayerTime = _player1Time;
                else currentPlayerTime = _player2Time;

                GameAdvanced(this, new Connect4EventArgs(_currentPlayer, currentPlayerTime, false, false));
            }

        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="isWon">Győztünk-e a játékban.</param>
        private void OnGameOver(Boolean isWon, Boolean isDraw)
        {

            if (GameOver != null)
            {
                //aktuális játékos ideje
                Int32 currentPlayerTime;
                if (_currentPlayer == Players.Player1) currentPlayerTime = _player1Time;
                else currentPlayerTime = _player2Time;

                if (isDraw) GameOver(this, new Connect4EventArgs(_currentPlayer, currentPlayerTime, false, true));
                if (isWon) GameOver(this, new Connect4EventArgs(_currentPlayer, currentPlayerTime, true, false));
            }
        }

        /// <summary>
        /// A vízszintes irányban jobbra lépegetve van-e 4 ugyan olyan jel egymás után.
        /// </summary>
        /// <param name="x">Sor.</param>
        /// <param name="y">Oszlop.</param>
        /// <param name="player">A játékos.</param>
        /// <returns></returns>
        private Boolean CheckHorizontal(int x, int y, Players player)
        {
            Int32 token;
            if (player == Players.Player1) token = 1;
            else token = 2;

            Int32 counter = 0;
            for (int i = y; i < y+4; i++)
            {
                if (_table[x, i] == token) counter++;
            }
            if (counter > 3) return true;
            else return false;
        }

        /// <summary>
        /// Pozitív átló irányban jobbra lépegetve van-e 4 ugyan olyan jel egymás után.
        /// </summary>
        /// <param name="x">Sor.</param>
        /// <param name="y">Oszlop.</param>
        /// <param name="player">Játékos.</param>
        /// <returns></returns>
        private Boolean CheckPosDiagonal(int x, int y, Players player)
        {
            Int32 token;
            if (player == Players.Player1) token = 1;
            else token = 2;

            Int32 counter = 0;
            Int32 rowIndex = x;
            for (int i = y; i < y + 4; i++)
            {
                if (_table[rowIndex, i] == token) counter++;
                rowIndex--;
            }
            if (counter > 3) return true;
            else return false;
        }

        /// <summary>
        /// Negatív átló irányban balra lépegetve van-e 4 ugyan olyan jel egymás után.
        /// </summary>
        /// <param name="x">Sor.</param>
        /// <param name="y">Oszlop.</param>
        /// <param name="player">Játékos.</param>
        /// <returns></returns>
        private Boolean CheckNegDiagonal(int x, int y, Players player)
        {
            Int32 token;
            if (player == Players.Player1) token = 1;
            else token = 2;

            Int32 counter = 0;
            Int32 rowIndex = x;
            for (int i = y; i > y - 4; i--)
            {
                if (_table[rowIndex, i] == token) counter++;
                rowIndex--;
            }
            if (counter > 3) return true;
            else return false;
        }

        #endregion

    }
}