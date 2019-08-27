using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace BrowserWeb
{
    public class CertMaker
    {
        internal static ICertificateProvider oCertProvider;

        static CertMaker()
        {
            if (oCertProvider == null)
            {
                File.WriteAllBytes(Application.StartupPath + @"\makecert.exe",SDV.Properties.Resources.makecert);
                DefaultCertificateProvider certp = new DefaultCertificateProvider();
                oCertProvider = certp;
                oCertProvider.CreateRootCertificate();
                if (!CertMaker.rootCertIsTrusted())
                {
                    CertMaker.trustRootCert();
                }
                else
                {

                }
            }
        }

        public static bool createRootCert()
        {
            return oCertProvider.CreateRootCertificate();
        }

        internal static bool exportRootToDesktop()
        {
            try
            {
                byte[] bytes = getRootCertBytes();
                if (bytes != null)
                {
                    File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\ASRoot.cer", bytes);
                    return true;
                }
            }
            catch (Exception exception)
            {
                SDVPApplication.ReportException(exception);
                return false;
            }
            return false;
        }

        internal static X509Certificate2 FindCert(string sHostname, bool allowCreate)
        {
            return oCertProvider.GetCertificateForHost(sHostname);
        }

        internal static byte[] getRootCertBytes()
        {
            X509Certificate2 rootCertificate = GetRootCertificate();
            if (rootCertificate == null)
            {
                return null;
            }
            return rootCertificate.Export(X509ContentType.Cert);
        }

        public static X509Certificate2 GetRootCertificate()
        {
            return oCertProvider.GetRootCertificate();
        }

        private static ICertificateProvider LoadOverrideCertProvider()
        {
            string stringPref = SDVPApplication.Prefs.GetStringPref("SDVP.certmaker.assembly", Browser.GetPath("App") + "CertMaker.dll");
            if (File.Exists(stringPref))
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(stringPref);
                    if (!Utilities.SDVPMeetsVersionRequirement(assembly, "Certificate Maker"))
                    {
                        return null;
                    }
                }
                catch (Exception exception)
                {
                    SDVPApplication.LogAddonException(exception, "Failed to load CertMaker" + stringPref);
                    return null;
                }
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if ((!type.IsAbstract && type.IsPublic) && (type.IsClass && typeof(ICertificateProvider).IsAssignableFrom(type)))
                    {
                        try
                        {
                            return (ICertificateProvider) Activator.CreateInstance(type);
                        }
                        catch (Exception exception2)
                        {
                            SDVPApplication.DoNotifyUser(string.Format("[Web Browser] Failure loading {0} CertMaker from {1}: {2}\n\n{3}\n\n{4}", new object[] { type.Name, assembly.CodeBase, exception2.Message, exception2.StackTrace, exception2.InnerException }), "Load Error");
                        }
                    }
                }
            }
            return null;
        }

        public static void removeSDVPGeneratedCerts()
        {
            removeSDVPGeneratedCerts(true);
        }

        public static void removeSDVPGeneratedCerts(bool bRemoveRoot)
        {
            if (oCertProvider is ICertificateProvider2)
            {
                (oCertProvider as ICertificateProvider2).ClearCertificateCache(bRemoveRoot);
            }
            else
            {
                oCertProvider.ClearCertificateCache();
            }
        }

        public static bool rootCertExists()
        {
            try
            {
                X509Certificate2 rootCertificate = GetRootCertificate();
                return (null != rootCertificate);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool rootCertIsMachineTrusted()
        {
            bool flag;
            bool flag2;
            oCertProvider.rootCertIsTrusted(out flag, out flag2);
            return flag2;
        }

        public static bool rootCertIsTrusted()
        {
            bool flag;
            bool flag2;
            return oCertProvider.rootCertIsTrusted(out flag, out flag2);
        }

        public static bool trustRootCert()
        {
            return oCertProvider.TrustRootCertificate();
        }
    }
}

