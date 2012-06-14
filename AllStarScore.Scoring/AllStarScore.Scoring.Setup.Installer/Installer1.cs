using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;


namespace AllStarScore.Scoring.Setup.Installer
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
//            System.Diagnostics.Debugger.Launch();
//            System.Diagnostics.Debugger.Break();
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8085);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            System.Diagnostics.Process.Start("http://www.microsoft.com");
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
    }

    //https://github.com/ravendb/ravendb/blob/master/Raven.Database/Server/NonAdminHttp.cs
    public static class NonAdminHttp
    {
        public static void EnsureCanListenToWhenInNonAdminContext(int port)
        {
//            if (CanStartHttpListener(port))
//                return;

            var exit = TryGrantingHttpPrivileges(port);

            if (CanStartHttpListener(port) == false)
                Console.WriteLine("Failed to grant rights for listening to http, exit code: " + exit);
        }

        private static void GetArgsForHttpAclCmd(int port, out string args, out string cmd)
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                cmd = "netsh";
                args = string.Format(@"http add urlacl url=http://+:{0}/ user=""{1}""", port,
                                     @"IIS AppPool\DefaultAppPool");
            }
            else
            {
                cmd = "httpcfg";
                args = string.Format(@"set urlacl /u http://+:{0}/ /a D:(A;;GX;;;""{1}"")", port,
                                     WindowsIdentity.GetCurrent().User);
            }
        }


        private static bool CanStartHttpListener(int port)
        {
            try
            {
                var httpListener = new HttpListener();
                httpListener.Prefixes.Add("http://+:" + port + "/");
                httpListener.Start();
                httpListener.Stop();
                return true;
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode != 5) //access denies
                    throw;
            }
            return false;
        }

        private static int TryGrantingHttpPrivileges(int port)
        {
            string args;
            string cmd;
            GetArgsForHttpAclCmd(port, out args, out cmd);

            Console.WriteLine("Trying to grant rights for http.sys");
            try
            {
                Console.WriteLine("runas {0} {1}", cmd, args);
                var process = Process.Start(new ProcessStartInfo
                {
                    Verb = "runas",
                    Arguments = args,
                    FileName = cmd,
                });
                process.WaitForExit();
                return process.ExitCode;
            }
            catch (Exception)
            {
                return -144;
            }
        }
    }
}
