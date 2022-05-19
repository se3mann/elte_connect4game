using System;
using System.IO;
using System.Threading.Tasks;

namespace Connect4.Persistence
{
    /// <summary>
    /// Connect 4 fájlkezelő típusa.
    /// </summary>
    public class Connect4FileDataAccess : IConnect4DataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public async Task<Connect4Table> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét
                    Int32 lastPlayer = Int32.Parse(numbers[1]); // beolvassuk az utolsónak lépett játékost
                    Connect4Table table = new Connect4Table(tableSize)
                    {
                        LastPlayer = lastPlayer
                    }; // létrehozzuk a táblát

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetValueWhenLoad(i, j, Int32.Parse(numbers[j]));
                        }
                    }


                return table;
                }
            }
            catch (Exception ex)
            {
                throw new Connect4DataException(path);
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, Connect4Table table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.TableSize); // kiírjuk a méretet, illetve az utolsó játékost
                    await writer.WriteLineAsync(" " + table.LastPlayer);
                    for (Int32 i = 0; i < table.TableSize; i++)
                    {
                        for (Int32 j = 0; j < table.TableSize; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch 
            {
                throw new Connect4DataException(path);
            }
        }
    }
}
