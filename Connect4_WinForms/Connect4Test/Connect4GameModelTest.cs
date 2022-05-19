using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Moq;
using ELTE.Connect4.Model;
using ELTE.Connect4.Persistence;

namespace ELTE.Connect4.Test
{
    [TestClass]
    public class Connect4GameModelTest
    {
        private Connect4GameModel _model;
        private Connect4Table _mockedTable;
        private Mock<IConnect4DataAccess> _mock;
        
        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new Connect4Table(10);
            _mockedTable.SetValueWhenLoad(9, 0, 1);
            _mockedTable.SetValueWhenLoad(9, 1, 2);
            _mockedTable.LastPlayer = 2;
            //elõre definiált tábla, a bal alsó sarkában egy X, mellette jobbra egy O
            //második játékos lépett utoljára
            //elsõ játékos jön
            //egyik sem nyert
            //nincs döntetlen

            _mock = new Mock<IConnect4DataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállított játéktáblát fogja visszaadni

            _model = new Connect4GameModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            _model.GameAdvanced += new EventHandler<Connect4EventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<Connect4EventArgs>(Model_GameOver);
        }

        [TestMethod]
        public async Task Connect4GameModelLoadTest()
        {
            // kezdünk egy új játékot
            _model.NewGame(_model.Table.TableSize);

            // majd betöltünk egy játékot
            await _model.LoadGameAsync(String.Empty);

            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                {
                    Assert.AreEqual(_mockedTable.GetValue(i, j), _model.Table.GetValue(i, j));
                    // ellenõrizzük, valamennyi mezõ értéke megfelelõ-e
                }

            // az elsõ játékos következik-e
            Assert.AreEqual(Players.Player1, _model.CurrentPlayer);
            Assert.AreEqual(2, _model.Table.LastPlayer);

            // ellenõrizzük, hogy meghívták-e a Load mûveletet a megadott paraméterrel
            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }

        private void Model_GameAdvanced(Object sender, Connect4EventArgs e)
        {
            Assert.IsTrue(_model.Player1Time >= 0); // a játékidõ nem lehet negatív
            Assert.IsTrue(_model.Player2Time >= 0); // a játékidõ nem lehet negatív

            Assert.AreEqual(e.CurrentPlayer, _model.CurrentPlayer); // aktuális játékos
            Assert.AreEqual(e.IsDraw, false); // a két értéknek egyeznie kell
            Assert.AreEqual(e.CurrentPlayerWin, false); // 
        }

        private void Model_GameOver(Object sender, Connect4EventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver); // biztosan vége van a játéknak
            bool gameOver = e.CurrentPlayerWin || e.IsDraw;
            Assert.IsTrue(gameOver);
        }

        [TestMethod]
        public void Connect4GameModelNewGame10Test()
        {
            _model.NewGame(10);

            Assert.AreEqual(10, _model.Table.TableSize);
            Assert.AreEqual(Players.Player1, _model.CurrentPlayer);
            Assert.AreEqual(0, _model.Player2Time);
            Assert.IsFalse(_model.IsCurrentPlayerWon());

            Int32 emptyFields = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(_model.Table.IsEmpty(i,j))
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(100, emptyFields);

        }

        [TestMethod]
        public void Connect4GameModelNewGame20Test()
        {
            _model.NewGame(20);

            Assert.AreEqual(20, _model.Table.TableSize);
            Assert.AreEqual(Players.Player1, _model.CurrentPlayer);
            Assert.AreEqual(0, _model.Player2Time);
            Assert.IsFalse(_model.IsCurrentPlayerWon());

            Int32 emptyFields = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (_model.Table.IsEmpty(i, j))
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(400, emptyFields);

        }

        [TestMethod]
        public void Connect4GameModelNewGame30Test()
        {
            _model.NewGame(30);

            Assert.AreEqual(30, _model.Table.TableSize);
            Assert.AreEqual(Players.Player1, _model.CurrentPlayer);
            Assert.AreEqual(0, _model.Player2Time);
            Assert.IsFalse(_model.IsCurrentPlayerWon());

            Int32 emptyFields = 0;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (_model.Table.IsEmpty(i, j))
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(900, emptyFields);

        }

        [TestMethod]
        public void Connect4GameModelStepTest()
        { 
            _model.NewGame();

            int x = 9, y = 0; //szabályos helyre lépünk
            int u = 6, v = 2; //szabálytalan helyre lépünk

            _model.Step(x, y);
            Assert.AreEqual(1, _model.Table.GetValue(x, y));
            Assert.AreEqual(Players.Player2, _model.CurrentPlayer); //sikeres lépés esetén játékost váltunk

            _model.Step(u, v);
            Assert.AreEqual(0, _model.Table.GetValue(u, v));
            Assert.IsTrue(_model.Table.IsEmpty(u, v));
            Assert.AreEqual(Players.Player2, _model.CurrentPlayer); //sikertelen lépés esetén nem váltunk játékost, eddig az elsõ lépett egyszer


        }

        [TestMethod]
        public void Connect4GameModelAdvanceTimeTest()
        {
            _model.NewGame();

            Int32 time1 = _model.Player1Time;
            for (int i = 0; i < 10; i++)
            {
                _model.AdvanceTime();
            }

            Assert.AreEqual(10, _model.Player1Time); // 10-et léptünk az idõben
        }
    }
}

