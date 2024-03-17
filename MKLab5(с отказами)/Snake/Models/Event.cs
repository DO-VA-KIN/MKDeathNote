using Snake.Core;
using System;

namespace Snake.Models
{
    internal class Event
    {
        public Action Execute { get; set; }

        protected int _num;
        public int Num
        {
            get => _num;
            set => _num = value;
        }

        protected string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        protected CellVM _currentCell;
        public CellVM CurrentCell
        {
            get => _currentCell;
            set => _currentCell = value;
        }

        protected CellVM _newCell;
        public CellVM NewCell
        {
            get => _newCell;
            set => _newCell = value;
        }

        public Directions Direction = Directions.None;

        private int _speed;
        public int Speed
        {
            get => _speed; 
            set => _speed = value;
        }

        protected bool _available = false;
        public bool Available
        {
            get => _available;
            set => _available = value;
        }
    }


    enum Directions
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}
