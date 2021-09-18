using System;
using System.Collections.Generic;

namespace LearningCSharp
{
    public class CircularList<T> : List<T>
    {
        public new T this[int i] => base[i % base.Count];
        public new void Insert(int i, T v) => base.Insert(i % base.Count, v);
        public new void InsertRange(int i, IEnumerable<T> r) => base.InsertRange(i % base.Count, r);
    }
}