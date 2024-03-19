using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        static System.Timers.Timer timer;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var port = 12345;
            string ip = textBox1.Text;
            string ip2 = textBox4.Text;
            tcpClient = new TcpClient(ip2, port);
            stream = tcpClient.GetStream();
            textBox2.AppendText("Пользователь" + " " + ip + " " + "подключился к чату");
            textBox2.AppendText(Environment.NewLine);
            Timer();
        }
        public async void Timer()
        {
            int time = 0;
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += ReceiveMessage;
            timer.Enabled = true;
            while (time < 5)
            {
                System.Threading.Thread.Sleep(1000);
                time++;
            }
            timer.Stop();
            timer.Dispose();

        }
        public async void ReceiveMessage(Object source, ElapsedEventArgs e)
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                    string message = Encoding.UTF8.GetString(data, 0, bytesRead);

                    textBox2.Invoke((MethodInvoker)delegate
                    {
                        textBox2.Text += message + Environment.NewLine;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении сообщений: " + ex.Message);
            }
        }
    

        public async Task SendMessage(string message) 
        {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);
                    string formattedMessage = $"{message}\n";
                    textBox2.AppendText(formattedMessage);
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("err: " + ex.Message);
                }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    string message = $"{textBox1.Text}: {textBox3.Text}\n";
                    SendMessage(message);

                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string message = textBox3.Text;
            string ip = textBox1.Text;
            string formattedMessage = $"{message}\n";
            for (int i = 0; i < message.Length; i++)
            {
                if (i == message.Length)
                {
                    textBox2.AppendText(formattedMessage);
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.ScrollToCaret();
                }
            }
        }
    }
}
