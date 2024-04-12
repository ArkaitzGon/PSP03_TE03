/****
 Servidor
 */
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TareaEvaluacion01
{
    public partial class Form1 : Form
    {

        private UdpClient socketServidor;
        private const int puerto = 2000;

        // Lista para almacenar los clientes
        List<IPEndPoint> clientes = new List<IPEndPoint>();
        public Form1()
        {
            InitializeComponent();
            iniciaImagen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /*
         Metodo que inicializa la imagen
         */
        private void iniciaImagen()
        {
            pictureBox1.Image = Image.FromFile("img\\birt.PNG");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//Ajusta imagen al PictureBox
            this.Controls.Add(pictureBox1);

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                socketServidor = new UdpClient(puerto);

                while (true)
                {

                    UdpReceiveResult resultados = await socketServidor.ReceiveAsync();

                    //Guarda los datos del contenido en un array y los vuelca a un string
                    byte[] datosRecibidos = resultados.Buffer;
                    string mensajeRecibido = Encoding.ASCII.GetString(datosRecibidos);

                    string[] nombreMensaje = mensajeRecibido.Split(';');
                    string nombre = nombreMensaje[0];
                    string mensaje = nombreMensaje[1];



                    IPEndPoint clienteEndpoint = resultados.RemoteEndPoint;

                    // Añade el cliente a la lista si aún no está en ella
                    if (!clientes.Contains(clienteEndpoint))
                    {
                        clientes.Add(clienteEndpoint);
                        textBox3.Text += nombre + "\r\n";
                        textBox2.Text += nombre + " se ha conectado\r\n";

                    }
                    textBox1.Text = clientes.Count.ToString();


                    byte[] mensajeRespuesta = Encoding.ASCII.GetBytes(mensaje);

                    // Envía el mensaje a todos los clientes
                    foreach (IPEndPoint cliente in clientes)
                    {
                        await socketServidor.SendAsync(mensajeRespuesta, mensajeRespuesta.Length, cliente);
                    }

                    textBox2.Text += nombre + ": " + mensaje + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (socketServidor != null)
            {
                socketServidor.Close();
            }
        }
    }
}
