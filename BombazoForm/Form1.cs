using System.Timers;
using Bombazo.Model;

namespace BombazoForm {
    public partial class Form1 : Form {
        private GameModel _model = new GameModel();
        private bool _tableIsReady = false;
        private int MapSize => GameTable.MapSize;
        private GameTable GameTable => _model.GameTable!;

        public Form1() {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Size clientDisplay = Screen.FromControl(this).WorkingArea.Size;
            ClientSize = new Size((int)Math.Floor(clientDisplay.Width * 0.80),
                (int)Math.Floor(clientDisplay.Height * 0.80));
            MinimumSize = new Size((int)Math.Floor(clientDisplay.Width * 0.75),
                (int)Math.Floor(clientDisplay.Height * 0.75));
        }

        private void openMapToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenMap().GetAwaiter();
        }

        private async Task OpenMap() {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.Filter = "Txt Files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    try {
                        gameTableFlowLayout.Visible = false;
                        _model = await GameModel.GameModelFactory(openFileDialog.FileName);
                        InitializeTable();
                        pauseToolStripMenuItem.Enabled = true;
                        _model.Timer.Elapsed += Tick;
                        ToolStripOnTick();
                    }
                    catch (Exception exc) {
                        MessageBox.Show("Hiba a fájl beolvasásakor! " + exc.Message, "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void InitializeTable() {
            Padding zeroPadding = new Padding(0);
            Size zeroSize = new Size(0, 0);

            //Egyéb form állítások
            gameTableFlowLayout.Controls.Clear();
            this.Padding = zeroPadding;
            gameTableFlowLayout.Padding = zeroPadding;
            gameTableFlowLayout.MinimumSize = zeroSize;

            //Flowlayout kiszámolása
            int flowlayoutSize = (int)(Math.Ceiling(this.ClientSize.Height * 0.90));
            gameTableFlowLayout.Size = new Size(flowlayoutSize, flowlayoutSize);
            //PanelSize kiszámolása
            int pSizeInt = (int)Math.Ceiling((gameTableFlowLayout.Size.Height / MapSize * 0.98));

            for (int i = 0; i < MapSize; i++) {
                for (int j = 0; j < MapSize; j++) {
                    FieldType fieldType = GameTable.GetFieldType(new Position(i, j));
                    Panel p = new Panel();
                    p.Size = new Size(pSizeInt, pSizeInt);
                    p.MinimumSize = zeroSize;
                    p.Margin = zeroPadding;
                    p.Padding = new Padding(5);
                    p.BackColor = SetBgColor(fieldType);
                    gameTableFlowLayout.Controls.Add(p);
                }
            }

            gameStatusStripLabel.Text = "A játék folyamatban van";
            openMapToolStrip.Enabled = false;
            gameTableFlowLayout.Visible = true;
            _tableIsReady = true;
        }

        private void Tick(object? sender, ElapsedEventArgs args) {
            if (!_model.IsGameOver()) {
                RefreshTable();
            }
            else {
                Invoke(AfterGameOver);
            }
            ToolStripOnTick();
        }

        private void RefreshTable() {
            if (_tableIsReady) {
                int index = 0;
                for (int i = 0; i < MapSize; i++) {
                    for (int j = 0; j < MapSize; j++) {
                        FieldType fieldType = GameTable.GetFieldType(new Position(i, j));
                        Panel? p = gameTableFlowLayout.Controls[index] as Panel;
                        p!.BackColor = SetBgColor(fieldType);
                        gameTableFlowLayout.Controls.SetChildIndex(p, index++);
                    }
                }
            }
        }

        private void PlayerInteraction(object sender, KeyEventArgs e) {
            if (_tableIsReady) {
                try {
                    switch (e.KeyCode) {
                        case Keys.Up:
                            _model.PlayerInteract(UserInput.UP);
                            break;
                        case Keys.Down:
                            _model.PlayerInteract(UserInput.DOWN);
                            break;
                        case Keys.Left:
                            _model.PlayerInteract(UserInput.LEFT);
                            break;
                        case Keys.Right:
                            _model.PlayerInteract(UserInput.RIGHT);
                            break;
                        case Keys.Space:
                            _model.PlayerInteract(UserInput.PLANT);
                            break;
                        case Keys.P:
                            _model.PauseGame();
                            break;
                    }
                }
                catch (GameOverException) {
                }

                if (_model.GameOver) {
                    Invoke(AfterGameOver);
                }

                RefreshTable();
            }
        }

        private Color SetBgColor(FieldType fieldType) {
            switch (fieldType) {
                case FieldType.ENEMY:
                    return Color.Red;
                case FieldType.WALL:
                    return Color.Gray;
                case FieldType.PLAYER:
                    return Color.Green;
                case FieldType.BOMB:
                    return Color.Black;
                case FieldType.EXPLOSION:
                    return Color.Orange;
                case FieldType.PLAYERANDBOMB:
                    return Color.Yellow;
                default:
                    return Color.NavajoWhite; //path
            }
        }

        private void AfterGameOver() {
            gameStatusStripLabel.Visible = false;
            openMapToolStrip.Enabled = true;
            pauseToolStripMenuItem.Enabled = false;
            _tableIsReady = false;
            ToolStripOnTick();
            ShowMessageBoxes();
        }

        private void ShowMessageBoxes() {
            switch (_model.OverReason) {
                case GameOverType.LOSE:
                    MessageBox.Show(
                        String.Format("Elkaptak!\nEltelt idő: {0:00}:{1:00}.\nMegölt ellenségek {2}",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60,
                            _model.KilledEnemiesCount()), "Hoppá!",
                        MessageBoxButtons.OK);
                    break;
                case GameOverType.DEAD:
                    MessageBox.Show(
                        String.Format(
                            "Meghaltál a robbanásban!\nEltelt idő: {0:00}:{1:00}.\nMegölt ellenségek {2}",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60,
                            _model.KilledEnemiesCount()), "Hoppá!",
                        MessageBoxButtons.OK);
                    break;
                case GameOverType.WIN:
                    MessageBox.Show(
                        String.Format(
                            "Gratulálok! Nyertél!\nEltelt idő: {0:00}:{1:00}.\nMegölted az összes ellenséget.",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60), "Nyertél!",
                        MessageBoxButtons.OK);
                    break;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Szeretnél új játékot játszani?", "Még egy menet?", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes) OpenMap().GetAwaiter();
            if (dialogResult == DialogResult.No) this.Close();
        }

        private void ToolStripOnTick() {
            timeLabel.Text = String.Format("Eltelt idő: {0:00}:{1:00}", _model.ElapsedSeconds / 60,
                _model.ElapsedSeconds % 60);
            gameIndicator.Text = _model.IsGameOver() ? "A játék véget ért" : "Robbantsd fel az ellenségeket!";
            killedEnemiesLabel.Text = $"Felrobbantott ellenségek száma: {_model.KilledEnemiesCount()}";
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e) {
            _model.PauseGame();
            gameStatusStripLabel.Text = _model.GameIsPaused ? "A játék megállítva" : "A játék folyamatban van";
            pauseToolStripMenuItem.Text = _model.GameIsPaused ? "Folytatás" : "Megállítás";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void színekJelentéseToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("A színek jelentése: \n" +
                            "Zöld: Játékos \n" +
                            "Piros: Ellenség \n" +
                            "Szürke: Fal \n" +
                            "Fekete: Bomba \n" +
                            "Citromsárga: Játékos és bomba egy mezőn \n" +
                            "Narancssárga: Robbanás területe \n" +
                            "Fehér: Járható útvonal", "Színek jelentése", MessageBoxButtons.OK);
        }

        private void pályaKészítéseToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(
                "A pálya fontos, hogy txt fájl legyen." +
                "Az első sorban adj meg egy egész számot, ez legyen a pálya mérete. pl 10 vagy 20. Vedd figyelemeb, hogy a nagyobb pályák hosszabb töltési időt igényelnek!\n " +
                "A második sorban add meg a falak koordinátáit, ilyen módon \" jelek nélkül: \"Walls=[x,y]\" Ha több falat szeretnél megadni, " +
                "akkor a koordinátákat pontos vesszővel válaszd el pl 2,4;5,1;5,6 Fontos hogy egész számokat adj meg! \n " +
                "A harmadik sorban add meg az ellenségek koordinátáit, ilyen módon \" jelek nélkül: \"Enemies=[x,y]\" Ha több ellenséget szeretnél megadni, " +
                "akkor a koordinátákat pontosvesszővel válaszd el pl 1,5;2,1;6,8 Fontos hogy egész számokat adj meg! \n " +
                "Ügyelj arra hogy a fájl ne tartalmazzon felesleges karaktereket, szóközöket, bekezdéseket! " +
                "Vedd figyelembe, hogy a 0,0 pozícióban csak a játékos lehet, tehát a 0,0 0,1 1,0 és az 1,1 pozíciókra ne rakj semmit! " +
                "Miután elkészítetted álmaid pályáját mentsd el és nyisd meg a játékon belül és mehet is a móka! " +
                "Sok sikert a pályák készítéséhez! :)", "Pályák készítése", MessageBoxButtons.OK);
        }
    }
}