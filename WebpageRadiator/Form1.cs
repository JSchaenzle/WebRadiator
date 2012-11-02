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
    public partial class Form1 : Form
    {
        List<CustomBrowser> openBrowsers = new List<CustomBrowser>();
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = Properties.Settings.Default.Interval / 1000;
            // Display the first page right away
            this.timer1.Interval = 100;
            this.timer1.Enabled = true;
        }

        private int currentSite = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Uri siteToLoad = getNextUri(ref currentSite);

                DisplayBrowser(siteToLoad);
                this.timer1.Interval = Properties.Settings.Default.Interval;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        void DisplayBrowser(Uri pageToDisplay)
        {
            CustomBrowser browser;
            if (openBrowsers.Any(b => b.BaseUri == pageToDisplay))
            {
                browser = openBrowsers.Single(b => b.BaseUri == pageToDisplay);
            }
            else
            {
                browser = new CustomBrowser(pageToDisplay);
                openBrowsers.Add(browser);
                
                tableLayoutPanel1.Controls.Add(browser, 0, 0);
                tableLayoutPanel1.SetColumnSpan(browser, 6);
                browser.Dock = DockStyle.Fill;
            }
            openBrowsers.ForEach(b => b.Visible = false);
            browser.Visible = true;

        }

        private Uri getNextUri(ref int currentSite)
        {
            if (currentSite == 5 || currentSite == 0)
            {
                currentSite = 1;
                if (Properties.Settings.Default.Site1_Enabled) return new Uri(Properties.Settings.Default.Site1_URL);
                else return getNextUri(ref currentSite);
            }
            else if (currentSite == 1)
            {
                currentSite = 2;
                if (Properties.Settings.Default.Site2_Enabled) return new Uri(Properties.Settings.Default.Site2_URL);
                else return getNextUri(ref currentSite);
            }
            else if (currentSite == 2)
            {
                currentSite = 3;
                if (Properties.Settings.Default.Site3_Enabled) return new Uri(Properties.Settings.Default.Site3_URL);
                else return getNextUri(ref currentSite);
            }
            else if (currentSite == 3)
            {
                currentSite = 4;
                if (Properties.Settings.Default.Site4_Enabled) return new Uri(Properties.Settings.Default.Site4_URL);
                else return getNextUri(ref currentSite);
            }
            else if (currentSite == 4)
            {
                currentSite = 5;
                if (Properties.Settings.Default.Site5_Enabled) return new Uri(Properties.Settings.Default.Site5_URL);
                else return getNextUri(ref currentSite);
            }
            else return new Uri("http://spin.atomicobject.com");
        }

        private void selectPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            SelectPagesFrm frm = new SelectPagesFrm();
            frm.ShowDialog();
            this.timer1.Enabled = true;
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.timer1.Interval = 1;
            this.timer1.Enabled = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Interval = (int)numericUpDown1.Value * 1000;
            Properties.Settings.Default.Save();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.timer1.Interval = 50;
            currentSite--;
            if (currentSite < 0) currentSite = 5;
            this.timer1.Enabled = true;
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {

            if (((Button)sender).Text != "Play")
            {
                ((Button)sender).Text = "Play";
                timer1.Enabled = false;
            }
            else
            {
                ((Button)sender).Text = "Pause";
                timer1.Enabled = true;
            }
        }
    }

    public class CustomBrowser : WebBrowser
    {
        private Uri base_uri;

        public CustomBrowser(Uri uri)
        {
            base_uri = uri;
            this.Navigate(uri);
        }

        public Uri BaseUri
        {
            get{ return base_uri;}
        } 
    }
}
