using System;
using System.Windows.Forms;

namespace Sketchpad
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}