using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace UART_Niron
{
    public partial class Form1 : Form
    {
        string TRN_Data;
        string RCV_Data;


        public Form1()
        {
            InitializeComponent();
        }
        /* Initialization */
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames(); // Get the ports names provided by the PC
            cBoxCOMPORT.Items.AddRange(ports);
            btnOpen.Enabled = true; // Initial state of the button " Open "
            btnClose.Enabled = false;  // Initial state of the button " Close "
            chUpData.Checked = false;
            chExData.Checked = false;
        }
        /* -Open- Button config. */
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {   
                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text); // Converts the string to a type-StopBits
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);// Converts the string to a type-Parity

                serialPort1.Open();
                progressBar1.Value = 100;
                btnOpen.Enabled = false; // Prevent clicking the button when the port is already active
                btnClose.Enabled = true;
                lblStatusCom.Text = "OPEN";
            }
            catch(Exception err)
            {
                if(cBoxCOMPORT.Text=="")
                    MessageBox.Show("Please Choose Port!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if(cBoxBaudRate.Text=="")
                    MessageBox.Show("Please Choose Baud Rate !", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if(cBoxDataBits.Text == "")
                    MessageBox.Show("Please Choose Number of Data bits!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if(cBoxStopBits.Text == "")
                    MessageBox.Show("Please Choose Number of Stop bits!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if(cBoxParityBits.Text == "")
                    MessageBox.Show("Please Choose Number of Parity bits!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                     MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOpen.Enabled = true; 
                btnClose.Enabled = false;
                lblStatusCom.Text = "CLOSED";
            }
        }

        /* -Close- Button config. */
        private void btnClose_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen) //Checks if the port is open ( TRUE = open)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                btnOpen.Enabled = true; 
                btnClose.Enabled = false;
                lblStatusCom.Text = "CLOSED";
            }
        }

        /* -Send- Button config. */
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) //Checks if the port is open ( TRUE = open)
            {
                TRN_Data = tBoxDataOut.Text;
                serialPort1.Write(TRN_Data);
            }
        }

        /* -Clear- Button config. */
        private void btnClearData_Click(object sender, EventArgs e)
        {
            if (tBoxDataOut.Text != "") // If the textbox isnt empty
                tBoxDataOut.Text = ""; // Erase the Content
        }

        

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RCV_Data = serialPort1.ReadExisting(); // Reads all the bytes in the stream
            this.Invoke(new EventHandler(ShowData));
        }

        private void ShowData(object sender, EventArgs e)
        {
            
            if(chUpData.Checked)
                tBoxDataIn.Text = RCV_Data; // Update the data everytime
            if(chExData.Checked)
                tBoxDataIn.Text += RCV_Data; // Add up to the exisiting data

        }

        private void chUpData_CheckedChanged(object sender, EventArgs e)
        {
            if (chUpData.Checked) // Check if the "Update data " checkBox is checked
            {
                chUpData.Checked = true;
                chExData.Checked = false;
            }
            else
                chExData.Checked = true;
        }

        private void chExData_CheckedChanged(object sender, EventArgs e)
        {
            if (chExData.Checked) // Check if the "Add to exisiting data " checkBox is checked
            {
                chExData.Checked = true;
                chUpData.Checked = false;
            }
            else
                chUpData.Checked = true;
        }

        private void btnClearRCV_Click(object sender, EventArgs e)
        {
            if (tBoxDataIn.Text != "")
                tBoxDataIn.Text = "";
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
