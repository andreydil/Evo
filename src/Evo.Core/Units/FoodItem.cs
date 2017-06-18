namespace Evo.Core.Units
{
    public class FoodItem : Unit
    {
        public FoodItem(ulong id) : base(id)
        {
        }

        public int Energy { get; set; }

        public override UnitType Type => UnitType.Food;
    }
}
