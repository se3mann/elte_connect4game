using System;

namespace Connect4.Persistence
{
    /// <summary>
    /// A poytogtatós amőba játék tábla típusa.
    /// </summary>
    public class Connect4Table
    {
        #region Fields

        private Int32 _tableSize; // ház méret
        private Int32[,] _fieldValues; // mezőértékek
        private Int32 _lastPlayer; //legutóbb lépett játékos
        

        #endregion

        #region Properties

        /// <summary>
        /// Játéktábla kitöltöttségének lekérdezése.
        /// </summary>
        public Boolean IsFilled 
        {
            get
            {
                foreach (Int32 value in _fieldValues)
                    if (value == 0)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Tábla hosszának lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 TableSize { get { return _tableSize; } set { _tableSize = value;  } }


        /// <summary>
        /// Az utolsónak lépett játékos lekérdezése, beállítása.
        /// </summary>
        public int LastPlayer { get { return _lastPlayer; } set { _lastPlayer = value; } }

        /// <summary>
        /// Mező értékének lekérdezése.
        /// </summary>
        /// <param name="x">Sor.</param>
        /// <param name="y">Oszlop.</param>
        /// <returns>Mező értéke.</returns>
        public Int32 this[Int32 x, Int32 y] { get { return GetValue(x, y); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Potyogós amőba játéktábla példányosítása alapból 5x5 méretre.
        /// </summary>
        public Connect4Table() : this(5) { }

        /// <summary>
        /// Potyogós amőba játéktábla példányosítása.
        /// </summary>
        /// <param name="tableSize">Játéktábla mérete.</param>
        public Connect4Table(Int32 tableSize)
        {
            if (tableSize == 5 || tableSize == 7 || tableSize == 10)
            {
                _tableSize = tableSize;
                _fieldValues = new int[tableSize, tableSize];
                _lastPlayer = 2; //Az első játékos kezd minden új tábla létrehozásakor
            } else
            {
                throw new ArgumentOutOfRangeException("The table size must be 10 or 20 or 30.", "tableSize");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Mező kitöltetlenségének lekérdezése.
        /// </summary>
        /// <param name="i">Sor indexe.</param>
        /// <param name="j">Oszlop indexe.</param>
        /// <returns>Igaz, ha a mező ki van töltve, egyébként hamis.</returns>
        public Boolean IsEmpty(Int32 i, Int32 j)
        {
            if (i < 0 || i >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("i", "The row index out of range.");
            if (j < 0 || j >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("j", "The column index is out of range.");

            return _fieldValues[i, j] == 0;
        }


        /// <summary>
        /// Mező értékének lekérdezése.
        /// </summary>
        /// <param name="i">Sor indexe.</param>
        /// <param name="j">Oszlop indexe.</param>
        /// <returns>A mező értéke.</returns>
        public Int32 GetValue(Int32 i, Int32 j)
        {
            if (i < 0 || i >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("i", "The row index out of range.");
            if (j < 0 || j >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("j", "The column index is out of range.");

            return _fieldValues[i, j];
        }

        /// <summary>
        /// Mező értékének beállítása.
        /// </summary>
        /// <param name="i">Sor indexe.</param>
        /// <param name="j">Oszlop indexe.</param>
        /// <param name="value">Érték.</param>
        public Boolean SetValue(Int32 i, Int32 j, Int32 value) 
        {
            if (i < 0 || i >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("i", "The row index out of range.");
            if (j < 0 || j >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("j", "The column index is out of range.");
            if (value < 1 || value > 2)
                throw new ArgumentOutOfRangeException("value", "The value is out of range.");
            if (!CheckStep(i, j)) // ha a beállítás érvénytelen, akkor nem végezzük el
                return false; 

            _fieldValues[i, j] = value;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="value"></param>
        public void SetValueWhenLoad(Int32 i, Int32 j, Int32 value)
        {
            _fieldValues[i, j] = value;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Lépésellenőrzés.
        /// </summary>
        /// <param name="i">Sor indexe.</param>
        /// <param name="j">Oszlop indexe.</param>
        /// <returns>Igaz, ha a lépés engedélyezett, különben hamis.</returns>
        private Boolean CheckStep(Int32 i, Int32 j)
        {

            //léptek-e már az adott mezőre
            if (_fieldValues[i, j] != 0) return false; 
            
            //van-e a mező alatt már érték, amire ráeshet az új érték
            Int32 maxRowIndex = _fieldValues.GetLength(0) - 1;
            if (i != maxRowIndex && _fieldValues[i+1,j] == 0) return false;
            else
            {
                return true;
            }
            
        }

        #endregion
    }
}
