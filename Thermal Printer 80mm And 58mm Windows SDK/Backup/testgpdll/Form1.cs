using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSDLL;
using System.IO;
using System.Text.RegularExpressions;

namespace testgpdll
{
    public partial class Form1 : Form
    {
        string pictureBoxFilePath = "iu2.jpg";
        Bitmap mBitmap;

        public Form1()
        {
            InitializeComponent();
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            string portname = comboBoxPort.Text;
            if (null == portname || "".Equals(portname))
                return;


            if (radioButtonBySerial.Checked)
            {
                int nBaudrate = int.Parse(comboBoxBaudrate.Text);
                if (nBaudrate <= 0)
                    return;
                int nParity = comboBoxParitybits.SelectedIndex;
                int nStopBits = comboBoxStopbits.SelectedIndex;
                int nDataBits = int.Parse(comboBoxDatabits.Text);
                int nFlowControl = comboBoxFlowControl.SelectedIndex;
                Pos.POS_Open(portname, nBaudrate, nDataBits, nStopBits, nParity, nFlowControl);
            }
            else if (radioButtonByLan.Checked)
            {
                Pos.POS_Open(portname, 0, 0, 0, 0, 2);
            }
            else if (radioButtonByUsb.Checked)
            {
                Pos.POS_Open(portname, 0, 0, 0, 0, 3);
            }

            if (Pos.POS_IsOpen())
            {
                btOpen.Enabled = false;
                btClose.Enabled = true;
                groupBoxOpenBy.Enabled = false;
            }
            else
            {
                btOpen.Enabled = true;
                btClose.Enabled = false;
                groupBoxOpenBy.Enabled = true;
                MessageBox.Show(Pos.lasterror);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Pos.POS_Close();
            btOpen.Enabled = true;
            btClose.Enabled = false;
            groupBoxOpenBy.Enabled = true;
        }

        private void btWrite_Click(object sender, EventArgs e)
        {
            byte[] data;
            if (((txbContent.Text == null) || ("".Equals(txbContent.Text))))
                return;
            data = Encoding.Default.GetBytes(txbContent.Text+"\r\n");
            Pos.POS_Write(data);
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            Pos.POS_Reset();
        }

        private void btFeedLine_Click(object sender, EventArgs e)
        {
            Pos.POS_FeedLine();
        }

        private void btSetMode_Click(object sender, EventArgs e)
        {
            Pos.POS_SetMode(cbPrintMode.SelectedIndex);
        }

        private void btSetMotionUnit_Click(object sender, EventArgs e)
        {
            Pos.POS_SetMotionUnit(Convert.ToInt32(numMotionUnitH.Value),Convert.ToInt32(numMotionUnitV.Value));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButtonBySerial.Select();

            cbPrintMode.SelectedIndex = 0;
            cbCharSet.SelectedIndex = 0;
            cbCodePage.SelectedIndex = 0;
            cbDrawerPin.SelectedIndex = 0;
            cbCutPaperMode.SelectedIndex = 0;
            cbSTextOutFontType.SelectedIndex = 0;
            cbSBarcodeType.SelectedIndex = 0;
            cbSBarcodeFontType.SelectedIndex = 0;
            cbSBarcodeHriFontPosition.SelectedIndex = 2;
            radioButton576.Select();
            btClose.Enabled = false;
        }

        private void btSetCharSetAndCodePage_Click(object sender, EventArgs e)
        {
            Pos.POS_SetCharSetAndCodePage(cbCharSet.SelectedIndex, cbCodePage.SelectedIndex);
        }

        private void btSetLineSpacing_Click(object sender, EventArgs e)
        {
            Pos.POS_SetLineSpacing(Convert.ToInt32(numLineSpacing.Value));
        }

        private void btSetRightSpacing_Click(object sender, EventArgs e)
        {
            Pos.POS_SetRightSpacing(Convert.ToInt32(numRightSpacing.Value));
        }

        private void btKickOutDrawer_Click(object sender, EventArgs e)
        {
            Pos.POS_KickOutDrawer(cbDrawerPin.SelectedIndex, Convert.ToInt32(numDrawerOnTimes.Value), Convert.ToInt32(numDrawerOffTimes.Value));
        }



        private void btCutPaper_Click(object sender, EventArgs e)
        {
            Pos.POS_CutPaper(cbCutPaperMode.SelectedIndex, Convert.ToInt32(numCutPaperDistance.Value));
        }

        private void btSetAreaWidth_Click(object sender, EventArgs e)
        {
            Pos.POS_S_SetAreaWidth(Convert.ToInt32(numSTextOutAreaWidth.Value));
        }

        private void btSTextOut_Click(object sender, EventArgs e)
        {
            if ((txbSTextOut.Text == null) || ("".Equals(txbSTextOut.Text)))
                return;

            int nFontStyle = 0;
            for (int i = 0; i <= 6; i++)
            {
                if (chkedlbSFontStyle.GetItemChecked(i))
                {
                    if (i == 0)
                        nFontStyle |= 1 << 3;
                    else
                        nFontStyle |= 1 << (i + 6);
                }
            }

            Pos.POS_S_TextOut(txbSTextOut.Text + "\n", Convert.ToInt32(numSTextOutOrgx.Value), Convert.ToInt32(numSWidthTimes.Value),
                Convert.ToInt32(numSHeightTimes.Value), cbSTextOutFontType.SelectedIndex, nFontStyle);
        }

        private void btSetBarcode_Click(object sender, EventArgs e)
        {
            if ((txbSBarcode.Text == null) || ("".Equals(txbSBarcode.Text)))
                return;
            Pos.POS_S_SetBarcode(txbSBarcode.Text,Convert.ToInt32(numSBarcodeOrgx.Value),cbSBarcodeType.SelectedIndex+0x41,
                Convert.ToInt32(numSBarcodeUnitWidth.Value),Convert.ToInt32(numSBarcodeHeight.Value),
                cbSBarcodeFontType.SelectedIndex,cbSBarcodeHriFontPosition.SelectedIndex);
        }

        private void radioButton384_CheckedChanged(object sender, EventArgs e)
        {
            if (File.Exists(pictureBoxFilePath))
            {
                Bitmap b = new Bitmap(pictureBoxFilePath);
                int width = 101;
                int height = b.Height * width / b.Width;
                height = (height + 7) / 8 * 8;
                mBitmap = Pos.POS_ResizeBitmap(b, width, height);
                if (null != mBitmap)
                    pictureBoxPicture.Image = Image.FromHbitmap(mBitmap.GetHbitmap());
            }
        }

        private void radioButton576_CheckedChanged(object sender, EventArgs e)
        {
            if (File.Exists(pictureBoxFilePath))
            {
                Bitmap b = new Bitmap(pictureBoxFilePath);
                int width = 576;
                int height = b.Height * width / b.Width;
                height = (height + 7) / 8 * 8;
                mBitmap = Pos.POS_ResizeBitmap(b, width, height);
                if (null != mBitmap)
                    pictureBoxPicture.Image = Image.FromHbitmap(mBitmap.GetHbitmap());
            }
        }

        private void openFileDialogLoadPicture_FileOk(object sender, CancelEventArgs e)
        {
            pictureBoxFilePath = openFileDialogLoadPicture.FileName;
            if (radioButton384.Checked)
                radioButton384_CheckedChanged(sender, e);
            else
                radioButton576_CheckedChanged(sender, e);
        }

        private void buttonPrintPicture_Click(object sender, EventArgs e)
        {
            if (null != mBitmap)
                Pos.POS_PrintBitmap(mBitmap);
        }

        private void buttonLoadPicture_Click(object sender, EventArgs e)
        {
            openFileDialogLoadPicture.ShowDialog();
        }

        private void buttonRTQueryStatus_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[4];
            Pos.POS_RTQueryStatus(buffer);
            MessageBox.Show(buffer[0] + " " + buffer[1] +" " + buffer[2] + " " + buffer[3]);
        }

