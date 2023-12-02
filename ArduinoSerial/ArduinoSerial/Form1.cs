using System.IO.Ports;
using System.Windows.Forms;

namespace ArduinoSerial
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;

            // Enumerar puertos COM disponibles y agregarlos al ComboBox
            string[] puertosDisponibles = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(puertosDisponibles);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Manejar datos recibidos del Arduino (si es necesario)
            string receivedData = serialPort.ReadLine();
            // Puedes mostrar receivedData en el ListBox de mensajes recibidos.
            MostrarMensajeRecibido(receivedData);
        }

        private void MostrarMensajeRecibido(string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarMensajeRecibido(mensaje)));
            }
            else
            {
                listBox1.Items.Add(mensaje);
                // Desplázate al último elemento para ver el mensaje más reciente
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Obtener el mensaje del TextBox
            string mensaje = textBox1.Text;

            // Enviar el mensaje al Arduino
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(mensaje);
            }

            // Puedes mostrar el mensaje enviado en el ListBox de mensajes recibidos si lo deseas.
            MostrarMensajeRecibido("Enviado a " + serialPort.PortName + " :" + mensaje);

            // Limpiar el TextBox de mensaje
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                // Obtener el puerto seleccionado desde el ComboBox
                string puertoSeleccionado = comboBox1.SelectedItem as string;

                if (puertoSeleccionado != null)
                {
                    // Configurar el puerto serie Bluetooth
                    serialPort.PortName = puertoSeleccionado;
                    serialPort.BaudRate = 9600;   // Ajustar la velocidad de baudios según la configuración de tu Arduino

                    try
                    {
                        // Espera 500 milisegundos antes de intentar abrir el puerto
                        Thread.Sleep(500);
                        serialPort.Open();
                        MostrarMensajeRecibido("Conectado al puerto serie");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al abrir el puerto serie: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un puerto COM antes de conectar.");
                }
            }
            else
            {
                MostrarMensajeRecibido("Ya conectado al puerto serie ");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (serialPort.IsOpen)
            {
                serialPort.Close();
                MostrarMensajeRecibido("Desconectado del puerto serie");
            }
            else
            {
                MostrarMensajeRecibido("No estás conectado al puerto serie");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            String encender = "A";
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(encender);
            }
            else
            {
                MostrarMensajeRecibido("No estás conectado a ningun puerto");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String apagar = "B";
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(apagar);

            }
            else
            {
                MostrarMensajeRecibido("No estás conectado a ningun puerto");
            }
        }
    }
}