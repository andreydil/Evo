using System;

namespace Evo.Core.Basic 
{
    [Serializable]
    public class Gene : LimitedInt
    {
        public Gene(int min, int max) : base(min, max)
        {
        }
    }
}
