using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Evo.Core.Basic;
using Evo.Core.Units;
using Evo.Core.Universe;

namespace Evo.GUI.Winforms
{
    public partial class FrmMain : Form
    {
        private const int MapMultiplier = 5;
        private World _world;
        private Color bgColor = Color.Black;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DoStep();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _world = new World(new Random(), new Coord(Map.Size.Width / MapMultiplier, Map.Size.Height / MapMultiplier));
                _world.MutationProbability.Value = 100;
                _world.MutationMaxDelta.Value = 40;
                _world.EnergyDrainModificator.Value = 1;
                _world.MaxFoodItemsPerTick.Value = 40;
                _world.MaxEneryPerFoodItem.Value = 80;
                _world.MaxFoodItems.Value = 300;

                var initPopulation = new List<Individual>(100);
                for (int i = 0; i < 50; i++)
                {
                    var individual = _world.Mutator.GenerateRandom();
                    initPopulation.Add(individual);
                }
                _world.SpreadIndividuals(initPopulation, new Coord(0, 0),
                    new Coord(Map.Size.Width / MapMultiplier, Map.Size.Height / MapMultiplier));
                _world.SpreadFood();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btn1Step_Click(object sender, EventArgs e)
        {
            DoStep();
        }

        private void btnToggleTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void Map_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(bgColor);
            var foodBrush = new HatchBrush(HatchStyle.Percent50, Color.ForestGreen);
            foreach (var foodItem in _world.Food)
            {
                e.Graphics.FillRectangle(foodBrush, foodItem.Point.X * MapMultiplier, foodItem.Point.Y * MapMultiplier,
                    MapMultiplier, MapMultiplier);
            }
            foreach (var individual in _world.Population)
            {
                e.Graphics.FillRectangle(new SolidBrush(GetIndividualColor(individual)),
                    individual.Point.X * MapMultiplier, individual.Point.Y * MapMultiplier, MapMultiplier, MapMultiplier);
            }
        }

        private void DoStep()
        {
            _world.Live1Tick();
            Map.Invalidate();
            this.Text = "Evo. Population = " + _world.Population.Count;

            var sb = new StringBuilder();
            foreach (var stat in _world.IncStats.GetStats())
            {
                sb.AppendLine($"{stat.Key}\t {stat.Value}");
            }
            this.txtStats.Text = sb.ToString();
        }

        private void Map_MouseClick(object sender, MouseEventArgs e)
        {
            var worldCoord = new Coord(e.X / MapMultiplier, e.Y / MapMultiplier);
            var unit = _world.Navigator.FindUnit(worldCoord);
            if (unit != null)
            {
                var individual = unit as Individual;
                if (individual != null)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Individual");
                    sb.AppendLine($"Id: {individual.Id}");
                    sb.AppendLine($"Status:");
                    sb.AppendLine($"Age: {individual.Age}");
                    sb.AppendLine($"Energy: {individual.Energy}");
                    sb.AppendLine($"Desire: {individual.Desire}");
                    sb.AppendLine($"Target: {individual.Target}");
                    sb.AppendLine("Genome:");
                    sb.AppendLine($"Lifetime: {individual.LifeTime}");
                    sb.AppendLine($"Aggression: {individual.Aggression}");
                    sb.AppendLine($"Strength: {individual.Strength}");
                    sb.AppendLine($"Fertility: {individual.Fertility}");
                    sb.AppendLine($"Purpose: {individual.Purpose}");
                    sb.AppendLine($"Sight Range: {individual.SightRange}");
                    sb.AppendLine($"Min Energy Acceptable: {individual.MinEnergyAcceptable}");
                    txtUnit.Text = sb.ToString();
                }
                var food = unit as FoodItem;
                if (food != null)
                {
                    txtUnit.Text = $"Food\r\nID: {food.Id}\r\nEnergy: {food.Energy}";
                }
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                Debug.WriteLine("Unit not found at " + worldCoord);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bgColorDialog.ShowDialog() == DialogResult.OK)
            {
                bgColor = bgColorDialog.Color;
                Map.Invalidate();
            }
        }

        private Color GetIndividualColor(Individual individual)
        {
            if (rbAge.Checked)
            {
                return GetColorFromValue(individual.Age);
            }
            if (rbEnergy.Checked)
            {
                return GetColorFromValue(individual.Energy, false);
            }
            if (rbDesire.Checked)
            {
                return GetColorFromValue(individual.Desire);
            }
            if (rbAggression.Checked)
            {
                return GetColorFromValue(individual.Aggression);
            }
            if (rbStrength.Checked)
            {
                return GetColorFromValue(individual.Strength);
            }
            if (rbLifeTime.Checked)
            {
                return GetColorFromValue(individual.LifeTime, false);
            }
            if (rbFertility.Checked)
            {
                return GetColorFromValue(individual.Fertility, false);
            }
            if (rbPurpose.Checked)
            {
                return GetColorFromValue(individual.Purpose);
            }
            if (rbSightRange.Checked)
            {
                return GetColorFromValue(individual.SightRange, false);
            }
            if (rbMinEneryAcceptable.Checked)
            {
                return GetColorFromValue(individual.MinEnergyAcceptable, false);
            }

            return Color.FromArgb(individual.Color * 0x100);
        }

        private Color GetColorFromValue(LimitedInt gene, bool biggerRed = true)
        {
            LimitedInt percent = new LimitedInt(0, 100)
            {
                Value = (int)Math.Round(100 * gene.NormalizedValue, MidpointRounding.AwayFromZero)
            };

            if (!biggerRed)
            {
                percent.Value = 100 - percent;
            }
            return Color.FromArgb(255 * percent / 100, (255 * (100 - percent)) / 100, 0);
        }

        private void rbViewColor_CheckedChanged(object sender, EventArgs e)
        {
            Map.Invalidate();
        }

        private void numSpeed_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1000 - (int)numSpeed.Value * 100;
        }
    }
}
