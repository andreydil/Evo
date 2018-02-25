namespace Evo.GUI.Winforms
{
    partial class FrmNewWorld
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewWorld));
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numRandomSeed = new System.Windows.Forms.NumericUpDown();
            this.lblRandomSeed = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.chkSetRandomSeed = new System.Windows.Forms.CheckBox();
            this.lblPopulation = new System.Windows.Forms.Label();
            this.numPopulationSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbIndividualsRandom = new System.Windows.Forms.RadioButton();
            this.rbIndividualsAverage = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRandomSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPopulationSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Location = new System.Drawing.Point(142, 266);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(223, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // numRandomSeed
            // 
            this.numRandomSeed.Enabled = false;
            this.numRandomSeed.Location = new System.Drawing.Point(169, 47);
            this.numRandomSeed.Name = "numRandomSeed";
            this.numRandomSeed.Size = new System.Drawing.Size(107, 20);
            this.numRandomSeed.TabIndex = 2;
            this.numRandomSeed.ValueChanged += new System.EventHandler(this.numRandomSeed_ValueChanged);
            // 
            // lblRandomSeed
            // 
            this.lblRandomSeed.AutoSize = true;
            this.lblRandomSeed.Enabled = false;
            this.lblRandomSeed.Location = new System.Drawing.Point(6, 49);
            this.lblRandomSeed.Name = "lblRandomSeed";
            this.lblRandomSeed.Size = new System.Drawing.Size(75, 13);
            this.lblRandomSeed.TabIndex = 3;
            this.lblRandomSeed.Text = "Random Seed";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(12, 14);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(35, 13);
            this.lblWidth.TabIndex = 5;
            this.lblWidth.Text = "Width";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(184, 12);
            this.numWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(113, 20);
            this.numWidth.TabIndex = 4;
            this.numWidth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(12, 40);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(38, 13);
            this.lblHeight.TabIndex = 7;
            this.lblHeight.Text = "Height";
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(184, 38);
            this.numHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(113, 20);
            this.numHeight.TabIndex = 6;
            this.numHeight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chkSetRandomSeed
            // 
            this.chkSetRandomSeed.AutoSize = true;
            this.chkSetRandomSeed.Location = new System.Drawing.Point(9, 19);
            this.chkSetRandomSeed.Name = "chkSetRandomSeed";
            this.chkSetRandomSeed.Size = new System.Drawing.Size(106, 17);
            this.chkSetRandomSeed.TabIndex = 8;
            this.chkSetRandomSeed.Text = "Set random seed";
            this.chkSetRandomSeed.UseVisualStyleBackColor = true;
            this.chkSetRandomSeed.CheckedChanged += new System.EventHandler(this.chkSetRandomSeed_CheckedChanged);
            // 
            // lblPopulation
            // 
            this.lblPopulation.AutoSize = true;
            this.lblPopulation.Location = new System.Drawing.Point(12, 66);
            this.lblPopulation.Name = "lblPopulation";
            this.lblPopulation.Size = new System.Drawing.Size(57, 13);
            this.lblPopulation.TabIndex = 10;
            this.lblPopulation.Text = "Population";
            // 
            // numPopulationSize
            // 
            this.numPopulationSize.Location = new System.Drawing.Point(184, 64);
            this.numPopulationSize.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numPopulationSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numPopulationSize.Name = "numPopulationSize";
            this.numPopulationSize.Size = new System.Drawing.Size(113, 20);
            this.numPopulationSize.TabIndex = 9;
            this.numPopulationSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbIndividualsRandom);
            this.groupBox1.Controls.Add(this.rbIndividualsAverage);
            this.groupBox1.Location = new System.Drawing.Point(15, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 71);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Individuals";
            // 
            // rbIndividualsRandom
            // 
            this.rbIndividualsRandom.AutoSize = true;
            this.rbIndividualsRandom.Location = new System.Drawing.Point(9, 42);
            this.rbIndividualsRandom.Name = "rbIndividualsRandom";
            this.rbIndividualsRandom.Size = new System.Drawing.Size(65, 17);
            this.rbIndividualsRandom.TabIndex = 14;
            this.rbIndividualsRandom.TabStop = true;
            this.rbIndividualsRandom.Text = "Random";
            this.rbIndividualsRandom.UseVisualStyleBackColor = true;
            // 
            // rbIndividualsAverage
            // 
            this.rbIndividualsAverage.AutoSize = true;
            this.rbIndividualsAverage.Checked = true;
            this.rbIndividualsAverage.Location = new System.Drawing.Point(9, 19);
            this.rbIndividualsAverage.Name = "rbIndividualsAverage";
            this.rbIndividualsAverage.Size = new System.Drawing.Size(65, 17);
            this.rbIndividualsAverage.TabIndex = 13;
            this.rbIndividualsAverage.TabStop = true;
            this.rbIndividualsAverage.Text = "Average";
            this.rbIndividualsAverage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSetRandomSeed);
            this.groupBox2.Controls.Add(this.lblRandomSeed);
            this.groupBox2.Controls.Add(this.numRandomSeed);
            this.groupBox2.Location = new System.Drawing.Point(15, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 86);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Random";
            // 
            // FrmNewWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(310, 301);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblPopulation);
            this.Controls.Add(this.numPopulationSize);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNewWorld";
            this.Text = "New World";
            ((System.ComponentModel.ISupportInitialize)(this.numRandomSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPopulationSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numRandomSeed;
        private System.Windows.Forms.Label lblRandomSeed;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.CheckBox chkSetRandomSeed;
        private System.Windows.Forms.Label lblPopulation;
        private System.Windows.Forms.NumericUpDown numPopulationSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbIndividualsRandom;
        private System.Windows.Forms.RadioButton rbIndividualsAverage;
    }
}