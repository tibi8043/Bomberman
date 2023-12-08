using Bombazo.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Threading;

namespace BombazoWPF.ViewModel {
    public class BombazoViewModel : ViewModelBase {
        private GameModel? _model;
        private bool _tableIsReady;        
        private GameTable GameTable => _model?.GameTable!;
        public string GameStatusText {
            get {
                if (_model!.GameOver) {
                    return "Játék vége";
                }

                if (_model!.GameIsPaused && !_model.GameOver) {
                    return "A játék meg van állítva";
                }
                return "A játék folyamatban van";
            }
        }
        public string ElapsedSeconds {
            get {
                int i = _model!.ElapsedSeconds;
                TimeSpan t = TimeSpan.FromSeconds(i);
                return t.ToString(@"mm\:ss");
            }
        }
        
        
        public int KilledEnemiesCount => _model?.KilledEnemiesCount() ?? 0;
        
        public bool TableIsReady {
            get => _tableIsReady;
            set {
                if (_tableIsReady != value) {
                    _tableIsReady = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Field> Fields { get; set; }
        public DelegateCommand LoadMapCommand { get; private set; }
        public DelegateCommand MoveUpCommand { get; private set; }
        public DelegateCommand MoveDownCommand { get; private set; }
        public DelegateCommand MoveLeftCommand { get; private set; }
        public DelegateCommand MoveRightCommand { get; private set; }
        public DelegateCommand PlantBombCommand { get; private set; }

        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand ColorInstructionsCommand { get; private set; }
        public DelegateCommand TickCommand { get; private set; }
        public DelegateCommand MapInstructionsCommand { get; private set; }
        
        public event EventHandler? LoadEvent;
        public event EventHandler? ExitEvent;
        public event EventHandler? TickEvent;
        public event EventHandler? AfterGameOverEvent;
        public event EventHandler? ColorInstructionsEvent;
        public event EventHandler? MapInstructionsEvent;
        

        public BombazoViewModel() {
            _tableIsReady = false;
            Fields = new ObservableCollection<Field>();
            MoveUpCommand = new DelegateCommand(param => Move(UserInput.UP));
            MoveDownCommand = new DelegateCommand(param => Move(UserInput.DOWN));
            MoveLeftCommand = new DelegateCommand(param => Move(UserInput.LEFT));
            MoveRightCommand = new DelegateCommand(param => Move(UserInput.RIGHT));
            PlantBombCommand = new DelegateCommand(param => Move(UserInput.PLANT));
            PauseCommand = new DelegateCommand(param => Pause());

            
            ColorInstructionsCommand = new DelegateCommand(param => ColorInstructions());
            ExitCommand = new DelegateCommand(param => Exit());
            LoadMapCommand = new DelegateCommand(param => LoadGame());
            TickCommand = new DelegateCommand(param => Tick());
            MapInstructionsCommand = new DelegateCommand(param => MapInstructions());
            
        }

        public void SetModel(GameModel model) => _model = model;

        #region Events

        private void MapInstructions() {
            MapInstructionsEvent?.Invoke(this, EventArgs.Empty);
        }
        private void ColorInstructions() {
            ColorInstructionsEvent?.Invoke(this, EventArgs.Empty);
        }
        public void LoadGame() {
            LoadEvent?.Invoke(this, EventArgs.Empty);
        }
        
        public void Exit() {
            ExitEvent?.Invoke(this, EventArgs.Empty);
        }

        private void Tick() {
            TickEvent?.Invoke(this, EventArgs.Empty);
        }

        public void AfterGameOver() {
            AfterGameOverEvent?.Invoke(this, EventArgs.Empty);
        }
        
        private void Move(UserInput input) {
            if (_tableIsReady) {
                _model?.PlayerInteract(input);
                if (_model!.GameOver) {
                    AfterGameOver();
                }
                Refresh();
            }
        }

        public void Refresh() {
            //map
            foreach (var field in Fields) {
                field.FieldType = GameTable.GetFieldType(field.Position!);
            }
            //menustrip
            OnPropertyChanged(nameof(GameStatusText));
            OnPropertyChanged(nameof(ElapsedSeconds));
            OnPropertyChanged(nameof(KilledEnemiesCount));
        }

        public void InitializeTable() {
            Fields.Clear();
            for (int i = 0; i < GameTable.MapSize; i++) {
                for (int j = 0; j < GameTable.MapSize; j++) {
                    Position tmp = new Position(i, j);
                    Fields.Add(new Field(GameTable.GetFieldType(tmp), tmp));
                }
            }
            Refresh();
        }
        private void Pause() {
            if (_tableIsReady) {
                _model?.PauseGame();
                OnPropertyChanged(nameof(_model.GameIsPaused));
            }
        }
        #endregion
    }
}
