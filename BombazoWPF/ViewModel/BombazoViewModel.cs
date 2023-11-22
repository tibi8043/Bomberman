using Bombazo.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private GameModel _model;
        private bool _tableIsReady;
        private int MapSize => GameTable.MapSize;
        private GameTable GameTable => _model?.GameTable!;

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
        public DelegateCommand TickCommand { get; private set; }

        public event EventHandler? LoadEvent;
        public event EventHandler? ExitEvent;
        public event EventHandler? TickEvent;
        public event EventHandler? AfterGameOverEvent;

        public BombazoViewModel() {
            _tableIsReady = false;
            Fields = new ObservableCollection<Field>();

            MoveUpCommand = new DelegateCommand(param => Move(UserInput.UP));
            MoveDownCommand = new DelegateCommand(param => Move(UserInput.DOWN));
            MoveLeftCommand = new DelegateCommand(param => Move(UserInput.LEFT));
            MoveRightCommand = new DelegateCommand(param => Move(UserInput.RIGHT));
            PlantBombCommand = new DelegateCommand(param => Move(UserInput.PLANT));
            PauseCommand = new DelegateCommand(param => Pause());

            ExitCommand = new DelegateCommand(param => Exit());
            LoadMapCommand = new DelegateCommand(param => LoadGame());
            TickCommand = new DelegateCommand(param => Tick());
        }

        public void SetModel(GameModel model) => _model = model;

        #region Events

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

        #endregion

        private void Move(UserInput input) {
            if (_tableIsReady) {
                _model?.PlayerInteract(input);
                if (_model!.GameOver) {
                    AfterGameOver();
                }
                RefreshMapCollection();
            }
        }

        public void RefreshMapCollection() {
            foreach (var field in Fields) {
                field.FieldType = GameTable.GetFieldType(field.Position!);
            }
        }

        public void InitializeTable() {
            Fields.Clear();
            for (int i = 0; i < GameTable.MapSize; i++) {
                for (int j = 0; j < GameTable.MapSize; j++) {
                    Position tmp = new Position(i, j);
                    Fields.Add(new Field(GameTable.GetFieldType(tmp), tmp));
                }
            }
            RefreshMapCollection();
        }
        private void Pause() {
            if (_tableIsReady) {
                _model?.PauseGame();
                OnPropertyChanged(nameof(_model.GameIsPaused));
            }
        }
    }
}