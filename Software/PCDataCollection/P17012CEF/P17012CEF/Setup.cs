using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P17012CEF
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            ConnectBtn.Enabled = false;
            ConnectBtn.Text = "Working";
            bool connected = Program.btc.Connect(COMPortCombo.SelectedItem.ToString());
            if (connected)
            {
                Close();
            }
            else
            {
                ConnectBtn.Text = "Start";
                ConnectBtn.Enabled = true;
                MessageBox.Show("Unable to open communication port.");
            }
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            COMPortCombo.Items.AddRange(ports);
        }
    }
}
