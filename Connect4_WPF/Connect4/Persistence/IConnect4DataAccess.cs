using System;
using System.Threading.Tasks;

namespace Connect4.Persistence
{
    /// <summary>
    /// Sudoku fájl kezelő felülete.
    /// </summary>
    public interface IConnect4DataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<Connect4Table> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String path, Connect4Table table);
    }
}
