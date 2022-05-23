using System;
using System.Windows.Forms;

namespace ARMDoctor
{
    public partial class main : Form
    {
        public main()
        {
            this.Userlogin = "Ламов Владимир";
            //Password A = new Password();
            //A.ShowDialog(this);
            InitializeComponent();
        }

        public string Userlogin;

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about About = new about();
            About.MdiParent = this;
            About.Show();

        }

        private void main_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelUserName.Text += "  " + Userlogin;
        }

        private void методыЛеченияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treatment_methods TM = new treatment_methods();
            TM.MdiParent = this;
            TM.Show();
        }
    }
}
