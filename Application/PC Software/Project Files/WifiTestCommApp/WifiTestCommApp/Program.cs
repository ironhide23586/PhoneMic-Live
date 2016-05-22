using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using TCPConnector;


namespace WifiTestCommApp
{
    class Program
    {
        static int PORT = 2014;

        static void Main(string[] args)
        {
            if (!IsAdmin())
                RestartElevated();
            Console.WriteLine("App started with admin priviledges = " + IsAdmin());
            WifiHotspotComm.Start_Hotspot("PhoneMicLive!", "8948365369o", true);
            Console.WriteLine("Hotspot Started....");
            TCPDataReader TCPRdr = new TCPDataReader(PORT);
            TCPRdr.ReceiveBufferSize = 3200;
            TCPRdr.StartListener();
            

            Console.WriteLine("Listening on port " + PORT + ".....");
            MemoryStream s;
            while (true)
            {
                s = new MemoryStream(TCPRdr.GetReceivedByteData());
                var waveFormat = new WaveFormat(16000, 16, 1); // must match the waveformat of the raw audio
                var waveOut = new WaveOut();
                var rawSource = new RawSourceWaveStream(s, waveFormat);
                waveOut.Init(rawSource);
                waveOut.Play();
            }
            Console.Read();
            WifiHotspotComm.Start_Hotspot(null, null, false);
        }

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartElevated()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = System.AppDomain.CurrentDomain.FriendlyName;
            //startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Environment.Exit(0);
            //System.Windows.Forms.Application.Exit();
        }
    }
}
