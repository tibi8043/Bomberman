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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            /*gameStatusStripLabel.Visible = false;
            openMapToolStrip.Enabled = true;
            pauseToolStripMenuItem.Enabled = false;
            ToolStripOnTick();
            */
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
                _viewModel.RefreshMapCollection();
            }
            else {
                _viewModel.AfterGameOver();
            }
        }
    }
}