using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace WifiPhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        SocketClient sc = new SocketClient();
        string testIP = "192.168.173.1";
        int testPort = 2014;

        Microphone mic = Microphone.Default;
        byte[] buffer;
        MemoryStream mStream = new MemoryStream();
        SoundEffect sound;

        bool Connected = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            mic.BufferReady += new EventHandler<EventArgs>(mic_BufferReady);
        }

        private void sendData(byte[] data)
        {
            sc.Connect(testIP, testPort);
            sc.Send(data);
        }

        private void mic_BufferReady(object sender, EventArgs e)
        {
            mic.GetData(buffer);
            sendData(buffer);
            mStream.Write(buffer, 0, buffer.Length);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mic.BufferDuration = TimeSpan.FromMilliseconds(100);
            buffer = new byte[mic.GetSampleSizeInBytes(mic.BufferDuration)];
            mic.Start();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            sound = new SoundEffect(mStream.ToArray(), mic.SampleRate, AudioChannels.Mono);
            sound.Play();
        }

    }

}