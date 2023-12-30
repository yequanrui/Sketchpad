using System;
using System.Windows.Forms;

namespace Sketchpad
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}