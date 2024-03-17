using System;
using System.Collections.Generic;
using System.Linq;


using Snake.Core;

namespace Snake.Models
{
    internal class Snake
    {
        private double[] _time;
        public double[] Time
        {
            get => _time;
            set => _time = value;
        }

        public event StateHandler SnakeDie;
        public event StateHandler NewSnake;
        public delegate void StateHandler(Snake sender, CellVM Cell);

        private Random _rand = new Random();
        public Random Rand
        {
            private get => _rand;
            set => _rand = value;
        }

        private MyList<MyList<CellVM>> _allCells;
        private CellVM _current;

        public Snake(MyList<MyList<CellVM>> allCells, CellVM start)
        {
            start.CellType = CellType.Life;
            _current = start;
            _allCells = allCells;
        }



        public void Move()
        {
            ushort nextRow = _current.Row;
            ushort nextColumn = _current.Column;

            //все соседи
            HashSet<CellVM> neighbors = new HashSet<CellVM>(8)
            {
                _allCells.GetCarefull(nextRow + 1).GetCarefull(nextColumn + 1),
                _allCells.GetCarefull(nextRow + 0).GetCarefull(nextColumn + 1),
                _allCells.GetCarefull(nextRow - 1).GetCarefull(nextColumn + 1),
                _allCells.GetCarefull(nextRow + 1).GetCarefull(nextColumn + 0),
                _allCells.GetCarefull(nextRow - 1).GetCarefull(nextColumn + 0),
                _allCells.GetCarefull(nextRow + 1).GetCarefull(nextColumn - 1),
                _allCells.GetCarefull(nextRow + 0).GetCarefull(nextColumn - 1),
                _allCells.GetCarefull(nextRow - 1).GetCarefull(nextColumn - 1)
            };

            //соседи по 4 сторонам движения
            List<CellVM> movableNeighbors = new List<CellVM>
            {
                _allCells.GetCarefull(nextRow + 1).GetCarefull(nextColumn),
                _allCells.GetCarefull(nextRow - 1).GetCarefull(nextColumn),
                _allCells.GetCarefull(nextRow).GetCarefull(nextColumn + 1),
                _allCells.GetCarefull(nextRow).GetCarefull(nextColumn - 1),
            };
            //т.к. у нас система без отказов - сразу отсееваем клетки недоступные для движения
            //оставим только пустые(мёртвые) клетки
            movableNeighbors = movableNeighbors.Where(m => m.CellType == CellType.Death).ToList();

            //все живые и все пустые клетки
            HashSet<CellVM> neighborsLife = new HashSet<CellVM>(neighbors.Where(x => x.CellType == CellType.Life));
            List<CellVM> neighborsDeath = new List<CellVM>(neighbors.Where(x => x.CellType == CellType.Death));

            //лист класса событий
            List<Event> events = new List<Event>();

            //живых соседий больше 3
            if (neighborsLife.Count > 3)
            {
                //создание нового события
                Event event_ = new Event()
                {
                    //выполняемое действие события - вызов смерти
                    Execute = () => Die(),
                    //для расчёта времени
                    Speed = 2,
                };
                //добавление в лист событий
                events.Add(event_);
            }

            //есть хотя бы 1 живой сосед
            if (neighborsLife.Count > 0)
            {
                //проходимм по всем клеткам доступным для движения
                foreach (CellVM cell in movableNeighbors)
                {
                    //создание и добавления события рождения в лист событий
                    Event event_ = new Event()
                    {
                        Execute = () => NewSnake?.Invoke(this, cell),
                        Speed = 3
                    };
                    events.Add(event_);
                }
            }

            //проходимм по всем клеткам доступным для движения
            foreach (CellVM cell in movableNeighbors)
            {
                //создание и добавления события движения в лист событий
                Event event_ = new Event()
                {
                    Execute = () =>
                    {
                        _current.CellType = CellType.Death;
                        _current = cell;
                        _current.CellType = CellType.Life;
                    },
                    Speed = 1
                };
                events.Add(event_);
            }

            //выбираем случайное действие из списка событий и выполняем
            int r = Rand.Next(events.Count);//случ. число в пределах размерности листа
            TimeCalculate(r, events[r].Speed);//расчёт времени
            events[r].Execute();//выполнение действия по выбранному индексу
        }

        private void TimeCalculate(int r, int speed)
        {
            if (Time != null)
                if (Time.Length > 0)
                    Time[0] -= Math.Log(r) / speed;
        }

        public void Die()
        {
            SnakeDie?.Invoke(this, _current);
            _current.CellType = CellType.Death;
        }
    }
}
