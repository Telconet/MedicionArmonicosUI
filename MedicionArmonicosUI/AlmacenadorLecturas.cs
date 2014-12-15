using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.IO;

namespace MedicionArmonicosUI
{
    class AlmacenadorLecturas
    {
        private BaseDeDatos bd;
        private Queue<medicionesBrutas> colaLecturas;
        private Configuracion config;
        private int capacidadInicial = 1000;
        private const int TAMANO_LOTE = 1000;
        private Thread tProcesamientoLecturas;
        private bool finalizar;
        private int tiempoSleep;
        private String subfijo;


        //Estructura que contiene los datos de las lecturas
        public struct lectura
        {
            public readonly DateTime fecha;
            public readonly int canal;
            public readonly int id_medicion;
            public readonly float dato;
            public readonly string tabla;

            public lectura(DateTime fechaLectura, int id, int canal, float datoLectura, string tabla)
            {
                this.dato = datoLectura;
                this.fecha = fechaLectura;
                this.id_medicion = id;
                this.tabla = tabla;
                this.canal = canal;
            }
        }

        //Mediciones recibidas por el ADC.
        public class medicionesBrutas
        {
            //public readonly double[,] mediciones;
            public readonly IntPtr mediciones;
            public readonly DateTime fechaInicioMediciones;
            public readonly DateTime fechaFinMediciones;
            public readonly int canalInicial;
            public readonly short[] lecturasArr;

            public medicionesBrutas(IntPtr mediciones,  DateTime fecha)
            {
                this.mediciones = mediciones;
                this.fechaInicioMediciones = fecha;
                this.canalInicial = 0;
                this.lecturasArr = null;
                this.fechaFinMediciones = DateTime.Now;
            }

            public medicionesBrutas(ref short[] lecturasArr, int canalIncial, DateTime fechaFin)
            {
                this.mediciones = IntPtr.Zero;
                this.fechaFinMediciones = fechaFin;
                this.lecturasArr = lecturasArr;
                this.canalInicial = 0;
                this.fechaInicioMediciones = DateTime.Now;
            }
        }

        //Constructor
        public AlmacenadorLecturas(Configuracion config, String subfijo)
        {
            this.colaLecturas = new Queue<medicionesBrutas>(capacidadInicial);
            this.config = config;
            this.finalizar = false;
            this.subfijo = subfijo;
            /*this.bd = new BaseDeDatos(config.obtenerParametro(Configuracion.Parametro.ip_bd),
                                             config.obtenerParametro(Configuracion.Parametro.nombre_bd),
                                             config.obtenerParametro(Configuracion.Parametro.usuario_bd),
                                             config.obtenerParametro(Configuracion.Parametro.clave_bd));*/
        }

        public void finalizarAlmacenamiento()
        {
            this.finalizar = true;
            if (this.tProcesamientoLecturas != null)
            {
                this.tProcesamientoLecturas.Join();
            }
        }

        public void iniciarAlmacenamiento(int id, MccDaq.MccBoard tarjeta, MccDaq.Range rango, int numeroCanales, int numeroMuestras, int frecuencia, String ruta, bool horaMedicion, int periodosDia, float divisor, float transformador, bool medicionContinua)
        {
            //Creamos el thread que se encargara de almacenar los datos o escribirlos al archivo de texto

            int frecuenciaTotal = numeroCanales * frecuencia;

            if (frecuenciaTotal < 70000)
            {
                this.tiempoSleep = 10;
            }
            else tiempoSleep = 0;

            if (!medicionContinua)
            {
                this.tProcesamientoLecturas = new Thread(this.procesarAlmacenarLecturas);
            }
            else
            {
                this.tProcesamientoLecturas = new Thread(this.procesarAlmacenarLecturasContinuas);
            }

            //Creamos los argumentos para el thread
            ArgumentoProcesamientoLecturas args = new ArgumentoProcesamientoLecturas(tarjeta, rango, tProcesamientoLecturas, numeroCanales, numeroMuestras, frecuencia, ruta, horaMedicion, periodosDia, divisor, transformador, id);

            //Empezamos el procesamiento.
            tProcesamientoLecturas.Priority = ThreadPriority.BelowNormal;               //no queremos hacer caer el host
            tProcesamientoLecturas.Name = "HiloAlmacenamiento_" + id.ToString();
            tProcesamientoLecturas.Start(args);
            

        }

