using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Evo.Core.Basic;
using Evo.Core.Units;
using Evo.Core.Universe;

namespace Evo.GUI.Winforms
{
    public partial class FrmMain : Form
    {
        private const int MapMultiplier = 5;
        private const int StatsRefreshTime = 100;
        private DateTime lastStatsRefresh = DateTime.Now;
        private World _world;
        private Color bgColor = Color.Black;

        private Task bgTask = null;
        private CancellationTokenSource bgTaskCancelationSource;

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
                _world.MutationProbability.Value = 120;
                _world.MutationMaxDelta.Value = 40;
                _world.EnergyDrainModificator.Value = 1;
                _world.MaxFoodItemsPerTick.Value = 60;
                _world.MaxEneryPerFoodItem.Value = 100;
                _world.MaxFoodItems.Value = 400;

                var initPopulation = new List<Individual>(100);
                for (int i = 0; i < 50; i++)
                {
                    var individual = _world.Mutator.GenerateAverage();
                    initPopulation.Add(individual);
                }
                _world.SpreadIndividuals(initPopulation, new Coord(0, 0), new Coord(Map.Size.Width / MapMultiplier, Map.Size.Height / MapMultiplier));
                _world.SpreadFood();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            chtPopulation.Series.Clear();
            var populationSeries = new Series
            {
                Name = "Population",
                Color = Color.DodgerBlue,
            };
            chtPopulation.Series.Add(populationSeries);

            var foodSeries = new Series
            {
                Name = "Food",
                Color = Color.ForestGreen,
            };
            chtPopulation.Series.Add(foodSeries);
        }

        private void btn1Step_Click(object sender, EventArgs e)
        {
            DoStep();
        }

        private void btnToggleTimer_Click(object sender, EventArgs e)
        {
            if (chkVisualize.Checked)
            {
                timer1.Enabled = !timer1.Enabled;
                btn1Step.Enabled = chkVisualize.Enabled = !timer1.Enabled;
            }
            else
            {
                if (bgTask == null)
                {
                    chkVisualize.Enabled = btn1Step.Enabled = false;
                    bgTaskCancelationSource = new CancellationTokenSource();
                    var cancellationToken = bgTaskCancelationSource.Token;
                    bgTask = Task.Factory.StartNew(() => 
                    {
                        while (true)
                        {
                            _world.Live1Tick();
                            if (_world.Population.Count == 0)
                            {
                                break;
                            }
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                        }
                    }, cancellationToken);
                }
                else
                {
                    bgTaskCancelationSource.Cancel();
                    if (!bgTask.IsCanceled)
                    {
                        bgTask.Wait();
                    }
                    bgTask = null;
                    chkVisualize.Enabled = btn1Step.Enabled = true;
                    DoStep();
                }
            }
        }

        private void Map_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(bgColor);

            if (bgTask != null)
            {
                return;
            }

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

            if ((DateTime.Now - lastStatsRefresh).TotalMilliseconds >= StatsRefreshTime)
            {
                showTextStats();
                showChart();
            }
        }

        private void showTextStats()
        {
            var sb = new StringBuilder();
            sb.AppendLine(formatStatLine("Tick", _world.Tick));
            foreach (var stat in _world.MainStats.GetStats().OrderBy(s => s.Key))
            {
                sb.AppendLine(formatStatLine(stat.Key, stat.Value));
            }
            txtStats.Text = sb.ToString();

            txtAverageUnit.Text = "Average Individual:\r\n" + GetIndividualInfo(_world.AverageIndividual, true);

            lastStatsRefresh = DateTime.Now;
        }

        private string formatStatLine(string label, object value)
        {
            const int lineLength = 25;
            string str = value.ToString();
            return $"{label}{new string(' ', lineLength - label.Length - str.Length)}{str}";
        }

        private void showChart()
        {
            chtPopulation.Series.Clear();
            chtPopulation.AlignDataPointsByAxisLabel();
            
            var populationSeries = new Series
            {
                Name = "Population",
                Color = Color.DodgerBlue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line,
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
            };
            foreach (var stat in _world.PopulationSize)
            {
                populationSeries.Points.AddXY(stat.Tick, stat.Value);
            }
            chtPopulation.Series.Add(populationSeries);

            var foodSeries = new Series
            {
                Name = "Food",
                Color = Color.ForestGreen,
                IsVisibleInLegend = true,
                IsXValueIndexed = false,
                ChartType = SeriesChartType.Line,
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Solid,
            };
            foreach (var stat in _world.FoodAmount)
            {
                foodSeries.Points.AddXY(stat.Tick, stat.Value);
            }
            chtPopulation.Series.Add(foodSeries);

            var legend = chtPopulation.Legends[0];
            legend.Docking = Docking.Bottom;
            legend.IsDockedInsideChartArea = false;
            legend.TableStyle = LegendTableStyle.Wide;
            legend.Alignment = StringAlignment.Near;

            chtPopulation.Refresh();
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
                    txtUnit.Text = "Selected unit:\r\n" + GetIndividualInfo(individual);
                }
                var food = unit as FoodItem;
                if (food != null)
                {
                    txtUnit.Text = $"{formatStatLine("Food", food.Id)}\r\n{formatStatLine("Energy", food.Energy)}";
                }
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                Debug.WriteLine("Unit not found at " + worldCoord);
            }
        }

        private string GetIndividualInfo(Individual individual, bool telemetryOnly = false)
        {
            var sb = new StringBuilder();
            if (!telemetryOnly)
            {
                sb.AppendLine(formatStatLine("Id", individual.Id.ToString()));
            }
            sb.AppendLine("\r\nStatus:");
            sb.AppendLine(formatStatLine("Age", individual.Age.Value));
            sb.AppendLine(formatStatLine("Energy", individual.Energy.Value));
            sb.AppendLine(formatStatLine("Desire", individual.Desire.Value));
            if (!telemetryOnly)
            {
                sb.AppendLine(formatStatLine("Target", individual.Target));
            }
            sb.AppendLine("\r\nGenome:");
            sb.AppendLine(formatStatLine("Color", "#" + individual.Color.Value.ToString("X6")));
            foreach (var geneItem in individual.Genome.Where(gi => gi.Key != GeneNames.Color))
            {
                sb.AppendLine(formatStatLine(geneItem.Key, individual.LifeTime.Value));
            }
            return sb.ToString();
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

            return Color.FromArgb(individual.Color.Red, individual.Color.Green, individual.Color.Blue);
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

        private void chkVisualize_CheckedChanged(object sender, EventArgs e)
        {
            numSpeed.Enabled = chkVisualize.Checked;
        }
    }
}
