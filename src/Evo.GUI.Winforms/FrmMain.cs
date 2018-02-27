using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Evo.Core.Basic;
using Evo.Core.Saving;
using Evo.Core.Units;
using Evo.Core.Universe;

namespace Evo.GUI.Winforms
{
    public partial class FrmMain : Form
    {
        private const int MapMultiplier = 5;
        private const int StatsRefreshTime = 100;
        private const string StartTimerCaption = "Start Timer";
        private const string StopTimerCaption = "Stop Timer";
        private DateTime _lastStatsRefresh = DateTime.Now;
        private World _world;
        private Color _bgColor = Color.Black;
        private Individual _curAverageIndividual;
        private int _maxDifference;
        private int _benchmarkTicks = 0;
        private readonly object _lockObj = new object();

        private Task bgTask = null;
        private CancellationTokenSource bgTaskCancelationSource;

        private int _curMouseX = 0;
        private int _curMouseY = 0;
        private Pen _wallPen = new Pen(Color.Coral, MapMultiplier);

        private Coord? _killAllAreaStartingPoint  = null;
        private Brush _killAllBrush = new SolidBrush(Color.Red);

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
            Init();
        }

        private void Init()
        {
            var worldConfigurator = new DefaultWorldConfigurator(Map.Size.Width / MapMultiplier, Map.Size.Height / MapMultiplier);
            InitWithNewWorld(worldConfigurator.CreateWorld());

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

        private void InitWithNewWorld(World world)
        {
            try
            {
                _world = world;

                _curAverageIndividual = _world.AverageIndividual;

                var minIndividual = _world.Mutator.GenerateIndividual(g => g.Min, 0, null);
                var maxIndividual = _world.Mutator.GenerateIndividual(g => g.Max, 0, null);
                _maxDifference = _world.StatCounter.GetDifference(minIndividual, maxIndividual);
                buildTuners();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn1Step_Click(object sender, EventArgs e)
        {
            DoStep();
        }

        private void btnToggleTimer_Click(object sender, EventArgs e)
        {
            if (chkVisualize.Checked)
            {
                if (timer1.Enabled)
                {
                    stopWithVisualization();
                }
                else
                {
                    startWithVisualization();
                }
                
            }
            else
            {
                if (bgTask == null)
                {
                    startBackGround();
                }
                else
                {
                    stopBackGround();
                }
            }
        }

        private void startWithVisualization()
        {
            timer1.Enabled = true;
            btn1Step.Enabled = false;
            worldToolStripMenuItem.Enabled = false;
            tabEditWorld.Enabled = false;
            btnToggleTimer.Text = StopTimerCaption;
        }

        private void stopWithVisualization()
        {
            timer1.Enabled = false;
            btn1Step.Enabled = true;
            worldToolStripMenuItem.Enabled = true;
            tabEditWorld.Enabled = true;
            btnToggleTimer.Text = StartTimerCaption;
        }

        private void startBackGround()
        {
            btn1Step.Enabled = false;
            bgTaskCancelationSource = new CancellationTokenSource();
            var cancellationToken = bgTaskCancelationSource.Token;
            bgTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    lock (_lockObj)
                    {
                        _world.Live1Tick();
                        if (!_world.Population.Any())
                        {
                            break;
                        }
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }, cancellationToken);
            btn1Step.Enabled = false;
            worldToolStripMenuItem.Enabled = false;
            tabEditWorld.Enabled = false;
            btnToggleTimer.Text = StopTimerCaption;
        }

        private void stopBackGround()
        {
            bgTaskCancelationSource.Cancel();
            if (!bgTask.IsCanceled)
            {
                bgTask.Wait();
            }
            bgTask = null;
            chkVisualize.Enabled = btn1Step.Enabled = true;
            DoStep();
            btn1Step.Enabled = true;
            worldToolStripMenuItem.Enabled = true;
            tabEditWorld.Enabled = true;
            btnToggleTimer.Text = StartTimerCaption;
        }

        public void EnableBenchmarking(int ticks)
        {
            _benchmarkTicks = ticks;
        }
        
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Benchmark();
        }

        private void Benchmark()
        {
            if (_benchmarkTicks <= 0)
            {
                return;
            }

            Text = $"Benchmarking {_benchmarkTicks} ticks...";
            Enabled = false;

            int ticks;
            lock (_lockObj)
            {
                 ticks = _benchmarkTicks;
                _benchmarkTicks = 0;
            }
            for (int i = 0; i < ticks; ++i)
            {
                _world.Live1Tick();
            }
            Close();
        }

        private void Map_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(_bgColor);

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
            foreach (var wall in _world.Navigator.Walls)
            {
                int wallCoord = wall.Coord * MapMultiplier + MapMultiplier / 2;
                switch (wall.Type)
                {
                    case WallType.Vertical:
                        e.Graphics.DrawLine(_wallPen, wallCoord, 0, wallCoord, Map.Height - 1);
                        break;
                    case WallType.Horizontal:
                        e.Graphics.DrawLine(_wallPen, 0, wallCoord, Map.Width - 1, wallCoord);
                        break;
                }
            }

            if (chkVerticalWall.Checked)
            {
                int wallX = _curMouseX / MapMultiplier * MapMultiplier + MapMultiplier / 2;
                e.Graphics.DrawLine(_wallPen, wallX, 0, wallX, Map.Height - 1);
            }
            else if (chkHorizontalWall.Checked)
            {
                int wallY = _curMouseY / MapMultiplier * MapMultiplier + MapMultiplier / 2;
                e.Graphics.DrawLine(_wallPen, 0, wallY, Map.Width - 1, wallY);
            }
            if (chkKillInArea.Checked && _killAllAreaStartingPoint.HasValue)
            {
                var minX = Math.Min(_killAllAreaStartingPoint.Value.X * MapMultiplier, _curMouseX / MapMultiplier * MapMultiplier);
                var minY = Math.Min(_killAllAreaStartingPoint.Value.Y * MapMultiplier, _curMouseY / MapMultiplier * MapMultiplier);
                var maxX = Math.Max(_killAllAreaStartingPoint.Value.X * MapMultiplier, _curMouseX / MapMultiplier * MapMultiplier);
                var maxY = Math.Max(_killAllAreaStartingPoint.Value.Y * MapMultiplier, _curMouseY / MapMultiplier * MapMultiplier);
                e.Graphics.FillRectangle(_killAllBrush, minX, minY, maxX - minX, maxY - minY);
            }
        }

