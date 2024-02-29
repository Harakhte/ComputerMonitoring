using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Server : Form
    {
        Listener listener;
        Socket sck;
        float fcpu = 0;
        float fram = 0;
        string diachiip;
        public Server()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart2.ChartAreas[0].AxisY.Maximum = 100;
            listener = new Listener(8);
            listener.SocketAccepted += new Listener.SocketAcceptedHandler(listener_SocketAccepted);
            Load += new EventHandler(Server_Load);
        }

        private void Server_Load(object sender, EventArgs e)
        {
            listener.Start();
        }

        void listener_SocketAccepted(System.Net.Sockets.Socket e)
        {
            Client client = new Client(e);
            client.Received += new Client.ClientReceivedHandler(client_Received);
            client.Sent += new Client.ClientSentHandler(client_sent);
            client.Disconnected += new Client.ClientDisconnectedHandler(client_Disconnected);
            Invoke((MethodInvoker)delegate
                {
                    ListViewItem i = new ListViewItem();
                    i.Text = client.EndPoint.ToString();
                    i.SubItems.Add(client.ID);
                    i.ImageIndex = 0;
                    i.Tag = client;
                    listView1.Items.Add(i);
                });
        }

        private void client_sent(Client sender, byte[] data)
        {
            throw new NotImplementedException();
        }

        void client_Disconnected(Client sender)
        {
            Invoke((MethodInvoker)delegate
              {
                  for (int i = 0; i < listView1.Items.Count; i++)
                  {
                      Client client = listView1.Items[i].Tag as Client;
                      if (client.ID == sender.ID)
                      {
                          listView1.Items.RemoveAt(i);
                          chart1.Series["CPU"].Points.Clear();
                          chart2.Series["RAM"].Points.Clear();
                          timer1.Stop();
                          break;
                      }
                  }
              });
        }
        void client_Received(Client sender, byte[] data)
        {
            Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    Client client = listView1.Items[i].Tag as Client;
                    if (client.ID == sender.ID)
                    {
                        while (true)
                        {
                            var text = Encoding.Default.GetString(data);
                            string chuoi = text.Substring(0, text.IndexOf(","));
                            fcpu = float.Parse(chuoi.Substring(chuoi.IndexOf("/") + 1));
                            fram = float.Parse(text.Substring(text.IndexOf(",") + 1));
                            break;
                        }
                    }
                }
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            chart1.Series["CPU"].Points.AddY(fcpu);
            chart2.Series["RAM"].Points.AddY(fram);
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            diachiip = listView1.SelectedItems[0].Text;
            chart1.Series["CPU"].Points.Clear();
            chart2.Series["RAM"].Points.Clear();
            timer1.Start();
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Shutdown();
        }
    }
}
