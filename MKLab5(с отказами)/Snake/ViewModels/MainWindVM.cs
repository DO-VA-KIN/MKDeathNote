using Prism.Commands;
using Prism.Mvvm;
using Snake.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Snake.Core;

namespace Snake.ViewModels
{
    class MainWindVM : BindableBase
    {
        #region переменные
        private const int _rowCount = 20;
        private const int _columnCount = 20;

        private bool _continueGame;
        public bool ContinueGame
        {
            get => _continueGame;
            private set
            {
                _continueGame = value;
                RaisePropertyChanged(nameof(ContinueGame));
            }
        }
        private string _status;
        public string Status
        {
            get => _status;
            private set
            {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        private uint _iterationsCount = 10 * (20 * 20) * (20 * 20);
        public uint IterationsCount
        {
            get => _iterationsCount;
            set
            {
                if (value > 0)
                {
                    _iterationsCount = value;
                    Settings.Values.IterationsCount = value;
                }
                RaisePropertyChanged(nameof(IterationsCount));
            }
        }

        private double[] _time = new double[1] { 0.0 };
        public double[] Time
        {
            get => _time;
            //set => _time = value;
        }

        private bool _timeSkip = true;
        public bool TimeSkip
        {
            get => _timeSkip; 
            set
            {
                _timeSkip = value;
                Settings.Values.TimeSkip = value;
                RaisePropertyChanged(nameof(TimeSkip));
            }
        }

        private ushort _speed = 250;
        public ushort Speed
        {
            get => _speed;
            set
            {
                if (value > 0 & value < 5001)
                {
                    _speed = value;
                    Settings.Values.Speed = value;
                }
                RaisePropertyChanged(nameof(Speed));
            }
        }


        HashSet<Point<ushort,ushort>> _snakesStarts = new HashSet<Point<ushort, ushort>>
        {
            new Point<ushort,ushort> {X = 5, Y = 5},
            new Point<ushort,ushort> {X = 5, Y = 15},
            new Point<ushort,ushort> {X = 15, Y = 5},
            new Point<ushort,ushort> {X = 15, Y = 15},
        };
        HashSet<Point<ushort, ushort>> SnakesStarts
        {
            get => _snakesStarts;
            set => _snakesStarts = value;
        }


        private SnakeFactory _snakes;
        public MyList<MyList<CellVM>> AllCells { get; } // = new List<List<CellVM>>();
        private int _iteration = 0;
        #endregion

        #region команды
        public DelegateCommand StartStopCommand { get; }
        private void OnStartStopExecute()
        {
            ContinueGame = !ContinueGame;
            if (ContinueGame)
            {
                if (_snakes != null)
                    _snakes.Dies();

                _snakes = new SnakeFactory(SnakesStarts, AllCells);
                _snakes.TimeSkip = TimeSkip;
                _snakes.Time = Time;
                Go();
            }
        }
        #endregion

        public MainWindVM()
        {
            Settings.Initialize();
            TimeSkip = Settings.Values.TimeSkip;
            IterationsCount = Settings.Values.IterationsCount;
            Speed = Settings.Values.Speed;
            Settings.TimerAutosave.Start();

            StartStopCommand = new DelegateCommand(OnStartStopExecute);

            AllCells = new MyList<MyList<CellVM>>(_rowCount);
            for (ushort row = 0; row < _rowCount; row++)
            {
                MyList<CellVM> rowList = new MyList<CellVM>(_columnCount);
                for (ushort column = 0; column < _columnCount; column++)
                {
                    CellVM cell = new CellVM(row, column);
                    rowList.Add(cell);
                }
                AllCells.Add(rowList);
            }
        }


        private async Task Go()
        {
            while (ContinueGame)
            {
                if (!(_iteration < IterationsCount))
                {
                    ContinueGame = false;
                    _iteration = 0;
                    return;
                }

                await Task.Delay(_speed);

                try
                {
                    do
                    {
                        _snakes.SnakesGo();
                        await Task.Delay(_speed);
                    }
                    while (Time[0] < 1 && TimeSkip);
                    Time[0] = 0;
                    _iteration++;
                }
                catch (Exception ex)
                {
                    ContinueGame = false;
                    Status = ex.Message;
                    _iteration = 0;
                }
            }
        }


    }
}