        /**
         * Este metodo correra en un thread, y se dedicara a tomar las lecturas del ADC
         * que fueron puesta en colaLecturas, por el thread que lee el ADC.
         */
        /*public void almacenarLecturas( Thread idThread, lectura[] lecturas)
        {
            //TODO
            //Formato de la tabla mediciones_fecha_hora
            while (true)
            {
                lectura[] lec;
                string nombreTabla = null;
                DateTime fechaHora = new DateTime(1999, 1, 1);
                float medicion = 0.0f;
                int id_medicion = 0;

                if (lecturas != null)
                {
                    lec = lecturas;
                    nombreTabla = lec[0].tabla;
                }
                else return;

                if (nombreTabla != null)
                {
                    //Verificamos si no existe tabla.
                    if (!this.bd.existeTabla(nombreTabla))
                    {
                        //creamos la tabla. USAR MYISAM! 10 veces más rápido
                        string consulta = "CREATE TABLE " + nombreTabla + " (fecha DATE NOT NULL, hora_inicio TIME NOT NULL, id_medicion INT(11) NOT NULL, voltaje FLOAT NOT NULL, PRIMARY KEY (fecha, hora_inicio, id_medicion))ENGINE=MYISAM";
                        this.bd.ejecutarNoConsulta(consulta);
                    }

                    StringBuilder cons = new StringBuilder(255);

                    fechaHora = lec[0].fecha;
                    string hora = fechaHora.ToString("HH:mm:s");
                    string fecha = fechaHora.ToString("yyyy-MM-dd");

                    Stopwatch cronometro = new Stopwatch();
                    cronometro.Start();

                    StringBuilder consultaBatch = new StringBuilder(1000);
                    for (int i = 0; i < lec.Length; i++)
                    {
                        //Insertar en lotes
                        consultaBatch.Append("INSERT INTO " + nombreTabla + " (fecha, hora_inicio, id_medicion, voltaje) values ");

                        int tamanoLote = 0;
                        int loteFinal = lec.Length % TAMANO_LOTE;

                        if ((lec.Length - i) == loteFinal)
                        {
                            tamanoLote = loteFinal;
                        }
                        else
                        {
                            tamanoLote = TAMANO_LOTE;
                        }

                        //Usando inserts por lotes de 1000 registros
                        for (int indiceLote = 0; indiceLote < tamanoLote; indiceLote++)
                        {
                            medicion = lec[i + indiceLote].dato;
                            id_medicion = lec[i + indiceLote].id_medicion;

                            consultaBatch.Append("(\'" + fecha + "\',\'" + hora + "\'," + id_medicion.ToString() + "," + medicion.ToString(CultureInfo.InvariantCulture.NumberFormat) + ")");


                            if (indiceLote < tamanoLote - 1)
                            {
                                consultaBatch.Append(",");
                            }
                            else
                            {
                                bd.ejecutarNoConsulta(consultaBatch.ToString());
                                consultaBatch.Remove(0, consultaBatch.Length);
                                i += tamanoLote - 1;
                            }
                        }
                    }
                    //Medimos cuanto nos tomo realizar todas las mediciones
                    cronometro.Stop();
                    TimeSpan ts = cronometro.Elapsed;

                    string tiempoTotal = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    Console.Out.WriteLine("tiempo total para insertar " + lec.Length + " muestras: " + tiempoTotal);
                }
            }
        }*/

