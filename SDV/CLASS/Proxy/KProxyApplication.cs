namespace BrowserWeb
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Security;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Windows.Forms;

    public class SDVPApplication
    {
        internal static PreferenceBag _Prefs = new PreferenceBag(null);
        public static bool isClosing;
        internal static readonly PeriodicWorker Janitor = new PeriodicWorker();
        internal static X509Certificate oDefaultClientCertificate;
        [CodeDescription("Web Browser's core proxy engine.")]
        public static Proxy oProxy;
        internal static SDVPTranscoders oTranscoders = new SDVPTranscoders();

        internal static  event SessionStateHandler AfterSessionComplete;

        internal static event SessionStateHandler BeforeRequest;
        internal static event SessionStateHandler LocalHost;

        internal static event SessionStateHandler BeforeResponse;

        internal static event SessionStateHandler BeforeReturningError;

        [CodeDescription("Sync this event to be notified when Web Browser has attached as the system proxy.")]
        internal static event SimpleEventHandler SDVPAttach;

        [CodeDescription("Sync this event to be notified when Web Browser has detached as the system proxy.")]
        internal static event SimpleEventHandler SDVPDetach;

        internal static event EventHandler<NotificationEventArgs> OnNotification;

        internal static event EventHandler<RawReadEventArgs> OnReadResponseBuffer;

        internal static event EventHandler<ValidateServerCertificateEventArgs> OnValidateServerCertificate;

        internal static event SessionStateHandler RequestHeadersAvailable;

        internal static event SessionStateHandler ResponseHeadersAvailable;

        internal static void CallLocalHost(Session session)
        {
            if (LocalHost != null)
            {
                LocalHost(session);
            }
        }
        static SDVPApplication()
        {
            _SetXceedLicenseKeys();
        }

        private SDVPApplication()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void _SetXceedLicenseKeys()
        {
        }

        internal static void CheckOverrideCertificatePolicy(Session oS, string sExpectedCN, X509Certificate ServerCertificate, X509Chain ServerCertificateChain, SslPolicyErrors sslPolicyErrors, ref CertificateValidity oValidity)
        {
            if (OnValidateServerCertificate != null)
            {
                ValidateServerCertificateEventArgs e = new ValidateServerCertificateEventArgs(oS, sExpectedCN, ServerCertificate, ServerCertificateChain, sslPolicyErrors);
                OnValidateServerCertificate(oS, e);
                oValidity = e.ValidityState;
            }
        }

        public static Proxy CreateProxyEndpoint(int iPort, bool bAllowRemote, X509Certificate2 certHTTPS)
        {
            Proxy proxy = new Proxy(false);
            if (certHTTPS != null)
            {
                proxy.AssignEndpointCertificate(certHTTPS);
            }
            if (proxy.Start(iPort, bAllowRemote))
            {
                return proxy;
            }
            proxy.Dispose();
            return null;
        }

        public static Proxy CreateProxyEndpoint(int iPort, bool bAllowRemote, string sHTTPSHostname)
        {
            Proxy proxy = new Proxy(false);
            if (!string.IsNullOrEmpty(sHTTPSHostname))
            {
                proxy.ActAsHTTPSEndpointForHostname(sHTTPSHostname);
            }
            if (proxy.Start(iPort, bAllowRemote))
            {
                return proxy;
            }
            proxy.Dispose();
            return null;
        }

        internal static void DebugSpew(string sMessage)
        {
            if (Browser.bDebugSpew)
            {
                Trace.WriteLine(sMessage);
            }
        }

        internal static void DoAfterSessionComplete(Session oSession)
        {
            if (AfterSessionComplete != null)
            {
                AfterSessionComplete(oSession);
            }
        }

        internal static void DoBeforeRequest(Session oSession)
        {
            if (BeforeRequest != null)
            {
                BeforeRequest(oSession);
            }
        }

        internal static void DoBeforeResponse(Session oSession)
        {
            if (BeforeResponse != null)
            {
                BeforeResponse(oSession);
            }
        }

        internal static void DoBeforeReturningError(Session oSession)
        {
            if (BeforeReturningError != null)
            {
                BeforeReturningError(oSession);
            }
        }

        public static bool DoExport(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> ehPCEA)
        {
            if (string.IsNullOrEmpty(sExportFormat))
            {
                return false;
            }
            TranscoderTuple tuple = oTranscoders.GetExporter(sExportFormat);
            if (tuple == null)
            {
                return false;
            }
            bool flag = false;
            try
            {
                ISessionExporter exporter = (ISessionExporter) Activator.CreateInstance(tuple.typeFormatter);
                if (ehPCEA == null)
                {
                    ehPCEA = delegate (object sender, ProgressCallbackEventArgs oPCE) {
                        string str = (oPCE.PercentComplete > 0) ? ("Export is " + oPCE.PercentComplete + "% complete; ") : string.Empty;
                        
                        Application.DoEvents();
                    };
                }
                flag = exporter.ExportSessions(sExportFormat, oSessions, dictOptions, ehPCEA);
                exporter.Dispose();
            }
            catch (Exception exception)
            {
                LogAddonException(exception, "Exporter for " + sExportFormat + " failed.");
                flag = false;
            }
            return flag;
        }

        public static Session[] DoImport(string sImportFormat, bool bAddToSessionList, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> ehPCEA)
        {
            Session[] sessionArray;
            if (string.IsNullOrEmpty(sImportFormat))
            {
                return null;
            }
            TranscoderTuple tuple = oTranscoders.GetImporter(sImportFormat);
            if (tuple == null)
            {
                return null;
            }
            try
            {
                ISessionImporter importer = (ISessionImporter) Activator.CreateInstance(tuple.typeFormatter);
                if (ehPCEA == null)
                {
                    ehPCEA = delegate (object sender, ProgressCallbackEventArgs oPCE) {
                        string str = (oPCE.PercentComplete > 0) ? ("Import is " + oPCE.PercentComplete + "% complete; ") : string.Empty;
                 
                        Application.DoEvents();
                    };
                }
                sessionArray = importer.ImportSessions(sImportFormat, dictOptions, ehPCEA);
                importer.Dispose();
                if (sessionArray == null)
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                LogAddonException(exception, "Importer for " + sImportFormat + " failed.");
                sessionArray = null;
            }
            return sessionArray;
        }

        internal static void DoNotifyUser(string sMessage, string sTitle)
        {
            DoNotifyUser(sMessage, sTitle, MessageBoxIcon.None);
        }

        internal static void DoNotifyUser(string sMessage, string sTitle, MessageBoxIcon oIcon)
        {
            if (OnNotification != null)
            {
                NotificationEventArgs e = new NotificationEventArgs(string.Format("{0} - {1}", sTitle, sMessage));
                OnNotification(null, e);
            }
            if (!Browser.QuietMode)
            {
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, oIcon);
            }
        }

        internal static bool DoReadResponseBuffer(Session oS, byte[] arrBytes, int cBytes)
        {
            if (OnReadResponseBuffer != null)
            {
                RawReadEventArgs e = new RawReadEventArgs(oS, arrBytes, cBytes);
                OnReadResponseBuffer(oS, e);
                if (e.AbortReading)
                {
                    return false;
                }
            }
            return true;
        }

        internal static void DoRequestHeadersAvailable(Session oSession)
        {
            if (RequestHeadersAvailable != null)
            {
                RequestHeadersAvailable(oSession);
            }
        }

        internal static void DoResponseHeadersAvailable(Session oSession)
        {
            if (ResponseHeadersAvailable != null)
            {
                ResponseHeadersAvailable(oSession);
            }
        }

        public static string GetDetailedInfo()
        {
            string str3 = string.Empty;
            string str = str3 + "\nRunning on: " + Browser.sMachineNameLowerCase + ":" + oProxy.ListenPort.ToString() + "\n";
            if (Browser.bHookAllConnections)
            {
                str = str + "Listening to: All Adapters\n";
            }
            else
            {
                str = str + "Listening to: " + (Browser.sHookConnectionNamed ?? "Default LAN") + "\n";
            }
            if (Browser.iReverseProxyForPort > 0)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "Acting as reverse proxy for port #", Browser.iReverseProxyForPort, "\n" });
            }
            if (oProxy.oAutoProxy != null)
            {
                str = str + "Gateway: Using Script\n" + oProxy.oAutoProxy.ToString();
            }
            else
            {
                IPEndPoint point = oProxy.FindGatewayForOrigin("http", "www.google.com");
                if (point != null)
                {
                    string str4 = str;
                    str = str4 + "Gateway: " + point.Address.ToString() + ":" + point.Port.ToString();
                }
                else
                {
                    str = str + "Gateway: No Gateway";
                }
            }
            string str2 = string.Empty;
            return string.Format("Web Browser ({0})\n{9}\n{1}-bit {2}, VM: {3:N2}mb, WS: {4:N2}mb\n{5}\n{6}\n\nYou've run SDVP: {7:N0} times.\n{8}\n", new object[] { Browser.bIsBeta ? string.Format("v{0} beta", Application.ProductVersion) : string.Format("v{0}", Application.ProductVersion), (8 == IntPtr.Size) ? "64" : "32", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"), Process.GetCurrentProcess().PagedMemorySize64 / 0x100000L, Process.GetCurrentProcess().WorkingSet64 / 0x100000L, ".NET " + Environment.Version, Environment.OSVersion.VersionString, Browser.iStartupCount, str, str2 });
        }

        public static string GetVersionString()
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string str = string.Empty;
            string str2 = "Web Browser";
            return string.Format("{0}/{1}.{2}.{3}.{4}{5}", new object[] { str2, versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart, str });
        }

        internal static void HandleHTTPError(Session oSession, SessionFlags flagViolation, bool bPoisonClientConnection, bool bPoisonServerConnection, string sMessage)
        {
            if (bPoisonClientConnection)
            {
                oSession.PoisonClientPipe();
            }
            if (bPoisonServerConnection)
            {
                oSession.PoisonServerPipe();
            }
            oSession.SetBitFlag(flagViolation, true);
            oSession["ui-backcolor"] = "LightYellow";
            sMessage = "[ProtocolViolation] " + sMessage;
            if ((oSession["x-HTTPProtocol-Violation"] == null) || !oSession["x-HTTPProtocol-Violation"].Contains(sMessage))
            {
                Session session;
                (session = oSession)["x-HTTPProtocol-Violation"] = session["x-HTTPProtocol-Violation"] + sMessage;
            }
        }

        public static bool IsStarted()
        {
            return (null != oProxy);
        }

        public static bool IsSystemProxy()
        {
            return ((oProxy != null) && oProxy.IsAttached);
        }

        internal static void LogAddonException(Exception eX, string sTitle)
        {
            if (Prefs.GetBoolPref("SDVP.debug.extensions.showerrors", false) || Prefs.GetBoolPref("SDVP.debug.extensions.verbose", false))
            {
                ReportException(eX, sTitle);
            }
        }

        internal static void OnSDVPAttach()
        {
            if (SDVPAttach != null)
            {
                SDVPAttach();
            }
        }

        internal static void OnSDVPDetach()
        {
            if (SDVPDetach != null)
            {
                SDVPDetach();
            }
        }

        internal static void ReportException(Exception eX)
        {
            ReportException(eX, "Sorry, you may have found a bug...");
        }

        public static void ReportException(Exception eX, string sTitle)
        {
            if (!(eX is ThreadAbortException) || !isClosing)
            {
                if (eX is ConfigurationErrorsException)
                {
                    DoNotifyUser(string.Concat(new object[] { 
                        "Your Microsoft .NET Configuration file is corrupt and contains invalid data. You can often correct this error by installing updates from WindowsUpdate and/or reinstalling the .NET Framework.\r\n", eX.Message, "\n\nType: ", eX.GetType().ToString(), "\nSource: ", eX.Source, "\n", eX.StackTrace, "\n\n", eX.InnerException, "\nSDVP v", Application.ProductVersion, (8 == IntPtr.Size) ? " (x64 " : " (x86 ", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"), ") [.NET ", Environment.Version, 
                        " on ", Environment.OSVersion.VersionString, "] "
                     }), sTitle, MessageBoxIcon.Hand);
                }
                else
                {
                    string str;
                    if (eX is OutOfMemoryException)
                    {
                        sTitle = "Out of Memory Error";
                        str = "An out-of-memory exception was encountered. To help avoid out-of-memory conditions. ";
                    }
                    else
                    {
                        str = "Web Browser has encountered an unexpected problem. If you believe this is a bug in SDVP, please copy this message by hitting CTRL+C, and submit a bug report using the Help | Send Feedback menu.\n\n";
                    }
                    DoNotifyUser(string.Concat(new object[] { 
                        str, eX.Message, "\n\nType: ", eX.GetType().ToString(), "\nSource: ", eX.Source, "\n", eX.StackTrace, "\n\n", eX.InnerException, "\nSDVP v", Application.ProductVersion, (8 == IntPtr.Size) ? " (x64 " : " (x86 ", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"), ") [.NET ", Environment.Version, 
                        " on ", Environment.OSVersion.VersionString, "] "
                     }), sTitle, MessageBoxIcon.Hand);
                }
                Trace.Write(string.Concat(new object[] { eX.Message, "\n", eX.StackTrace, "\n", eX.InnerException }));
            }
        }

        [CodeDescription("Reset the SessionID counter to 0. This method can lead to confusing UI, so call sparingly.")]
        public static void ResetSessionCounter()
        {
            Session.ResetSessionCounter();
        }

        public static void Shutdown()
        {
            if (oProxy != null)
            {
                oProxy.Detach();
                oProxy.Dispose();
                oProxy = null;
            }
        }

        public static void Startup(int iListenPort, SDVPStartupFlags oFlags)
        {
            if (oProxy != null)
            {
                throw new InvalidOperationException("Calling startup twice without calling shutdown is not permitted.");
            }
            if ((iListenPort < 0) || (iListenPort > 0xffff))
            {
                throw new ArgumentOutOfRangeException("bListenPort", "Port must be between 0 and 65535.");
            }
            Browser.ListenPort = iListenPort;
            Browser.bAllowRemoteConnections = SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.AllowRemoteClients);
            Browser.bMITM_HTTPS = SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.DecryptSSL);
            Browser.bCaptureCONNECT = true;
            Browser.bForwardToGateway = SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.ChainToUpstreamGateway);
            Browser.bHookAllConnections = SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.MonitorAllConnections);
            if (SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.CaptureLocalhostTraffic))
            {
                Browser.sHostsThatBypassSDVP = Browser.sHostsThatBypassSDVP;
            }
            oProxy = new Proxy(true);
            if (oProxy.Start(Browser.ListenPort, Browser.bAllowRemoteConnections))
            {
                if (iListenPort == 0)
                {
                    Browser.ListenPort = oProxy.ListenPort;
                }
                if (SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.RegisterAsSystemProxy))
                {
                    oProxy.Attach(true);
                }
                else if (SDVPStartupFlags.None < (oFlags & SDVPStartupFlags.ChainToUpstreamGateway))
                {
                    oProxy.CollectConnectoidAndGatewayInfo();
                }
            }
        }

        public static void Startup(int iListenPort, bool bRegisterAsSystemProxy, bool bDecryptSSL)
        {
            SDVPStartupFlags oFlags = SDVPStartupFlags.Default;
            if (bRegisterAsSystemProxy)
            {
                oFlags |= SDVPStartupFlags.RegisterAsSystemProxy;
            }
            else
            {
                oFlags &= ~SDVPStartupFlags.RegisterAsSystemProxy;
            }
            if (bDecryptSSL)
            {
                oFlags |= SDVPStartupFlags.DecryptSSL;
            }
            else
            {
                oFlags &= ~SDVPStartupFlags.DecryptSSL;
            }
            Startup(iListenPort, oFlags);
        }

        public static void Startup(int iListenPort, bool bRegisterAsSystemProxy, bool bDecryptSSL, bool bAllowRemote)
        {
            SDVPStartupFlags oFlags = SDVPStartupFlags.Default;
            if (bRegisterAsSystemProxy)
            {
                oFlags |= SDVPStartupFlags.RegisterAsSystemProxy;
            }
            else
            {
                oFlags &= ~SDVPStartupFlags.RegisterAsSystemProxy;
            }
            if (bDecryptSSL)
            {
                oFlags |= SDVPStartupFlags.DecryptSSL;
            }
            else
            {
                oFlags &= ~SDVPStartupFlags.DecryptSSL;
            }
            if (bAllowRemote)
            {
                oFlags |= SDVPStartupFlags.AllowRemoteClients;
            }
            else
            {
                oFlags &= ~SDVPStartupFlags.AllowRemoteClients;
            }
            Startup(iListenPort, oFlags);
        }

        [CodeDescription("Web Browser's Preferences collection.")]
        internal static ISDVPPreferences Prefs
        {
            get
            {
                return _Prefs;
            }
        }
    }
}