        private void DoStep()
        {
            _world.Live1Tick();
            Map.Invalidate();
            this.Text = "Evo. Population = " + _world.Population.Count();

            if ((DateTime.Now - _lastStatsRefresh).TotalMilliseconds >= StatsRefreshTime)
            {
                _curAverageIndividual = _world.AverageIndividual;
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

            txtAverageUnit.Text = "Average Individual:\r\n" + GetIndividualInfo(_curAverageIndividual, true);

            _lastStatsRefresh = DateTime.Now;
        }

        private string formatStatLine(string label, object value)
        {
            const int lineLength = 26;
            string str = value.ToString();
            var numOfSpaces = lineLength - label.Length - str.Length;
            if (numOfSpaces <= 0)
            {
                numOfSpaces = 1;
            }
            return $"{label}{new string(' ', numOfSpaces)}{str}";
        }

        private void showChart()
        {
            chtPopulation.Series.Clear();
            
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
                    txtUnit.Text = "Selected unit:\r\n" + GetIndividualInfo(individual)
                                   + formatStatLine("Difference from average", _world.StatCounter.GetDifference(individual, _curAverageIndividual));

                }
                var food = unit as FoodItem;
                if (food != null)
                {
                    txtUnit.Text = $"{formatStatLine("Food", food.Id)}\r\n{formatStatLine("Energy", food.Energy)}";
                }
                tabView.SelectedIndex = 2;
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
                sb.AppendLine(formatStatLine(geneItem.Key, geneItem.Value.Value));
            }
            sb.AppendLine(formatStatLine("Min generation", individual.MinGeneration));
            sb.AppendLine(formatStatLine("Max generation", individual.MaxGeneration));
            return sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bgColorDialog.ShowDialog() == DialogResult.OK)
            {
                _bgColor = bgColorDialog.Color;
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
            if (rbDiffFromAverage.Checked)
            {
                return GetColorFromValue(new Gene(0, _maxDifference) { Value = 5 * _world.StatCounter.GetDifference(individual, _curAverageIndividual) });
            }

            return Color.FromArgb(individual.Color.Red, individual.Color.Green, individual.Color.Blue);
        }

        private Color GetColorFromValue(LimitedInt gene, bool biggerRed = true)
        {
            var percent = new LimitedInt(0, 100)
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
            timer1.Interval = 500 - (int)numSpeed.Value * 50;
        }

        private void chkVisualize_CheckedChanged(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                stopWithVisualization();
                startBackGround();
            }
            else if (bgTask != null)
            {
                stopBackGround();
                startWithVisualization();
            }
            numSpeed.Enabled = chkVisualize.Checked;
        }

        private string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        private void buildTuners()
        {
            const int leftMargin = 5;
            const int slidersX = 160;
            const int yDelta = 40;
            const int valueLabelX = 140;
            const int labelYShift = 3;
            int curY = 10;
            for (int i = 0, n = tabTune.Controls.Count; i < n; ++i)
            {
                tabTune.Controls.RemoveAt(0);
            }
            foreach (var tunerItem in _world.Tuners)
            {
                var tuner = tunerItem.Value;
                var nameLabel = new Label
                {
                    Text = SplitCamelCase(tunerItem.Key),
                    Location = new Point(leftMargin,curY + labelYShift),
                    AutoSize = true,
                };
                tabTune.Controls.Add(nameLabel);

                var valueLabel = new Label
                {
                    Text = tuner.Value.ToString(),
                    Location = new Point(valueLabelX, curY + labelYShift),
                    AutoSize = true,
                };
                tabTune.Controls.Add(valueLabel);

                var slider = new TrackBar
                {
                    Location = new Point(slidersX, curY),
                    Minimum = tuner.Min,
                    Maximum = tuner.Max,
                    Value = tuner.Value,
                    TickStyle = TickStyle.None,
                    Width = tabTune.Width - slidersX - leftMargin,
                };
                slider.Scroll += (sender, args) =>
                {
                    valueLabel.Text = slider.Value.ToString();
                    tuner.Value = slider.Value;
                };
                tabTune.Controls.Add(slider);

                curY += yDelta;
            }
        }
        
        private void Map_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            _curMouseX = e.X;
            _curMouseY = e.Y;
            Map.Invalidate();
        }
        
        private void chkVerticalWall_Click(object sender, EventArgs e)
        {
            chkHorizontalWall.Checked = chkRemoveWall.Checked = chkKillInArea.Checked = false;
            Map.Invalidate();
        }

        private void chkHorizontalWall_Click(object sender, EventArgs e)
        {
            chkVerticalWall.Checked = chkRemoveWall.Checked = chkKillInArea.Checked = false;
            Map.Invalidate();
        }

        private void chkRemoveWall_Click(object sender, EventArgs e)
        {
            chkHorizontalWall.Checked = chkVerticalWall.Checked = chkKillInArea.Checked = false;
            Map.Invalidate();
        }

        private void chkKillInArea_Click(object sender, EventArgs e)
        {
            chkHorizontalWall.Checked = chkRemoveWall.Checked = chkVerticalWall.Checked = false;
            Map.Invalidate();
        }

        private void chkRemoveWall_CheckedChanged(object sender, EventArgs e)
        {
            Map.Cursor = chkRemoveWall.Checked ? Cursors.Cross : DefaultCursor;
        }

        private void Map_Click(object sender, EventArgs e)
        {
            if (chkVerticalWall.Checked)
            {
                _world.Navigator.AddWall(new Wall(WallType.Vertical, _curMouseX / MapMultiplier));
                chkVerticalWall.Checked = false;
                Map.Invalidate();
            }
            else if (chkHorizontalWall.Checked)
            {
                _world.Navigator.AddWall(new Wall(WallType.Horizontal, _curMouseY / MapMultiplier));
                chkHorizontalWall.Checked = false;
                Map.Invalidate();
            }
            else if (chkRemoveWall.Checked)
            {
                _world.Navigator.RemoveWall(_curMouseX / MapMultiplier, _curMouseY / MapMultiplier);
                chkRemoveWall.Checked = false;
                Map.Invalidate();
            }
            else if (chkKillInArea.Checked)
            {
                if (_killAllAreaStartingPoint.HasValue)
                {
                    _world.Navigator.KillAllInArea(_killAllAreaStartingPoint.Value, new Coord(_curMouseX / MapMultiplier, _curMouseY / MapMultiplier));
                    _killAllAreaStartingPoint = null;
                    chkKillInArea.Checked = false;
                }
                else
                {
                    _killAllAreaStartingPoint = new Coord(_curMouseX / MapMultiplier, _curMouseY / MapMultiplier);
                }
                Map.Invalidate();
            }
        }

        private Coord ConvertCoordFormToWorld(Coord point)
        {
            return new Coord(point.X / MapMultiplier, point.Y / MapMultiplier);
        }

        private Coord ConvertCoordWorldToForm(Coord point)
        {
            return new Coord(point.X * MapMultiplier, point.Y * MapMultiplier);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!worldToolStripMenuItem.Enabled) return;

            if (saveWorldDialog.ShowDialog() == DialogResult.OK)
            {
                WorldSaver.Save(_world, saveWorldDialog.FileName);
            }
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (!worldToolStripMenuItem.Enabled) return;

            if (openWorldDialog.ShowDialog() == DialogResult.OK)
            {
                var world = WorldSaver.Load(openWorldDialog.FileName);
                InitWithNewWorld(world);
                Map.Invalidate();
            }
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            var frmAbout = new FrmAbout { StartPosition = FormStartPosition.CenterParent };
            frmAbout.ShowDialog();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!worldToolStripMenuItem.Enabled) return;

            var frmNewWorld = new FrmNewWorld(_world.Size.X, _world.Size.Y) { StartPosition = FormStartPosition.CenterParent };
            if (frmNewWorld.ShowDialog() == DialogResult.OK)
            {
                InitWithNewWorld(frmNewWorld.GetWorld());
                Map.Width = _world.Size.X * MapMultiplier;
                Map.Height = _world.Size.Y * MapMultiplier;
                DoStep();
            }
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            if (!worldToolStripMenuItem.Enabled) return;

            this.Close();
        }
    }
}
