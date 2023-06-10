
namespace PrinterControl
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectBtn = new System.Windows.Forms.Button();
            this.printerListBox = new System.Windows.Forms.ComboBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.resetPrinterBtn = new System.Windows.Forms.Button();
            this.lineFeedBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cutPaperBtn = new System.Windows.Forms.Button();
            this.connectedLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(111, 55);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 0;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // printerListBox
            // 
            this.printerListBox.FormattingEnabled = true;
            this.printerListBox.Location = new System.Drawing.Point(49, 28);
            this.printerListBox.Name = "printerListBox";
            this.printerListBox.Size = new System.Drawing.Size(218, 21);
            this.printerListBox.TabIndex = 1;
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(192, 55);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 2;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // resetPrinterBtn
            // 
            this.resetPrinterBtn.Location = new System.Drawing.Point(6, 19);
            this.resetPrinterBtn.Name = "resetPrinterBtn";
            this.resetPrinterBtn.Size = new System.Drawing.Size(264, 23);
            this.resetPrinterBtn.TabIndex = 3;
            this.resetPrinterBtn.Text = "Reset Printer";
            this.resetPrinterBtn.UseVisualStyleBackColor = true;
            this.resetPrinterBtn.Click += new System.EventHandler(this.resetPrinterBtn_Click);
            // 
            // lineFeedBtn
            // 
            this.lineFeedBtn.Location = new System.Drawing.Point(6, 48);
            this.lineFeedBtn.Name = "lineFeedBtn";
            this.lineFeedBtn.Size = new System.Drawing.Size(264, 23);
            this.lineFeedBtn.TabIndex = 4;
            this.lineFeedBtn.Text = "Line Feed";
            this.lineFeedBtn.UseVisualStyleBackColor = true;
            this.lineFeedBtn.Click += new System.EventHandler(this.lineFeedBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectedLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.printerListBox);
            this.groupBox1.Controls.Add(this.connectBtn);
            this.groupBox1.Controls.Add(this.closeBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 89);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer Communication";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Printer";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cutPaperBtn);
            this.groupBox2.Controls.Add(this.resetPrinterBtn);
            this.groupBox2.Controls.Add(this.lineFeedBtn);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 120);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Printer Control";
            // 
            // cutPaperBtn
            // 
            this.cutPaperBtn.Location = new System.Drawing.Point(6, 77);
            this.cutPaperBtn.Name = "cutPaperBtn";
            this.cutPaperBtn.Size = new System.Drawing.Size(264, 23);
            this.cutPaperBtn.TabIndex = 5;
            this.cutPaperBtn.Text = "Paper Cutter";
            this.cutPaperBtn.UseVisualStyleBackColor = true;
            this.cutPaperBtn.Click += new System.EventHandler(this.cutPaperBtn_Click);
            // 
            // connectedLabel
            // 
            this.connectedLabel.AutoSize = true;
            this.connectedLabel.Location = new System.Drawing.Point(6, 60);
            this.connectedLabel.Name = "connectedLabel";
            this.connectedLabel.Size = new System.Drawing.Size(82, 13);
            this.connectedLabel.TabIndex = 3;
            this.connectedLabel.Text = "Not Connected.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 557);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.ComboBox printerListBox;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button resetPrinterBtn;
        private System.Windows.Forms.Button lineFeedBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cutPaperBtn;
        private System.Windows.Forms.Label connectedLabel;
    }
}

