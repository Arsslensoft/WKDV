using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SDV
{
    public partial class BOMBCHRONOFRM : Form
    {
        public BOMBCHRONOFRM()
        {
            InitializeComponent();
        }
        internal bool SHOWCHRONO = false;
        private void BOMBCHRONOFRM_Shown(object sender, EventArgs e)
        {
            if (SHOWCHRONO)
            {
                System.Media.SoundPlayer p = new System.Media.SoundPlayer(SDV.Properties.Resources.BOMB);
                
                digitalClockCtrl1.CountDownTime = 4000;
                p.Play();
                digitalClockCtrl1.SetClockType = ClockType.CountDown;
            }
            else
            {

            }
        }

        private void digitalClockCtrl1_CountDownDone()
        {
            Tools.ShutDownSystem(5);
            Tools.close = true;
            Application.Exit();
        }
    }
}
