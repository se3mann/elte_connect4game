using System;

namespace ELTE.Connect4.Persistence
{
    /// <summary>
    /// Sudoku adatelérés kivétel típusa.
    /// </summary>
    public class Connect4DataException : Exception
    {
        /// <summary>
        /// Connect4 adatelérés kivétel példányosítása.
        /// </summary>
        public Connect4DataException(String path)
        {
            _path = path;
        }

        String _path;
        public String Path { get { return _path; } }
    }
    
    
}
