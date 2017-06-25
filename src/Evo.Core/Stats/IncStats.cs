using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evo.Core.Stats
{
    [Serializable]
    public class IncStats
    {
        private readonly Dictionary<string, int> _statItems = new Dictionary<string, int>();

        public void AddStat(string key, int value = 1)
        {
            if (_statItems.ContainsKey(key))
            {
                _statItems[key] = _statItems[key] + value;
            }
            else
            {
                _statItems.Add(key, value);
            }
        }

        public int GetStat(string key)
        {
            return _statItems.ContainsKey(key) ? _statItems[key] : 0;
        }

        public Dictionary<string, int> GetStats()
        {
            return _statItems;  //TODO: return a copy
        }
    }
}