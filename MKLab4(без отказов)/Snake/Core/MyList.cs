using System;
using System.Collections;
using System.Collections.Generic;


namespace Snake.Core
{
    internal class MyList<T> : List<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _id;
        public string ID
        {
            get => _id;
            set => _id = value;
        }

        private int _current = 0;
        public int Current
        {
            get => _current;
            set => _current = value;
        }

        public MyList() { }
        public MyList(int capacity) => Capacity = capacity;
        //public MyList(List<T> list) => this.AddRange = list;

        //public static explicit operator List<T>(MyList<T> myList)
        //{
        //    return myList;
        //}

        public T StepForward()
        {
            if (this.Count == 0) throw new Exception("Empty");

            if (++Current < this.Count) return this[Current];
            else return this[0];
        }

        public T StepBack()
        {
            if (this.Count == 0) throw new Exception("Empty");

            if (--Current > -1) return this[Current];
            else return this[Count - 1];
        }

        public T GetCarefull(int i)
        {
            if (this.Count == 0) throw new Exception("Empty");

            if (i < 0) i = Count - 1;
            if (i > Count - 1) i = 0;

            return this[i];
        }
    }
}
