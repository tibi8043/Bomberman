using Accessibility;
using Bombazo.Model;
using Bombazo.Presistence;
using BombazoWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace BombazoWPF {
    public partial class App : Application {
        private GameModel _model = null!;
        private BombazoViewModel _viewModel = null!;
        private MainWindow _view = null!;

        public App() {
            Startup += App_Startup;
        }

        private void App_Startup(object? sender, EventArgs args) {
            // nézemodell létrehozása

            _viewModel = new BombazoViewModel();
            _viewModel.ExitEvent += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadEvent += new EventHandler(ViewModel_LoadGame);
            _viewModel.TickEvent += new EventHandler(ViewModel_Tick);
            _viewModel.AfterGameOverEvent += new EventHandler(ViewModel_AfterGameOver);
            _viewModel.ColorInstructionsEvent += new EventHandler(ViewModel_ColorInstructions);
            _viewModel.MapInstructionsEvent += new EventHandler(ViewModel_MapInstructions);
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }

        private void ViewModel_ExitGame(object? sender, EventArgs args) {
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Bombázó", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes) {
                _view.Close();
            }
        }

        private async void ViewModel_LoadGame(object? sender, EventArgs args) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Bombázó tábla betöltése";
            openFileDialog.Filter = "Txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true) {
                try {
                    _model = await GameModel.GameModelFactory(openFileDialog.FileName);
                    _model.Timer.Elapsed += ViewModel_Tick;

                    _viewModel.SetModel(_model);

                    _viewModel.InitializeTable();
                    _viewModel.TableIsReady = true;
                }
                catch (Exception exc) {
                    MessageBox.Show("Hiba a fájl beolvasásakor! " + exc.Message, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void ViewModel_AfterGameOver(object? sender, EventArgs args) {
            _viewModel.TableIsReady = false;
            _viewModel.Refresh();
            Dispatcher.Invoke(ShowMessageBoxes);
        }

        private void ShowMessageBoxes() {
            switch (_model.OverReason) {
                case GameOverType.LOSE:
                    MessageBox.Show(
                        String.Format("Elkaptak!\nEltelt idő: {0:00}:{1:00}.\nMegölt ellenségek {2}",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60,
                            _model.KilledEnemiesCount()), "Hoppá!",
                        MessageBoxButton.OK);
                    break;
                case GameOverType.DEAD:
                    MessageBox.Show(
                        String.Format(
                            "Meghaltál a robbanásban!\nEltelt idő: {0:00}:{1:00}.\nMegölt ellenségek {2}",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60,
                            _model.KilledEnemiesCount()), "Hoppá!",
                        MessageBoxButton.OK);
                    break;
                case GameOverType.WIN:
                    MessageBox.Show(
                        String.Format(
                            "Gratulálok! Nyertél!\nEltelt idő: {0:00}:{1:00}.\nMegölted az összes ellenséget.",
                            _model.ElapsedSeconds / 60, _model.ElapsedSeconds % 60), "Nyertél!",
                        MessageBoxButton.OK);
                    break;
            }

            MessageBoxResult dialogResult = MessageBox.Show(
                "Szeretnél új játékot játszani?", "Még egy menet?", MessageBoxButton.YesNoCancel);

            if (dialogResult == MessageBoxResult.Yes) _viewModel.LoadGame();
            if (dialogResult == MessageBoxResult.No) _viewModel.Exit();
        }

        private void ViewModel_Tick(object? sender, EventArgs args) {
            if (!_model.IsGameOver()) {
                _viewModel.Refresh();
            }
            else {
                _viewModel.AfterGameOver();
            }
        }

        public void ViewModel_ColorInstructions(object? sender, EventArgs args) {
            MessageBox.Show("A színek jelentése: \n" +
                            "Zöld: Játékos \n" +
                            "Piros: Ellenség \n" +
                            "Szürke: Fal \n" +
                            "Fekete: Bomba \n" +
                            "Citromsárga: Játékos és bomba egy mezőn \n" +
                            "Narancssárga: Robbanás területe \n" +
                            "Fehér: Járható útvonal", "Színek jelentése", MessageBoxButton.OK);
        }

        public void ViewModel_MapInstructions(object? sender, EventArgs args) {
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
                "Sok sikert a pályák készítéséhez! :)", "Pályák készítése", MessageBoxButton.OK);
        }
    }
}