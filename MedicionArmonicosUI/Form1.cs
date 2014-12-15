using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace MedicionArmonicosUI
{
    public partial class Form1 : Form
    {
        private AdquisicionDatos dataAcq;
        private bool estaMidiendo;
        private int diasARegistrar;
        private TextBox[] frecuenciasTxtBx;
        private TextBox[] canalesTxtBx;

        private System.Timers.Timer temporizadorBorradoArchivos;

        //constate
        private const int MILISEGUNDOS_POR_DIA = 86400000;
        private const int DIAS_POR_SEMANA = -7;
        private const int DIAS_A_REGISTRAR_POR_DEFECTO = 4;
        

        public Form1()
        {
            InitializeComponent();
            this.dataAcq = null;
            this.estaMidiendo = false;
            this.ventanaCmbBx.SelectedIndex = 0;        //Por defecto 1 minuto
            this.numeroTarjetasCmbBx.SelectedIndex = 1;
            this.diasRegistrosCmbBx.SelectedIndex = 3;
            Thread.CurrentThread.Name = "HiloUI";
            this.frecuenciasTxtBx = new TextBox[4];
            this.canalesTxtBx = new TextBox[4];

            //Referencias
            canalesTxtBx[0] = tarjeta0CanalTxtBx;
            canalesTxtBx[1] = tarjeta1CanalTxtBx;
            canalesTxtBx[2] = tarjeta2CanalTxtBx;
            canalesTxtBx[3] = tarjeta3CanalTxtBx;

            frecuenciasTxtBx[0] = tarjeta0FrecTxtBx;
            frecuenciasTxtBx[1] = tarjeta1FrecTxtBx;
            frecuenciasTxtBx[2] = tarjeta2FrecTxtBx;
            frecuenciasTxtBx[3] = tarjeta3FrecTxtBx;

            //Ya que por defecto habilito 2 tarjetas, deshabilito las opciones
            //para las otras 2
            for (int i = 2; i < 4; i++)
            {
                frecuenciasTxtBx[i].Enabled = false;
                canalesTxtBx[i].Enabled = false;
            }

            //Temporizador de borrado de archivos
            this.temporizadorBorradoArchivos = new System.Timers.Timer(MILISEGUNDOS_POR_DIA);
            
        }

        void temporizadorBorradoArchivos_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //throw new NotImplementedException();
                this.temporizadorBorradoArchivos.Stop();

                //Obtenemos el directorio.
                DirectoryInfo directorio = new DirectoryInfo(this.rutaArchivoTxtBox.Text);

                DateTime fechaHaceSieteDias = DateTime.Now.AddDays(DIAS_POR_SEMANA);         //- 7dias
                FileInfo[] archivos = directorio.GetFiles(".txt");

                //Empezamos a comparar las fechas. Si son menores, borramos el archivo.
                for (int i = 0; i < archivos.Length; i++)
                {
                    //CHECK....
                    DateTime fechaArchivo = archivos[i].CreationTime.Add(new TimeSpan(2,0,0));           //fecha de creacion, añadimos 2 horas por si el archivo se creo minutos antes de media noche

                    if (fechaArchivo.Date < fechaHaceSieteDias.Date)
                    {
                        //TODO: borrar archivo
                        archivos[i].Delete();
                    }
                }

                this.temporizadorBorradoArchivos.Start();
            }
            catch (IOException ex)
            {
                //Si paso algo con los archivos, salimos.
                this.temporizadorBorradoArchivos.Start();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void opcionesMuestreo_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.rutaArchivoTxtBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void rutaArchivoTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!estaMidiendo)
            {
                bool almacenarHora = false;
                bool medicionContinua = false;
                int frecuencia = 0;
                int canalSuperior = 0;
                int ventana = 0;
                int numeroTarjetas = 1;
                float divisorVoltaje, vueltasTrasformador;
                String ruta = null;

                
                //Ruta
                if (this.rutaArchivoTxtBox.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Por favor elija una ruta para almacenar las mediciones.", "Ruta no válida.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    ruta = rutaArchivoTxtBox.Text + "\\";
                }

                //timestamps?
                if (this.tiempoMedicionChckBx.Checked)
                {
                    almacenarHora = true;
                }
                else almacenarHora = false;


                //Medidicion continua
                if (this.medicionContinuaChckBx.Checked)
                {
                    medicionContinua = true;
                }
                else medicionContinua = false;

                //Si no usamos medicion continua, validamos la ventana de medidicon.
                if (!medicionContinua)
                {
                    //Ventana
                    if (this.ventanaCmbBx.Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Por favor elija una ventana de medicion.", "Venta de medición no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        switch (ventanaCmbBx.SelectedIndex)
                        {
                            /*case 0:
                                MessageBox.Show("Por favor elija una ventana de medicion.", "Venta de medición no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;*/
                            case 0:
                                ventana = 3600 / 60;     //1 min
                                break;
                            case 1:
                                ventana = 3600 / 12; //5 minutos
                                break;
                            case 2:
                                ventana = 3600 / 6; //10 minutos
                                break;
                            case 3:
                                ventana = 3600 / 4; //15 minutos
                                break;
                            case 4:
                                ventana = 3600 / 2; //30 minutos
                                break;
                            case 5:
                                ventana = 3600; //60 minutos
                                break;
                            default:
                                MessageBox.Show("Por favor elija una ventana de medicion.", "Venta de medición no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                        }
                    }

                }

                //divisor voltaje
                if (this.divisorVoltajeTxtBx.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Por favor ingrese Divisor de voltaje válido.", "Divisor de voltaje no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (float.TryParse(divisorVoltajeTxtBx.Text.Trim(), out divisorVoltaje))
                    {
                        divisorVoltaje = float.Parse(divisorVoltajeTxtBx.Text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese Divisor de voltaje válido.", "Divisor de voltaje no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }

                //transformador
                if (this.transformadorTxtBx.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Por favor ingrese Divisor de voltaje válido.", "Divisor de voltaje no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (float.TryParse(transformadorTxtBx.Text.Trim(), out vueltasTrasformador))
                    {
                        vueltasTrasformador = float.Parse(transformadorTxtBx.Text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese Divisor de voltaje válido.", "Divisor de voltaje no válido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }


                //Numero tarjetas
                if (this.numeroTarjetasCmbBx.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Por favor ingrese un numero de tarjetas válido.", "Número de tarjetas invalido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (int.TryParse(numeroTarjetasCmbBx.Text.Trim(), out numeroTarjetas))
                    {
                        vueltasTrasformador = int.Parse(numeroTarjetasCmbBx.Text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese un numero de tarjetas válido.", "Número de tarjetas invalido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }

                int[] canalesInferiores = new int[numeroTarjetas];
                int[] canalesSuperiores = new int[numeroTarjetas];
                int[] frecuencias = new int[numeroTarjetas];
                String[] prefijos = new String[numeroTarjetas];

                //Probamos cada uno de los campos
                for (int i = 0; i < numeroTarjetas; i++)
                {
                  
                    //Frecuencia
                    if (this.frecuenciasTxtBx[i].Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Por favor ingrese una frecuencia válida.", "Frecuencia inválida.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (int.TryParse(frecuenciasTxtBx[i].Text.Trim(), out frecuencias[i]))
                        {
                            frecuencias[i] = int.Parse(frecuenciasTxtBx[i].Text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        }
                        else
                        {
                            MessageBox.Show("Por favor ingrese una frecuencia válida.", "Frecuencia inválida.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                    }

                    //Canal superior
                    if (this.canalesTxtBx[i].Text.Trim() == String.Empty)
                    {
                        MessageBox.Show("Por favor ingrese un numero de canal válido.", "Número de canal invalido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (int.TryParse(canalesTxtBx[i].Text.Trim(), out canalesSuperiores[i]))
                        {
                            canalesSuperiores[i] = int.Parse(canalesTxtBx[i].Text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        }
                        else
                        {
                            MessageBox.Show("Por favor ingrese un numero de canal válido.", "Número de canal invalido.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                    }

                    //canal inferior
                    canalesInferiores[i] = 0;

                }

                //Si no tenenemos un valor valido, ponemos los dias por defecto
                if (!int.TryParse(this.diasRegistrosCmbBx.Text, out this.diasARegistrar))
                {
                    this.diasARegistrar = DIAS_A_REGISTRAR_POR_DEFECTO;
                }

                this.diasRegistrosCmbBx.Enabled = false;
                this.temporizadorBorradoArchivos.Elapsed += new System.Timers.ElapsedEventHandler(temporizadorBorradoArchivos_Elapsed);
                this.temporizadorBorradoArchivos.Start();

                //creamos el nuevo objeto
                this.dataAcq = new AdquisicionDatos(canalesInferiores, canalesSuperiores, frecuencias, ventana, almacenarHora, ruta, divisorVoltaje, vueltasTrasformador, medicionContinua, numeroTarjetas);
                this.dataAcq.iniciarAdquisicionDatos();

                //Empezamos a medir
                this.estaMidiendo = true;
                this.iniciarMuestroBttn.Text = "Detener muestro";

            }
            else
            {
                //TODO: para el muestreo
                this.estaMidiendo = false;
                this.iniciarMuestroBttn.Enabled = false;
                this.temporizadorBorradoArchivos.Stop();

                if (dataAcq != null)
                {
                    this.dataAcq.detenerMedicion();
                }
                
                //TODO esperar que threads terminen
                this.iniciarMuestroBttn.Text = "Iniciar muestrreo";
                this.iniciarMuestroBttn.Enabled = true;
                this.diasRegistrosCmbBx.Enabled = true;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataAcq != null)
            {
                this.dataAcq.detenerMedicion();
                this.dataAcq.liberarMemoria();
            }
            Application.Exit();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dataAcq != null)
            {
                this.dataAcq.liberarMemoria();
            }
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (medicionContinuaChckBx.Checked)
            {
                this.ventanaCmbBx.Enabled = false;
            }
            else this.ventanaCmbBx.Enabled = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numeroTarjetasCmbBx_TextChanged(object sender, EventArgs e)
        {
            int numeroTarjetas;

            //Cuando cambie
            if(int.TryParse(numeroTarjetasCmbBx.Text, out numeroTarjetas))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < numeroTarjetas)
                    {
                        if (frecuenciasTxtBx != null & canalesTxtBx != null)
                        {
                            if (frecuenciasTxtBx[i] != null && canalesTxtBx[i] != null)
                            {
                                this.frecuenciasTxtBx[i].Enabled = true;
                                this.canalesTxtBx[i].Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        if (frecuenciasTxtBx != null & canalesTxtBx != null)
                        {
                            if (frecuenciasTxtBx[i] != null && canalesTxtBx[i] != null)
                            {
                                this.frecuenciasTxtBx[i].Enabled = false;
                                this.canalesTxtBx[i].Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
