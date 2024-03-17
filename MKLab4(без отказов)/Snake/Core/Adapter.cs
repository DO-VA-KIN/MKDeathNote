using Snake.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core
{
    internal static class Adapter
    {
        public static MyList<T> ToMyList<T>(this List<T> list)
        {
            MyList<T> mylist = new MyList<T>(list.Count);
            mylist.AddRange(list);
            return mylist;
        }


    }
}