        private void radioButtonBySerial_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxPort.Items.Clear();
            comboBoxPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            label33.Text = "Port#";
            label29.Visible = true;
            label30.Visible = true;
            label31.Visible = true;
            label32.Visible = true;
            label8.Visible = true;
            comboBoxBaudrate.SelectedIndex = 4;
            comboBoxBaudrate.Visible = true;
            comboBoxDatabits.SelectedIndex = 3;
            comboBoxDatabits.Visible = true;
            comboBoxFlowControl.SelectedIndex = 1;
            comboBoxFlowControl.Visible = true;
            comboBoxStopbits.SelectedIndex = 0;
            comboBoxStopbits.Visible = true;
            comboBoxParitybits.SelectedIndex = 0;
            comboBoxParitybits.Visible = true;
            if (comboBoxPort.Items.Count > 0)
                comboBoxPort.SelectedIndex = 0;
        }

        private void radioButtonByLan_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxPort.Items.Clear();
            comboBoxBaudrate.Visible = false;
            comboBoxDatabits.Visible = false;
            comboBoxFlowControl.Visible = false;
            comboBoxStopbits.Visible = false;
            comboBoxParitybits.Visible = false;
            comboBoxPort.Text = "Enter the ip address";
            label33.Text = "IP: ";
            label29.Visible = false;
            label30.Visible = false;
            label31.Visible = false;
            label32.Visible = false;
            label8.Visible = false;
        }

        private void radioButtonByUsb_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxPort.Items.Clear();
            foreach (string i in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                comboBoxPort.Items.Add(i);
            comboBoxBaudrate.Visible = false;
            comboBoxDatabits.Visible = false;
            comboBoxFlowControl.Visible = false;
            comboBoxStopbits.Visible = false;
            comboBoxParitybits.Visible = false;
            label33.Text = "USB: ";
            label29.Visible = false;
            label30.Visible = false;
            label31.Visible = false;
            label32.Visible = false;
            label8.Visible = false;
            if (comboBoxPort.Items.Count > 0)
                comboBoxPort.SelectedIndex = 0;
        }
    }
}