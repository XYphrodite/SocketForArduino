using System;
using System.Windows.Forms;
using SimpleTCP;

namespace SocketForArduino
{
    public partial class Form1 : Form
    {
        bool lB = false;
        int oldX=0, oldY=0, oldZ = 0;
        bool isFirst = true;
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;
            server.StringEncoder = System.Text.Encoding.UTF8;
            richTextBox1.ReadOnly = true;
        }
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            richTextBox1.Invoke((MethodInvoker)delegate ()
            {
                chartX.Series["X"].Points.Clear();
                chartX.Series["Y"].Points.Clear();
                chartX.Series["Z"].Points.Clear();
                String digit=String.Empty;
                int x, y, z;
                for (int i = 0; i < 4; i++)
                {
                    digit += e.MessageString[i];
                }
                x=decode(digit);
                digit = String.Empty;
                for (int i = 4; i < 8; i++)
                {
                    digit += e.MessageString[i];
                }
                y = decode(digit);
                digit = String.Empty;
                for (int i = 8; i < 12; i++)
                {
                    digit += e.MessageString[i];
                }
                z = decode(digit);
                richTextBox1.Text += "Recieved at " + DateTime.Now.ToString("h:mm:ss tt") + 
                ":\tX = " + x.ToString() + ", Y = " + y.ToString() + ", Z = " + z.ToString() + Environment.NewLine;
                chartX.Series["X"].Points.Add(x);
                chartX.Series["Y"].Points.Add(y);
                chartX.Series["Z"].Points.Add(z);
                if (!isFirst)
                {
                    chart1.Series[0].Points.Add(getDisance(oldX, oldY, oldZ, x, y, z));
                    
                }
                else
                {
                    isFirst = false;
                }
                oldX = x;
                oldY = y;
                oldZ = z;
            });
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (!lB)
            {
                System.Net.IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");
                server.Start(ip, int.Parse(portTextBox.Text));
                listenButton.Text = "Stop listening";
                richTextBox1.Text += "Server is running...\n";
                server.DataReceived += Server_DataReceived;
            }
            else
            {
                server.Stop();
                listenButton.Text = "Start listening";
                richTextBox1.Text += "Server was stopped\n";
            }
            lB = !lB;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            chartX.Series["X"].Points.Clear();
            chartX.Series["Y"].Points.Clear();
            chartX.Series["Z"].Points.Clear();
            chart1.Series[0].Points.Clear();
        }

        private int decode(String a)
        {
            bool minus = false;
            int result = 0;
            if (a[0]=='p')
            {

            }
            else if (a[0]=='m')
            {
                minus = true;
            }
            else
            {
                return 0;
            }
            switch (a[1])
            {
                case 'a':
                    result += 100;
                    break;
                case 'b':
                    result += 200;
                    break;
                case 'c':
                    result += 300;
                    break;
                case 'd':
                    result += 400;
                    break;
                case 'e':
                    result += 500;
                    break;
                case 'f':
                    result += 600;
                    break;
                case 'g':
                    result += 700;
                    break;
                case 'h':
                    result += 800;
                    break;
                case 'i':
                    result += 900;
                    break;
                case 'v':
                    break;
            }
            switch (a[2])
            {
                case 'a':
                    result += 10;
                    break;
                case 'b':
                    result += 20;
                    break;
                case 'c':
                    result += 30;
                    break;
                case 'd':
                    result += 40;
                    break;
                case 'e':
                    result += 50;
                    break;
                case 'f':
                    result += 60;
                    break;
                case 'g':
                    result += 70;
                    break;
                case 'h':
                    result += 80;
                    break;
                case 'i':
                    result += 90;
                    break;
                case 'v':
                    break;
            }
            switch (a[3])
            {
                case 'a':
                    result += 1;
                    break;
                case 'b':
                    result += 2;
                    break;
                case 'c':
                    result += 3;
                    break;
                case 'd':
                    result += 4;
                    break;
                case 'e':
                    result += 5;
                    break;
                case 'f':
                    result += 6;
                    break;
                case 'g':
                    result += 7;
                    break;
                case 'h':
                    result += 8;
                    break;
                case 'i':
                    result += 9;
                    break;
                case 'v':
                    break;
            }
            if (minus)
            {
                result = -result;
            }
            return result;
        }

        private double getDisance(int x1,int y1, int z1, int x2, int y2, int z2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
        }
    }
}
