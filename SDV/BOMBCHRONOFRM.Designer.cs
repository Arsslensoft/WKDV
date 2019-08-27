namespace SDV
{
    partial class BOMBCHRONOFRM
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
            this.digitalClockCtrl1 = new SDV.DigitalClockCtrl();
            this.SuspendLayout();
            // 
            // digitalClockCtrl1
            // 
            this.digitalClockCtrl1.BackColor = System.Drawing.Color.Black;
            this.digitalClockCtrl1.CountDownTime = 10000;
            this.digitalClockCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.digitalClockCtrl1.Location = new System.Drawing.Point(0, 0);
            this.digitalClockCtrl1.Name = "digitalClockCtrl1";
            this.digitalClockCtrl1.SetClockType = SDV.ClockType.DigitalClock;
            this.digitalClockCtrl1.Size = new System.Drawing.Size(677, 266);
            this.digitalClockCtrl1.TabIndex = 0;
            this.digitalClockCtrl1.CountDownDone += new SDV.DigitalClockCtrl.CountDown(this.digitalClockCtrl1_CountDownDone);
            // 
            // BOMBCHRONOFRM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 266);
            this.Controls.Add(this.digitalClockCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BOMBCHRONOFRM";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.BOMBCHRONOFRM_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DigitalClockCtrl digitalClockCtrl1;
    }
}