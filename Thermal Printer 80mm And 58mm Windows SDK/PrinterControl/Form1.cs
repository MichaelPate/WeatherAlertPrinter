using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSDLL;

namespace PrinterControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string portname = printerListBox.Text;                  // Try to connect to listed printer from combo box
            if (null == portname || "".Equals(portname)) return;    // Dont do anything if the box is empty

            Pos.POS_Open(portname, 0, 0, 0, 0, 3);                  // Assuming a USB printer so use POS_Open

            if (Pos.POS_IsOpen())                                   // If connected, change ena
            {
                connectBtn.Enabled = false;
                closeBtn.Enabled = true;
                connectedLabel.Text = "Connected.";
            }
            else                                                    // If not connected, show a message box with the error
            {                                          
                connectBtn.Enabled = true;
                closeBtn.Enabled = false;
                connectedLabel.Text = "Error.";
                MessageBox.Show(Pos.lasterror);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string i in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                printerListBox.Items.Add(i);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Pos.POS_Close();
            connectBtn.Enabled = true;
            closeBtn.Enabled = false;
            connectedLabel.Text = "Not Connected.";
        }

        private void resetPrinterBtn_Click(object sender, EventArgs e)
        {
            Pos.POS_Reset();
        }

        private void lineFeedBtn_Click(object sender, EventArgs e)
        {
            Pos.POS_FeedLine();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cutPaperBtn_Click(object sender, EventArgs e)
        {
            Pos.POS_CutPaper(0, 0);
        }
    }
}
