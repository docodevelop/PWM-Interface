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

namespace PWM_Interface
{
	public partial class Form1 : Form
	{
		private string DataBuffer = "";
		List<string> str = new List<string>();
		int i = 0;
		double counter = 0;
		public Form1()
		{
			InitializeComponent();
			chart2.ChartAreas[0].AxisY.Minimum = 1;
			chart2.ChartAreas[0].AxisY.Interval = 25;
			chart2.ChartAreas[0].AxisY.Maximum = 260;
			if (serialPort1.IsOpen)
			{
				serialPort1.Close();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string[] availablePorts = SerialPort.GetPortNames();
			//Se agrega un item por default
			//Add a default item
			comboBox1.Items.Add("Selecciona un Puerto Serial");
			foreach (string port in availablePorts)
			{
				comboBox1.Items.Add(port);
			}
			//Seleccionar el primero como default
			//Select the first as default
			comboBox1.SelectedIndex = 0;
		}

		private void MostrarErrorPuerto()
		{
			MessageBox.Show("Debe seleccionar un Puerto Serial", "Mensaje", MessageBoxButtons.OKCancel);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//Boton iniciar
			if (serialPort1.IsOpen)
			{
				i = 0;
				timer1.Start();
				chart2.Series[0].Points.Clear();
			}
			else
			{
				MostrarErrorPuerto();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//Boton detener
			if (serialPort1.IsOpen)
			{
				i = 0;
				timer1.Stop();
				chart2.Series[0].Points.Clear();
			}
			else
			{
				MostrarErrorPuerto();
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Validar que no sea la primera opcion
			//Validate the selected option
			if (comboBox1.SelectedIndex > 0)
			{
				string portName = comboBox1.SelectedItem.ToString();
				serialPort1.PortName = portName;
				//Serial.begin(this is the baud rate value)
				serialPort1.BaudRate = 115200;
				serialPort1.Open();
			}
		}

		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			if (serialPort1.IsOpen)
			{
				label2.Text = trackBar1.Value.ToString();
				agregarValores();
				mandarInfo();
			}
			else
			{
				MostrarErrorPuerto();
			}
		}

		private void agregarValores()
		{
			if (serialPort1.IsOpen)
			{
				str.Add(trackBar1.Value.ToString());
				DataBuffer += "&2-" + trackBar1.Value;
				label4.UseMnemonic = false;
				label4.Text = DataBuffer.ToString() + "&";
			}
			else
			{
				MostrarErrorPuerto();
			}
		}
		private void mandarInfo()
		{
			counter++;
			if (serialPort1.IsOpen)
			{
				if (DataBuffer != "")
				{
					DataBuffer += "&";
					Console.WriteLine(DataBuffer);
					serialPort1.Write(DataBuffer);
					DataBuffer = "";
					label4.Text = "";
				}
				else
				{
					MessageBox.Show("Agregue valores al PWM", "Mensaje", MessageBoxButtons.OKCancel);
				}
			}
			else
			{
				MostrarErrorPuerto();
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			i++;
			if (chart2.Series[0].Points.Count == 20)
			{
				chart2.Series[0].Points.Clear();
				chart2.Series[0].Points.AddXY(i, trackBar1.Value);
				chart2.Series[0].Points.RemoveAt(0);
				chart2.Update();
			}
			chart2.Series[0].Points.AddXY(i, trackBar1.Value);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (serialPort1.IsOpen)
			{
				serialPort1.Close();
			}
		}
	}
}
