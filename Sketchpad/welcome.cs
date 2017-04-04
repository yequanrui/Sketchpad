using System;
using System.Windows.Forms;

namespace Sketchpad
{
    public partial class welcome : Form
    {
        public welcome()
        {
            InitializeComponent();
        }

        private void lbl_enter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}