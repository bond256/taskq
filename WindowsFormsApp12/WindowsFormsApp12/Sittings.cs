using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Sittings : Form
    {

        public string[] wind_sp;
        public string[] wind_dir;
        public Sittings()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Properties.Settings.Default.dir_sp1= direct_wind1.Text;
            Properties.Settings.Default.dir_sp2 = direct_wind2.Text;
            Properties.Settings.Default.win_sp1= wind_speed1.Text;
            Properties.Settings.Default.win_sp2 = wind_speed2.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
