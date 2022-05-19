using System;
using System.IO;
using System.Threading.Tasks;
using Connect4.Droid.Persistence;
using Connect4.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace Connect4.Droid.Persistence
{
    /// <summary>
    /// Tic-Tac-Toe adatelérés megvalósítása Android platformra.
    /// </summary>
    public class AndroidDataAccess : IConnect4DataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A beolvasott mezőértékek.</returns>
        public async Task<Connect4Table> LoadAsync(String path)
        {
            // a betöltés a személyen könyvtárból történik
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // a fájlműveletet taszk segítségével végezzük (aszinkron módon)
            String[] values = (await Task.Run(() => File.ReadAllText(filePath))).Split(' ');

            Int32 tableSize = Int32.Parse(values[0]);
            Int32 lastPlayer = Int32.Parse(values[1]);
            Connect4Table table = new Connect4Table(tableSize)
            {
                LastPlayer = lastPlayer
            }; // létrehozzuk a táblát

            Int32 valueIndex = 2;
            for (Int32 rowIndex = 0; rowIndex < tableSize; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < tableSize; columnIndex++)
                {
                    table.SetValueWhenLoad(rowIndex, columnIndex, Int32.Parse(values[valueIndex])); // értékek betöltése
                    valueIndex++;
                }
            }

            return table;
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, Connect4Table table)
        {
            String text = table.TableSize.ToString() + " " + table.LastPlayer.ToString(); // méret

            for (Int32 i = 0; i < table.TableSize; i++)
            {
                for (Int32 j = 0; j < table.TableSize; j++)
                {
                    text += " " + table[i, j]; // mezőértékek
                }
            }


            // fájl létrehozása
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // kiírás (aszinkron módon)
            await Task.Run(() => File.WriteAllText(filePath, text));
        }
    }
}