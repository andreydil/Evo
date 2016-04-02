﻿// <auto-generated/>
namespace Evo.GUI.Winforms
{
    public partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            this.btnToggleTimer = new System.Windows.Forms.Button();
            this.btn1Step = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chtPopulation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtStats = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSightRange = new System.Windows.Forms.RadioButton();
            this.rbMinEneryAcceptable = new System.Windows.Forms.RadioButton();
            this.rbPurpose = new System.Windows.Forms.RadioButton();
            this.rbFertility = new System.Windows.Forms.RadioButton();
            this.rbLifeTime = new System.Windows.Forms.RadioButton();
            this.rbAggression = new System.Windows.Forms.RadioButton();
            this.rbStrength = new System.Windows.Forms.RadioButton();
            this.rbDesire = new System.Windows.Forms.RadioButton();
            this.rbEnergy = new System.Windows.Forms.RadioButton();
            this.rbColor = new System.Windows.Forms.RadioButton();
            this.rbAge = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.Map = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bgColorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtPopulation)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Map)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Map);
            this.splitContainer1.Size = new System.Drawing.Size(1509, 802);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numSpeed);
            this.groupBox2.Controls.Add(this.btnToggleTimer);
            this.groupBox2.Controls.Add(this.btn1Step);
            this.groupBox2.Location = new System.Drawing.Point(7, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Control";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Speed";
            // 
            // numSpeed
            // 
            this.numSpeed.Location = new System.Drawing.Point(131, 51);
            this.numSpeed.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(36, 20);
            this.numSpeed.TabIndex = 2;
            this.numSpeed.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numSpeed.ValueChanged += new System.EventHandler(this.numSpeed_ValueChanged);
            // 
            // btnToggleTimer
            // 
            this.btnToggleTimer.Location = new System.Drawing.Point(6, 48);
            this.btnToggleTimer.Name = "btnToggleTimer";
            this.btnToggleTimer.Size = new System.Drawing.Size(75, 23);
            this.btnToggleTimer.TabIndex = 1;
            this.btnToggleTimer.Text = "Timer";
            this.btnToggleTimer.UseVisualStyleBackColor = true;
            this.btnToggleTimer.Click += new System.EventHandler(this.btnToggleTimer_Click);
            // 
            // btn1Step
            // 
            this.btn1Step.Location = new System.Drawing.Point(6, 19);
            this.btn1Step.Name = "btn1Step";
            this.btn1Step.Size = new System.Drawing.Size(75, 23);
            this.btn1Step.TabIndex = 0;
            this.btn1Step.Text = "Step";
            this.btn1Step.UseVisualStyleBackColor = true;
            this.btn1Step.Click += new System.EventHandler(this.btn1Step_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 89);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(358, 713);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chtPopulation);
            this.tabPage2.Controls.Add(this.txtStats);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(350, 687);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Stats";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chtPopulation
            // 
            chartArea3.Name = "ChartArea1";
            this.chtPopulation.ChartAreas.Add(chartArea3);
            this.chtPopulation.Dock = System.Windows.Forms.DockStyle.Top;
            legend3.Name = "Legend1";
            this.chtPopulation.Legends.Add(legend3);
            this.chtPopulation.Location = new System.Drawing.Point(3, 3);
            this.chtPopulation.Name = "chtPopulation";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chtPopulation.Series.Add(series3);
            this.chtPopulation.Size = new System.Drawing.Size(344, 298);
            this.chtPopulation.TabIndex = 1;
            this.chtPopulation.TabStop = false;
            this.chtPopulation.Text = "Population";
            // 
            // txtStats
            // 
            this.txtStats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtStats.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtStats.Location = new System.Drawing.Point(3, 307);
            this.txtStats.Multiline = true;
            this.txtStats.Name = "txtStats";
            this.txtStats.ReadOnly = true;
            this.txtStats.Size = new System.Drawing.Size(344, 377);
            this.txtStats.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtUnit);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(367, 687);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Unit";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtUnit
            // 
            this.txtUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUnit.Location = new System.Drawing.Point(3, 3);
            this.txtUnit.Multiline = true;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(361, 681);
            this.txtUnit.TabIndex = 0;
            this.txtUnit.TabStop = false;
            this.txtUnit.Text = "<Click on a unit to get its info>";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(367, 687);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "View";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSightRange);
            this.groupBox1.Controls.Add(this.rbMinEneryAcceptable);
            this.groupBox1.Controls.Add(this.rbPurpose);
            this.groupBox1.Controls.Add(this.rbFertility);
            this.groupBox1.Controls.Add(this.rbLifeTime);
            this.groupBox1.Controls.Add(this.rbAggression);
            this.groupBox1.Controls.Add(this.rbStrength);
            this.groupBox1.Controls.Add(this.rbDesire);
            this.groupBox1.Controls.Add(this.rbEnergy);
            this.groupBox1.Controls.Add(this.rbColor);
            this.groupBox1.Controls.Add(this.rbAge);
            this.groupBox1.Location = new System.Drawing.Point(8, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 277);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show";
            // 
            // rbSightRange
            // 
            this.rbSightRange.AutoSize = true;
            this.rbSightRange.Location = new System.Drawing.Point(6, 226);
            this.rbSightRange.Name = "rbSightRange";
            this.rbSightRange.Size = new System.Drawing.Size(84, 17);
            this.rbSightRange.TabIndex = 11;
            this.rbSightRange.Text = "Sight Range";
            this.rbSightRange.UseVisualStyleBackColor = true;
            this.rbSightRange.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbMinEneryAcceptable
            // 
            this.rbMinEneryAcceptable.AutoSize = true;
            this.rbMinEneryAcceptable.Location = new System.Drawing.Point(6, 249);
            this.rbMinEneryAcceptable.Name = "rbMinEneryAcceptable";
            this.rbMinEneryAcceptable.Size = new System.Drawing.Size(135, 17);
            this.rbMinEneryAcceptable.TabIndex = 10;
            this.rbMinEneryAcceptable.Text = "Min Energy Acceptable";
            this.rbMinEneryAcceptable.UseVisualStyleBackColor = true;
            this.rbMinEneryAcceptable.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbPurpose
            // 
            this.rbPurpose.AutoSize = true;
            this.rbPurpose.Location = new System.Drawing.Point(6, 203);
            this.rbPurpose.Name = "rbPurpose";
            this.rbPurpose.Size = new System.Drawing.Size(64, 17);
            this.rbPurpose.TabIndex = 9;
            this.rbPurpose.Text = "Purpose";
            this.rbPurpose.UseVisualStyleBackColor = true;
            this.rbPurpose.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbFertility
            // 
            this.rbFertility.AutoSize = true;
            this.rbFertility.Location = new System.Drawing.Point(6, 180);
            this.rbFertility.Name = "rbFertility";
            this.rbFertility.Size = new System.Drawing.Size(57, 17);
            this.rbFertility.TabIndex = 8;
            this.rbFertility.Text = "Fertility";
            this.rbFertility.UseVisualStyleBackColor = true;
            this.rbFertility.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbLifeTime
            // 
            this.rbLifeTime.AutoSize = true;
            this.rbLifeTime.Location = new System.Drawing.Point(6, 157);
            this.rbLifeTime.Name = "rbLifeTime";
            this.rbLifeTime.Size = new System.Drawing.Size(68, 17);
            this.rbLifeTime.TabIndex = 7;
            this.rbLifeTime.Text = "Life Time";
            this.rbLifeTime.UseVisualStyleBackColor = true;
            this.rbLifeTime.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbAggression
            // 
            this.rbAggression.AutoSize = true;
            this.rbAggression.Location = new System.Drawing.Point(6, 111);
            this.rbAggression.Name = "rbAggression";
            this.rbAggression.Size = new System.Drawing.Size(77, 17);
            this.rbAggression.TabIndex = 6;
            this.rbAggression.Text = "Aggression";
            this.rbAggression.UseVisualStyleBackColor = true;
            this.rbAggression.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbStrength
            // 
            this.rbStrength.AutoSize = true;
            this.rbStrength.Location = new System.Drawing.Point(6, 134);
            this.rbStrength.Name = "rbStrength";
            this.rbStrength.Size = new System.Drawing.Size(65, 17);
            this.rbStrength.TabIndex = 5;
            this.rbStrength.Text = "Strength";
            this.rbStrength.UseVisualStyleBackColor = true;
            this.rbStrength.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbDesire
            // 
            this.rbDesire.AutoSize = true;
            this.rbDesire.Location = new System.Drawing.Point(6, 88);
            this.rbDesire.Name = "rbDesire";
            this.rbDesire.Size = new System.Drawing.Size(55, 17);
            this.rbDesire.TabIndex = 4;
            this.rbDesire.Text = "Desire";
            this.rbDesire.UseVisualStyleBackColor = true;
            this.rbDesire.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbEnergy
            // 
            this.rbEnergy.AutoSize = true;
            this.rbEnergy.Location = new System.Drawing.Point(6, 65);
            this.rbEnergy.Name = "rbEnergy";
            this.rbEnergy.Size = new System.Drawing.Size(58, 17);
            this.rbEnergy.TabIndex = 2;
            this.rbEnergy.Text = "Energy";
            this.rbEnergy.UseVisualStyleBackColor = true;
            this.rbEnergy.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbColor
            // 
            this.rbColor.AutoSize = true;
            this.rbColor.Checked = true;
            this.rbColor.Location = new System.Drawing.Point(6, 19);
            this.rbColor.Name = "rbColor";
            this.rbColor.Size = new System.Drawing.Size(49, 17);
            this.rbColor.TabIndex = 1;
            this.rbColor.TabStop = true;
            this.rbColor.Text = "Color";
            this.rbColor.UseVisualStyleBackColor = true;
            this.rbColor.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // rbAge
            // 
            this.rbAge.AutoSize = true;
            this.rbAge.Location = new System.Drawing.Point(6, 42);
            this.rbAge.Name = "rbAge";
            this.rbAge.Size = new System.Drawing.Size(44, 17);
            this.rbAge.TabIndex = 3;
            this.rbAge.Text = "Age";
            this.rbAge.UseVisualStyleBackColor = true;
            this.rbAge.CheckedChanged += new System.EventHandler(this.rbViewColor_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Change background color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Map
            // 
            this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map.Location = new System.Drawing.Point(0, 0);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(1140, 802);
            this.Map.TabIndex = 0;
            this.Map.TabStop = false;
            this.Map.Paint += new System.Windows.Forms.PaintEventHandler(this.Map_Paint_1);
            this.Map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Map_MouseClick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1509, 802);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FrmMain";
            this.Text = "Evo";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtPopulation)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Map)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn1Step;
        private System.Windows.Forms.Button btnToggleTimer;
        private System.Windows.Forms.PictureBox Map;
        private System.Windows.Forms.TextBox txtStats;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ColorDialog bgColorDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbEnergy;
        private System.Windows.Forms.RadioButton rbColor;
        private System.Windows.Forms.RadioButton rbAge;
        private System.Windows.Forms.RadioButton rbSightRange;
        private System.Windows.Forms.RadioButton rbMinEneryAcceptable;
        private System.Windows.Forms.RadioButton rbPurpose;
        private System.Windows.Forms.RadioButton rbFertility;
        private System.Windows.Forms.RadioButton rbLifeTime;
        private System.Windows.Forms.RadioButton rbAggression;
        private System.Windows.Forms.RadioButton rbStrength;
        private System.Windows.Forms.RadioButton rbDesire;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numSpeed;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtPopulation;
    }
}

