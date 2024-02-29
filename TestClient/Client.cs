using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestClient
{
    public partial class Client : Form
    {
       // public static Byte[] receivebuffer = new Byte[1024];
        public PerformanceCounter theCPUCounter =
        new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter theMemCounter =
        new PerformanceCounter("Memory", "% Committed Bytes In Use");
        Socket sck;
        public Client()
        {
            InitializeComponent();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /*public void ReceiveOrder()
        {
            byte[] receivebuf = new byte[1024];
            int rec = sck.Receive(receivebuf);
            byte[] data = new byte[rec];
            Array.Copy(receivebuf, data, rec);
            var text = Encoding.ASCII.GetString(data);
            if (text.ToLower() == "shutdown")
            {
                Process.Start("shutdown", "/s /t 0");
            }
        }*/

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            sck.Connect("127.0.0.1", 8);
            MessageBox.Show("Connected");
            while (true)
            {
                sck.Send(Encoding.Default.GetBytes("/" + (theCPUCounter.NextValue().ToString()
                    + "," + theMemCounter.NextValue().ToString())));
                Thread.Sleep(1000);
            }
                /*EndPoint dummyendpoint = new IPEndPoint(IPAddress.Any, 0);
                var length = sck.ReceiveFrom(receivebuffer, ref dummyendpoint);
                var result = Encoding.ASCII.GetString(receivebuffer, 0, length);
                if (String.Compare(result, "OK", true) == 0)
                {
                    Console.WriteLine("OK");
                }
                if (String.Compare("shutdown", result) == 0)
                {
                    Process.Start("shutdown", "/s /t 0");
                }
                Array.Clear(receivebuffer, 0, 1024);
                sck.Close();
                Thread.Sleep(1000);*/
            }
            /*byte[] receivebuf = new byte[1024];
            int rec = sck.Receive(receivebuf);
            byte[] data = new byte[rec];
            Array.Copy(receivebuf, data, rec);
            var text = Encoding.ASCII.GetString(data);
            if (text.ToLower() == "shutdown")
            {
                Process.Start("shutdown", "/s /t 0");
            }*/
        }
    }