using System;
using System.Windows.Forms;
using Evo.Core.Universe;

namespace Evo.GUI.Winforms
{
    public partial class FrmNewWorld : Form
    {
        public FrmNewWorld(int width, int height)
        {
            InitializeComponent();
            numWidth.Value = width;
            numHeight.Value = height;
        }

        public World GetWorld()
        {
            var random = chkSetRandomSeed.Checked ? new Random((int) numRandomSeed.Value) : new Random();
            var worldConfigurator = new ParamsWorldConfigurator((int)numWidth.Value, (int)numHeight.Value);
            worldConfigurator.Random = random;
            worldConfigurator.RandomIndividuals = rbIndividualsRandom.Checked;
            return worldConfigurator.CreateWorld();
        }
        
        private void chkSetRandomSeed_CheckedChanged(object sender, EventArgs e)
        {
            lblRandomSeed.Enabled = numRandomSeed.Enabled = chkSetRandomSeed.Checked;
        }

        private void numRandomSeed_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
