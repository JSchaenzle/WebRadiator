using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebpageRadiator
{
    public partial class SelectPagesFrm : Form
    {
        public SelectPagesFrm()
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void SelectPagesFrm_Load(object sender, EventArgs e)
        {
        
        }
    }
}
