using System;

namespace ELTE.Connect4.Model
{
    /// <summary>
    /// Sudoku eseményargumentum típusa.
    /// </summary>
    public class Connect4EventArgs : EventArgs
    {
        private Players _currentPlayer;
        private Int32 _currentPlayerTime;
        //private Int32 _player2Time;
        private Boolean _currentPlayerWin;
        private Boolean _isDraw;
        
        

        /// <summary>
        /// Aktuális játékos lekérdezése.
        /// </summary>
        public Int32 CurrentPlayerTime { get { return _currentPlayerTime; } }

        /// <summary>
        /// Játékidő lekérdezése az aktuális játékosnál.
        /// </summary>
        public Players CurrentPlayer { get { return _currentPlayer; } }

        /// <summary>
        /// Döntetlen lekérdezése.
        /// </summary>
        public Boolean IsDraw { get { return _isDraw; } }

        /// <summary>
        /// Győzelem lekérdezése.
        /// </summary>
        public Boolean CurrentPlayerWin { get { return _currentPlayerWin; } }

        /// <summary>
        /// Connect 4 eseményargumentum példányosítása.
        /// </summary>
        /// <param name="currentPlayer">Aktuális játékos lekérdezése.</param>
        /// <param name="currentPlayerTime">Aktuális játékos idejééek a lekérdezése.</param>
        /// <param name="currentPlayerWin">Aktuális játékos győzelme lekérdezése.</param>
        /// <param name="isDraw">Döntetlen lekérdezése.</param>
        public Connect4EventArgs(Players currentPlayer, Int32 currentPlayerTime, Boolean currentPlayerWin, Boolean isDraw) 
        {
            _currentPlayer = currentPlayer;
            _currentPlayerTime = currentPlayerTime;
            _currentPlayerWin = currentPlayerWin;
            _isDraw = isDraw;
        }
    }
}
