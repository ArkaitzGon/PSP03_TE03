/*****
 Cliente
 */
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Cliente
{
    public partial class Form1 : Form
    {

        private UdpClient clienteUDP = new UdpClient();
        IPEndPoint servidorEndpoint;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //IP y puerto del servidor para la conexión
                string ipServidor = textBox1.Text;//IP
                int puertoServidor = 2000;//puerto
                servidorEndpoint = new IPEndPoint(IPAddress.Parse(ipServidor), puertoServidor);


            }
            catch (Exception error)
            {
                Debug.WriteLine("Error: {0}", error.ToString());
            }

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            // Recojo el texto de los textbox 
            string nombre = textBox2.Text;
            string mensaje = textBox3.Text;

            // Junto los string para mandarlos juntos 
            string envio = nombre + ";" + mensaje;
            // Codificamos a byte para mandar el mensaje
            byte[] mensajeEnviar = Encoding.ASCII.GetBytes(envio);
            await clienteUDP.SendAsync(mensajeEnviar, mensajeEnviar.Length, servidorEndpoint);

            // Esperar la recepción de la respuesta del servidor 
            UdpReceiveResult resultado = await clienteUDP.ReceiveAsync();

            // Convertir la respuesta recibida en una cadena de texto
            string respuesta = Encoding.ASCII.GetString(resultado.Buffer);

            textBox4.Text += respuesta + "\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(clienteUDP!=null)
            {
                clienteUDP.Close();
            }
            
        }
    }
}
