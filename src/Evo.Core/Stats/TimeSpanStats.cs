using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.Core.Stats
{
    public class TimeSpanStats<T>: IEnumerable<TimeSpanStatItem<T>>
    {
        private readonly List<TimeSpanStatItem<T>> _data;

        public TimeSpanStats(int maxSize)
        {
            MaxSize = maxSize;
            _data = new List<TimeSpanStatItem<T>>(maxSize);
        }

        public int MaxSize { get; set; }

        public TimeSpanStatItem<T> this[int i] => _data[i];

        public void Add(ulong tick, T value)
        {
            Add(new TimeSpanStatItem<T>(tick, value));
        }

        public void Add(TimeSpanStatItem<T> item)
        {
            if (_data.Count >= MaxSize)
            {
                _data.RemoveAt(0);
            }
            _data.Add(item);
        }

        public IEnumerator<TimeSpanStatItem<T>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }

    public struct TimeSpanStatItem<T>
    {
        public ulong Tick;
        public T Value;

        public TimeSpanStatItem(ulong tick, T value)
        {
            Tick = tick;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Tick} - {Value}";
        }
    }
}
