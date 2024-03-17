using Snake.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Snake.Core;

namespace Snake.Models
{
    internal class SnakeFactory
    {
        Random Rand = new Random();

        private bool _timeSkip = true;
        public bool TimeSkip
        {
            get => _timeSkip;
            set => _timeSkip = value; 
        }
        private double[] _time = new double[1] { 0.0 };
        public double[] Time
        { 
            get=> _time;
            set => _time = value;
        }

        private ushort _delay = 1;
        public ushort Delay
        {
            get => _delay;
            set
            {
                if (value > 0)
                    _delay = value;
            }
        }

        private List<Snake> _snakes;
        public List<Snake> Snakes
        {
            get => _snakes;
            private set => _snakes = value;
        }

        private List<Point<ushort, CellVM>> _newSnakes = new List<Point<ushort, CellVM>>();
        private List<Snake> _diesSnakes = new List<Snake>();
        private readonly MyList<MyList<CellVM>> _allCells;


        public SnakeFactory(HashSet<Point<ushort, ushort>> snakesStarts, MyList<MyList<CellVM>> allCells)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            _allCells = allCells;
            _snakes = new List<Snake>(snakesStarts.Count);
            foreach (Point<ushort, ushort> start in snakesStarts)
            {
                Snake snake = new Snake(allCells, allCells[start.X][start.Y]){ Rand = new Random(rand.Next()) };
                snake.Time = Time;
                snake.NewSnake += SnakeNewSnake;
                snake.SnakeDie += SnakeDie;
                _snakes.Add(snake);
            }
        }

        private void SnakeDie(Snake sender, CellVM Cell)
        {
            sender.NewSnake -= SnakeNewSnake;
            sender.SnakeDie -= SnakeDie;
            _diesSnakes.Add(sender);
        }

        private void SnakeNewSnake(Snake sender, CellVM Cell)
        {
            _newSnakes.Add(new Point<ushort, CellVM> {X = Delay, Y = Cell });
        }


        public void Dies()
        {
            foreach (var snake in _newSnakes)
                snake.Y.CellType = CellType.Death;

            foreach (Snake snake in Snakes)
            {
                snake.NewSnake -= SnakeNewSnake;
                snake.SnakeDie -= SnakeDie;
                snake.Die();
            }
        }

        public void SnakesGo()
        {

            foreach (Snake snake in _diesSnakes)
                Snakes.Remove(snake);
            _diesSnakes.Clear();

            Snakes[Rand.Next(Snakes.Count)].Move();

            foreach (Point<ushort, CellVM> item in _newSnakes)
            {
                if (--item.X == 0)
                {
                    Snake snake = new Snake(_allCells, item.Y);
                    snake.Time = Time;
                    snake.NewSnake += SnakeNewSnake;
                    snake.SnakeDie += SnakeDie;
                    Snakes.Add(snake);
                }
            }
            _newSnakes = _newSnakes.Where(point => point.X > 0).ToList();
        }

    }
}
