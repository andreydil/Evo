using System.Collections.Generic;

namespace Evo.Core.Units
{
    public class UnitIdComparer : IComparer<Unit>
    {
        public int Compare(Unit x, Unit y)
        {
            if (x.Id == y.Id)
            {
                return 0;
            }
            if (x.Id < y.Id)
            {
                return -1;
            }
            return 1;
        }
    }
}