        /**
         * Almacena las lecturas en archivos de texto 
         */
        public void almacenarLecturasTexto(Thread idThread, lectura[,] lecturasADC, String ruta, int frecuenciaMuestreo, int periodosDia ,bool horaMedicion)
        {
            DateTime fechaHora = new DateTime(1999, 1, 1);

            if (lecturasADC != null)
            {
                Stopwatch cronometro = new Stopwatch();
                cronometro.Start();
                for (int canal = 0; canal < lecturasADC.GetLength(0); canal++)
                {
                    //Obtenemos nombre de archivo, de acuerdo a la fecha
                    StringBuilder cons = new StringBuilder(255);
                    fechaHora = lecturasADC[canal, 0].fecha;
                    String nombreArchivo;

                    float ticks = (1.0f / ((float)frecuenciaMuestreo)) * 1000000000.0f / 100.0f;
                    int ticksInt = (int)ticks;                                                      //Se puede perder precisión si frecuencia no es even. 

                    //Abrimos el archivo
                    try
                    {
                        StreamWriter archivoSalida;
                        nombreArchivo = "mediciones_" + "canal_" + canal.ToString() + "_" + fechaHora.ToString("yyyy-MM-dd") + "_hora_" + fechaHora.ToString("HH") + ".txt";

                        //verificar si existe el archivo
                        if (!File.Exists(ruta + nombreArchivo))
                        {
                            archivoSalida = new StreamWriter(ruta + nombreArchivo);    //Si archivo no existe, es creado.
                            String fecha = fechaHora.ToString("yyyy-MM-dd HH:mm:ss");
                            archivoSalida.WriteLine("Fecha de inicio de medicion: " + fechaHora.ToString("yyyy-MM-dd HH:mm:ss ") + " Frecuencia de muestro: " + frecuenciaMuestreo + " Hz.");
                        }
                        else
                        {
                            archivoSalida = new StreamWriter(ruta + nombreArchivo);    //Caso contrario abrimos el archivo
                        }
                        

                        //Almacenamos las mediciones. Teniendo la fecha inicial, y sabiendo la frecuencia
                        //podemos saber el tiempo de cada medición.
                        for (int indiceLectura = 0; indiceLectura < lecturasADC.GetLength(1); indiceLectura++)
                        {
                            if (horaMedicion)
                            {
                                archivoSalida.WriteLine(fechaHora.ToString("HH:mm:ss.fffffff") + "," + lecturasADC[canal, indiceLectura].dato.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                fechaHora = fechaHora.AddTicks(ticksInt);
                            }
                            else
                            {
                                //archivoSalida.WriteLine(lecturasADC[canal, indiceLectura].dato.ToString(CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }

                        archivoSalida.Flush();
                        archivoSalida.Close();
                    }
                    catch (Exception ex)
                    {
                        //TODO: notificar de error
                    }
                    System.Console.WriteLine("Se almacenaron todas las mediciones");
                }

                cronometro.Stop();
                System.Console.WriteLine("Tiempo escribir las {1}: {0}", cronometro.Elapsed.ToString(), (lecturasADC.GetLength(0)*lecturasADC.GetLength(1)).ToString());
            }
        }
        

        /**
         * Este metodo se encargara de recibir las lecturas del ADC, y ponerlas en el formato apropiado para
         * su almacenamiento. PARA MEDICION CON VENTANAS
         */
        public void recibirLecturas(IntPtr handleLecturas, DateTime fechaHora /*, int numeroMuestras, int numeroCanales, MccDaq.Range rangoMuestreo, MccDaq.MccBoard tarjeta*/)
        {
            
            //Stopwatch cronometro = new Stopwatch();
            //cronometro.Start();
            medicionesBrutas med = new medicionesBrutas(handleLecturas, fechaHora);     //canal incial no se usa en
                                                                                            //medicion por ventanas.
            lock(colaLecturas)
            {
                this.colaLecturas.Enqueue(med); 
            }
            //cronometro.Stop();
            //System.Console.WriteLine("Tiempo crear la estructura y encolarla: {0}", cronometro.Elapsed.ToString());//
            
        }

        /**
         * Este metodo se encargara de recibir las lecturas del ADC, y ponerlas en el formato apropiado para
         * su almacenamiento. PARA MEDICION CONTINUA.
         */
        public void recibirLecturas(ref short[] lecturas, DateTime fechaHoraFin, int canalInicial)
        {

            //Stopwatch cronometro = new Stopwatch();
            //cronometro.Start();
            medicionesBrutas med = new medicionesBrutas(ref lecturas, canalInicial, fechaHoraFin);

            lock (colaLecturas)
            {
                this.colaLecturas.Enqueue(med);
                //colaLecturas.TrimExcess();
            }
            //cronometro.Stop();
            //System.Console.WriteLine("Tiempo crear la estructura y encolarla: {0}", cronometro.Elapsed.ToString());//
        }

        /**
         * Para procesamiento continuo de lecturas
         */
        private void procesarAlmacenarLecturasContinuas(object parameter)
        {
   
            while (true)
            {
                if (tiempoSleep > 0)
                {
                    Thread.Sleep(this.tiempoSleep);             //Si es muy lento, llegamos a OutOfMemoryException!!
                }
                medicionesBrutas lecturasLocal = null;

                if (finalizar)
                {
                    return;
                }

                lock (colaLecturas)
                {
                    if (colaLecturas.Count > 0)
                    {
                        lecturasLocal = colaLecturas.Dequeue();
                    }
                }

                

               if (lecturasLocal != null)
                {
                    MccDaq.MccBoard tarjeta = ((ArgumentoProcesamientoLecturas)parameter).tarjeta;
                    MccDaq.Range rangoADC = ((ArgumentoProcesamientoLecturas)parameter).rango;
                    Thread idThread = ((ArgumentoProcesamientoLecturas)parameter).idThread;
                    String ruta = ((ArgumentoProcesamientoLecturas)parameter).ruta;
                    int frecuencia = ((ArgumentoProcesamientoLecturas)parameter).frecuencia;
                    bool horaMedicion = ((ArgumentoProcesamientoLecturas)parameter).horaMedicion;
                    int numeroCanales = ((ArgumentoProcesamientoLecturas)parameter).numeroCanales;
                    int numeroMuestras = ((ArgumentoProcesamientoLecturas)parameter).numeroMuestras;
                    int periodosDia = ((ArgumentoProcesamientoLecturas)parameter).periodosDia;
                    float divisorVoltaje = ((ArgumentoProcesamientoLecturas)parameter).divisorVoltaje;
                    float vueltasTransformador = ((ArgumentoProcesamientoLecturas)parameter).vueltasTransformador;
                    int idTarjeta = ((ArgumentoProcesamientoLecturas)parameter).idTarjeta;

                    //System.Console.WriteLine("Tamano de la cola tarjeta " + idTarjeta + " es: " + colaLecturas.Count);       //BORRAR

                    short[] datosADC = lecturasLocal.lecturasArr;

                    float ticks = (1.0f / ((float)frecuencia)) * 1000000000.0f / 100.0f;        //Ticks por muestra
                    int ticksInt = (int)ticks;
                    int ticksTotalesInt = ticksInt * datosADC.Length;                                //restamos los ticks para saber el tiempo de la primera lectura

                    DateTime fechaHoraInicio = lecturasLocal.fechaInicioMediciones.Subtract(new TimeSpan(ticksTotalesInt));


                    if (datosADC != null)
                    {

                        bool divisorValido = true;
                        bool transformadorValido = true;

                        //Si tenemos un divisor de voltaje valido, procedemos
                        if (divisorValido && transformadorValido)
                        {
                            //Abrimos los archivos a escribir
                            StreamWriter archivoSalida;
                            String nombreArchivo = "mediciones_" + this.subfijo + "_" + fechaHoraInicio.ToString("yyyy-MM-dd") + ".txt";
                            
                            //Stopwatch cronometro = new Stopwatch();
                            //cronometro.Start();

                            //verificar si existe el archivo
                            if (!File.Exists(ruta + nombreArchivo))
                            {
                                archivoSalida = new StreamWriter(ruta + nombreArchivo, true);    //Si archivo no existe, es creado.
                                String fecha = fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss");
                                archivoSalida.WriteLine("Fecha de inicio de medicion: " + fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss.fffffff ") + ", Frecuencia de muestreo: " + frecuencia + " Hz.");
                                archivoSalida.Flush();
                            }
                            else
                            {
                                archivoSalida = new StreamWriter(ruta + nombreArchivo, true);    //Caso contrario abrimos el archivo
                            }

                            //empezamos a escribir
                            int canal = lecturasLocal.canalInicial;
                            for (int indiceLectura = 0; indiceLectura < datosADC.Length; indiceLectura++)
                            {
                                if (canal == numeroCanales)
                                {
                                    canal = 0;
                                }

                                //Almacenamos las mediciones. Teniendo la fecha inicial, y sabiendo la frecuencia
                                //podemos saber el tiempo de cada medición.
                                float voltaje = 1.0f;

                                //NO se puede usar mientras se realiza un SCAN. WTF???
                                //MccDaq.ErrorInfo stat = tarjeta.ToEngUnits(rangoADC, datosADC[indiceLectura], out voltaje)
                                if (rangoADC == MccDaq.Range.Bip10Volts)
                                {
                                    //voltaje = 0.0048828125f * ((float)datosADC[indiceLectura]) - 10.0f;
                                    //voltaje = (float)datosADC[indiceLectura];
                                    //PAra 16 bits y +/-10V
                                    //voltaje = 0.00030517578125f * ((float)datosADC[indiceLectura]) - 10.0f;
                                    MccDaq.ErrorInfo stat = tarjeta.ToEngUnits(rangoADC, datosADC[indiceLectura], out voltaje);
                                }


                                //Si medimos un solo sistema trifasico y dos temperaturas en una sola tarjeta
                                /*if (idTarjeta == 0)
                                {
                                    //canales 0 a 3 son las fases.
                                    if (canal < 4)
                                    {
                                        //convertirlo a voltaje con el divisor 
                                        voltaje = voltaje / divisorVoltaje;

                                        //ahora al voltaje reale
                                        voltaje = voltaje * vueltasTransformador;
                                    }
                                    else
                                    {
                                        //convertir voltaje a temperatura
                                        voltaje = voltaje * 100.0000000f;
                                    }
                                }*/

                                if (idTarjeta == 0)
                                {
                                    //convertirlo a voltaje con el divisor 
                                    voltaje = voltaje / divisorVoltaje;

                                    //ahora al voltaje reale
                                    voltaje = voltaje * vueltasTransformador;
                                }
                                else if (idTarjeta == 1)
                                {
                                    //convertir voltaje a temperatura
                                    voltaje = voltaje * 100.0000000f;
                                }

                                if (horaMedicion)
                                {
                                    archivoSalida.WriteLine(fechaHoraInicio.ToString("HH:mm:ss.fffffff") + "," + voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat));
                                    
                                    if (canal == numeroCanales - 1)
                                    {
                                        archivoSalida.WriteLine(fechaHoraInicio.ToString("HH:mm:ss.fffffff") + "," + voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + ",");
                                    }
                                    else archivoSalida.WriteLine(fechaHoraInicio.ToString("HH:mm:ss.fffffff") + "," + voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + ",");

                                    fechaHoraInicio.Add(new TimeSpan(ticksInt));
                                }
                                else
                                {
                                    if (canal == numeroCanales - 1)
                                    {
                                        //archivoSalida.Write("canal " + canal.ToString() + ": " + voltaje.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\r\n");
                                        archivoSalida.Write(voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + "\r\n");
                                        //archivoSalida.Write(voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + "\r\n");
                                    }
                                    else
                                    {
                                        //archivoSalida.Write("canal " + canal.ToString() + ": " + voltaje.ToString(CultureInfo.InvariantCulture.NumberFormat) + ", ");
                                        archivoSalida.Write(voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + ",");
                                        //archivoSalida.Write(voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat) + "\r\n");
                                    }
                                    
                                }

                                archivoSalida.Flush();
                                canal++;


                                String nombreArchivoSgtLectura = "mediciones_" + this.subfijo + "_" + fechaHoraInicio.ToString("yyyy-MM-dd") + ".txt";

                                //La siguiente lectura va en nuevo archivo.
                                if(!nombreArchivoSgtLectura.Equals(nombreArchivo))
                                {
                                    archivoSalida.Close();
                                    nombreArchivo = nombreArchivoSgtLectura;

                                    if (!File.Exists(ruta + nombreArchivo))
                                    {
                                        archivoSalida = new StreamWriter(ruta + nombreArchivo, true);    //Si archivo no existe, es creado.
                                        String fecha = fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss");
                                        archivoSalida.WriteLine("Fecha de inicio de medicion: " + fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss ") + " Frecuencia de muestro: " + frecuencia + " Hz.");
                                    }
                                    else
                                    {
                                        archivoSalida = new StreamWriter(ruta + nombreArchivo,true);    //Caso contrario abrimos el archivo
                                    }
                                }
                            }

                            archivoSalida.Flush();
                            archivoSalida.Close();
                            datosADC = null;
                            nombreArchivo = null;
                            archivoSalida = null;
                            lecturasLocal = null;
                        }
                    }
                }
            }
        }


        /**
         * Este procesamiento lo realizamos en segundo plano, para evitar que el DAQ espere.
         */
        private void procesarAlmacenarLecturas(object parameter)
        {
            
            //TODO: sacar lecturas de la cola
            while (true)
            {
                Thread.Sleep(1000);
                medicionesBrutas lecturasLocal = null;

                if (finalizar)
                {
                    return;
                }

                lock (colaLecturas)
                {
                    if (colaLecturas.Count > 0)
                    {
                        lecturasLocal = colaLecturas.Dequeue(); 
                    }
                }

                if (lecturasLocal != null)
                {
                    MccDaq.MccBoard tarjeta = ((ArgumentoProcesamientoLecturas)parameter).tarjeta;
                    MccDaq.Range rangoADC = ((ArgumentoProcesamientoLecturas)parameter).rango;
                    Thread idThread = ((ArgumentoProcesamientoLecturas)parameter).idThread;
                    String ruta = ((ArgumentoProcesamientoLecturas)parameter).ruta;
                    int frecuencia = ((ArgumentoProcesamientoLecturas)parameter).frecuencia;
                    int idTarjeta = ((ArgumentoProcesamientoLecturas)parameter).idTarjeta;
                    bool horaMedicion = ((ArgumentoProcesamientoLecturas)parameter).horaMedicion;
                    int numeroCanales = ((ArgumentoProcesamientoLecturas)parameter).numeroCanales;
                    int numeroMuestras = ((ArgumentoProcesamientoLecturas)parameter).numeroMuestras;
                    int periodosDia = ((ArgumentoProcesamientoLecturas)parameter).periodosDia;
                    float divisorVoltaje = ((ArgumentoProcesamientoLecturas)parameter).divisorVoltaje;
                    float vueltasTransformador = ((ArgumentoProcesamientoLecturas)parameter).vueltasTransformador;

                    ushort[] datosADC = new ushort[numeroMuestras];
                    MccDaq.ErrorInfo ULStat = MccDaq.MccService.WinBufToArray(lecturasLocal.mediciones, datosADC, 0, numeroMuestras);

                    DateTime fechaHoraInicio = lecturasLocal.fechaInicioMediciones;

                    if (datosADC != null)
                    {
                        
                        bool divisorValido = true; 
                        bool transformadorValido = true; 

                        //Si tenemos un divisor de voltaje valido, procedemos
                        if (divisorValido && transformadorValido)
                        {
                            int muestrasPorCanal = numeroMuestras / numeroCanales;

                            //Abrimos los archivos a escribir
                            StreamWriter[] archivosSalida = new StreamWriter[numeroCanales];
                            for (int indiceArchivo = 0; indiceArchivo < numeroCanales; indiceArchivo++)
                            {
                                String nombreArchivo = "";      

                                if (periodosDia == 24)              //ventana 1 hora
                                {
                                    nombreArchivo = "mediciones_" + subfijo + "_canal_" + indiceArchivo.ToString() + "_" + fechaHoraInicio.ToString("yyyy-MM-dd") + "_hora_" + fechaHoraInicio.ToString("HH") + ".txt";
                                }
                                else if (periodosDia == 1440)        //ventana 1 minuto
                                {
                                    nombreArchivo = "mediciones_" + subfijo + "_canal_" + indiceArchivo.ToString() + "_" + fechaHoraInicio.ToString("yyyy-MM-dd") + "_hora_" + fechaHoraInicio.ToString("HH") + "_minuto_" + fechaHoraInicio.ToString("mm") + ".txt";
                                }
                                try
                                {
                                    //verificar si existe el archivo
                                    if (!File.Exists(ruta + nombreArchivo))
                                    {
                                        archivosSalida[indiceArchivo] = new StreamWriter(ruta + nombreArchivo);    //Si archivo no existe, es creado.
                                        String fecha = fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss");
                                        archivosSalida[indiceArchivo].WriteLine("Fecha de inicio de medicion: " + fechaHoraInicio.ToString("yyyy-MM-dd HH:mm:ss ") + " Frecuencia de muestro: " + frecuencia + " Hz.");
                                        //archivosSalida[indiceArchivo].WriteLine("volateja tarjeta,voltaje calculado,diferencia voltaje,counts");
                                    }
                                    else
                                    {
                                        archivosSalida[indiceArchivo] = new StreamWriter(ruta + nombreArchivo);    //Caso contrario abrimos el archivo
                                    }
                                }
                                catch (Exception e)
                                {
                                    //TODO: Error al abrir los archivos
                                    return;
                                }
                            }

                            Stopwatch cronometro = new Stopwatch();
                            cronometro.Start();

                            int canal = 0;
                            for (int indiceLectura = 0; indiceLectura < datosADC.Length; indiceLectura++)
                            {
                                if (canal == numeroCanales)
                                {
                                    canal = 0;
                                }
                                
                                float ticks = (1.0f / ((float)frecuencia)) * 1000000000.0f / 100.0f;
                                int ticksInt = (int)ticks;                               //Se puede perder precisión si frecuencia no es even. 

                                //Almacenamos las mediciones. Teniendo la fecha inicial, y sabiendo la frecuencia
                                //podemos saber el tiempo de cada medición.
                                float voltaje = 1.0f;

                                //NO se puede usar mientras se realiza un SCAN. WTF???
                                //MccDaq.ErrorInfo stat = tarjeta.ToEngUnits(rangoADC, datosADC[indiceLectura], out voltaje);
                              //Tarjeta 1 son voltajes
                                if (rangoADC == MccDaq.Range.Bip10Volts)
                                {
                                    voltaje = 0.0048828125f * ((float)datosADC[indiceLectura]) - 10.0f;
                                }

                                if (idTarjeta == 0)
                                {
                                    //convertirlo a voltaje con el divisor 
                                    voltaje = voltaje / divisorVoltaje;

                                    //ahora al voltaje reale
                                    voltaje = voltaje * vueltasTransformador;
                                }
                                else if (idTarjeta == 1)
                                {
                                    //convertir voltaje a temperatura
                                    voltaje = voltaje * 100.0000000f;
                                }
                                
                               

                                if (horaMedicion)
                                {
                                    archivosSalida[canal].WriteLine(fechaHoraInicio.ToString("HH:mm:ss.fffffff") + "," + voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat));
                                    fechaHoraInicio = fechaHoraInicio.AddTicks(ticksInt);
                                }
                                else
                                {
                                    archivosSalida[canal].WriteLine(voltaje.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat));
                                    //archivosSalida[canal].WriteLine(voltaje.ToString(CultureInfo.InvariantCulture.NumberFormat) + "," + voltaje_calculado.ToString(CultureInfo.InvariantCulture.NumberFormat) + "," + ((voltaje_calculado - voltaje).ToString(CultureInfo.InvariantCulture.NumberFormat)) + "," + datosADC[indiceLectura].ToString());
                                }

                                archivosSalida[canal].Flush();
                                canal++;
                            }

                            
                            
                            //Cerramos los archivos
                            for (int indiceArchivo = 0; indiceArchivo < numeroCanales; indiceArchivo++)
                            {
                                try
                                {
                                    archivosSalida[indiceArchivo].Close();
                                }
                                catch (Exception e)
                                {
                                    //TODO: Error al abrir los archivos
                                    return;
                                }
                            }


                            cronometro.Stop();
                            System.Console.WriteLine("Tiempo escribir las {1}: {0}", cronometro.Elapsed.ToString(), datosADC.Length.ToString());

                            System.Console.WriteLine("Se almacenaron todas las mediciones");
                        }
                    }
                }
            }
        }
    }
}


/**
 *
 */
public class ArgumentoProcesamientoLecturas
{
    public readonly MccDaq.MccBoard tarjeta;
    public readonly MccDaq.Range rango;
    public readonly Thread idThread;
    public readonly int frecuencia;
    public readonly String ruta;
    public readonly bool horaMedicion;
    public readonly int numeroCanales;
    public readonly int numeroMuestras;
    public readonly int periodosDia;
    public readonly int idTarjeta;
    public readonly float divisorVoltaje;
    public readonly float vueltasTransformador;


    public ArgumentoProcesamientoLecturas(MccDaq.MccBoard tarjeta, MccDaq.Range rango, Thread id, int numeroCanales, int numeroMuestras, int frecuencia, String ruta, bool horaMedicion, int periodosDia, float divisor, float vueltasTransformador, int idTarjeta)
    {
        this.tarjeta = tarjeta;
        this.rango = rango;
        this.idThread = id;
        this.frecuencia = frecuencia;
        this.ruta = ruta;
        this.horaMedicion = horaMedicion;
        this.numeroCanales = numeroCanales;
        this.numeroMuestras = numeroMuestras;
        this.horaMedicion = horaMedicion;
        this.periodosDia = periodosDia;
        this.divisorVoltaje = divisor;
        this.vueltasTransformador = vueltasTransformador;
        this.idTarjeta = idTarjeta;
    }
}