using System;

namespace Evo.Core.Units
{
    [Serializable]
    public class FoodItem : Unit
    {
        public FoodItem(ulong id) : base(id, UnitType.Food)
        {
        }

        public int Energy { get; set; }
    }
}
