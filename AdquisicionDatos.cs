//Para correrlo en una PC sin tarjeta de adquisicion de datos (e.g. laptop)
//#define DEBUG                       

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Globalization;
using System.Diagnostics;


namespace MedicionArmonicosUI
{
    class AdquisicionDatos
    {
        private Configuracion config;
        private AlmacenadorLecturas[] almacenador;
        private int[] tasaMuestreo;
        private int[] canalSuperior;
        private int[] canalInferior;
        private int ventanaMuestreo;
        private bool almacenarHoraMedicion;
        private MccDaq.Range rangoMuestreo;
        private String ruta;
        private bool iniciado;
        private MccDaq.MccBoard[] tarjeta;
        private MccDaq.ScanOptions opcionesADC;
        private System.Timers.Timer[] temporizador;
        private DateTime fecha;
        private bool medicionContinua;
        private float divisorVoltaje;
        private float vueltasTransformador;
        private Form1.TipoArchivo tipoArchivo;


        //Para revision continua
        private int numeroTarjetas;
        short[] status;
        int[] cantidadActual;
        int[] indiceActual;
        int[] indiceAnterior; 
        int[] canalInicial;
        private IntPtr[] handleADC;           //para medicion continua
        


        private const int TASA_MUESTREO_POR_DEFECTO = 10000;
        private const int TIEMPO_MUESTREO_POR_DEFECTO = 10;

        public AdquisicionDatos(int[] canalInferior, int[] canalSuperior, int[] frecuenciaMuestro, int ventanaMuestro, bool almacenarHoraMedicion, String ruta, float divisorVoltaje, float vueltasTransformador, bool medicionContinua, int numeroTarjetas, Form1.TipoArchivo tipoArchivo)
        {
            //this.config = configuracion;
            
            this.canalInferior = canalInferior;
            this.canalSuperior = canalSuperior;
            this.tasaMuestreo = frecuenciaMuestro;
            this.ventanaMuestreo = ventanaMuestro;
            this.almacenarHoraMedicion = almacenarHoraMedicion;
            this.ruta = ruta;
            this.medicionContinua = medicionContinua;
            this.iniciado = false;
            this.divisorVoltaje = divisorVoltaje;
            this.vueltasTransformador = vueltasTransformador;

            this.numeroTarjetas = numeroTarjetas;
            this.almacenador = new AlmacenadorLecturas[numeroTarjetas];
            this.temporizador = new System.Timers.Timer[numeroTarjetas];
            this.cantidadActual = new int[numeroTarjetas];
            this.indiceAnterior = new int[numeroTarjetas];
            this.indiceActual = new int[numeroTarjetas];
            this.status = new short[numeroTarjetas];
            this.canalInicial = new int[numeroTarjetas];
            this.handleADC = new IntPtr[numeroTarjetas];
            this.tarjeta = new MccDaq.MccBoard[numeroTarjetas];
            this.tipoArchivo = tipoArchivo;

        }

        /**
         * Creamos un thread para adquirir los datos
         */
        public int iniciarAdquisicionDatos()
        {
            Thread hiloAdquisicion = new Thread(new ThreadStart(iniciarAdquisicionDatosFn));
           
            hiloAdquisicion.Start();

            return 0;
        }

        /*
         * Liberamos los buffers de los datos
         * */
        public void liberarMemoria()
        {
            for (int i = 0; i < handleADC.Length; i++)
            {
                if (handleADC[i] != IntPtr.Zero)
                {
                    MccDaq.MccService.WinBufFreeEx(handleADC[i]);
                }
            }
        }

        /**
         * Metodo para iniciar la adquisicion de datos
         */
        private void iniciarAdquisicionDatosFn()
        {

            //Inicilizamos la tarjeta DAQ
            for (int i = 0; i < this.numeroTarjetas; i++)
            {
                tarjeta[i] = new MccDaq.MccBoard(i);
            }

          
            //Rango +/- 10V, muestreo continuo y con datos convertidos a 12-bits, frequencia y canales
            this.rangoMuestreo = MccDaq.Range.Bip10Volts;

            //SI no hemos iniciado, iniciamos la adquisicion de datos.
            if (!iniciado)
            {
                if (!this.medicionContinua)
                {
                    //Medicion por ventana de tiempo.
                    opcionesADC = MccDaq.ScanOptions.BlockIo | MccDaq.ScanOptions.Background;

                    //con la frequencia y el tiempo de muestreo, podemos saber cuantas muestras necesitamos
                    //por 1 segundo. Este sera el tamaño del buffer
                    int[] tasaMuestreoLocal = new int[numeroTarjetas];
                    int[] numeroMuestras = new int[numeroTarjetas];
                    int[] numeroCanales = new int[numeroTarjetas]; 

                    //Separamos memoria para obtener los datos
                    for (int i = 0; i < numeroTarjetas; i++)
                    {
                        numeroCanales[i] = canalSuperior[i] - canalInferior[i] + 1;
                        numeroMuestras[i] = tasaMuestreo[i] * ventanaMuestreo * (canalSuperior[i] - canalInferior[i] + 1);     //Muestras totales
                        this.handleADC[i] = MccDaq.MccService.WinBufAllocEx(numeroMuestras[i]);

                        if (this.handleADC[i] == IntPtr.Zero)
                        {
                            for (int j = i; j >= 0; j--)
                            {
                                MccDaq.MccService.WinBufFreeEx(this.handleADC[j]);
                            }
                        }
                        //TODO: return
                    }

                    //Numero de peridos de medicion por dia
                    int periodosDia = 3600 * 24 / (ventanaMuestreo);               //Numero de periodos por dia basado en tamaño de ventana.

                    for (int i = 0; i < numeroTarjetas; i++)
                    {
                        this.almacenador[i] = new AlmacenadorLecturas(config, "tarjeta_" + i.ToString(), tipoArchivo);
                        this.almacenador[i].iniciarAlmacenamiento(i, tarjeta[i], this.rangoMuestreo, numeroCanales[i], numeroMuestras[i], this.tasaMuestreo[i], this.ruta, this.almacenarHoraMedicion, periodosDia, this.divisorVoltaje, this.vueltasTransformador, this.medicionContinua);
                        
                        //Timer de verificacion de status de medicion
                        temporizador[i] = new System.Timers.Timer(1);     //cada 1 ms
                        temporizador[i].Elapsed += new System.Timers.ElapsedEventHandler(temporizador_Elapsed);
                    }

                    this.iniciado = true;
                    this.fecha = DateTime.Now;
                    for (int i = 0; i < numeroTarjetas; i++)
                    {
                        tasaMuestreoLocal[i] = this.tasaMuestreo[i];
#if !DEBUG
                        MccDaq.ErrorInfo ulstat2 = tarjeta[i].AInScan(canalInferior[i], canalSuperior[i], numeroMuestras[i], ref tasaMuestreoLocal[i], rangoMuestreo, handleADC[i], opcionesADC);
#endif
                        temporizador[i].Start();
                    }
                }
                else
                {
                    //Medicion continua...
                    this.ventanaMuestreo = 60;
                    opcionesADC = MccDaq.ScanOptions.Background | MccDaq.ScanOptions.Continuous;   //MccDaq.ScanOptions.BlockIo |

                    //Creamos un buffer de 1 hora de mediciones.
                    int[] tasaMuestreoLocal = new int[numeroTarjetas];
                    int[] numeroMuestras = new int[numeroTarjetas];
                    int[] numeroCanales = new int[numeroTarjetas]; 

                    /*int tasaMuestreoLocal = this.tasaMuestreo;
                    int numeroMuestras = tasaMuestreo * ventanaMuestreo * (canalSuperior - canalInferior + 1);                                                                                                  //Se dividen entre el numero de caneles. Si muestre 4 canales
                    int numeroCanales = (canalSuperior - canalInferior + 1);*/

                    //Separamos memoria para obtener los datos
                    for (int i = 0; i < numeroTarjetas; i++)
                    {
                        tasaMuestreoLocal[i] = this.tasaMuestreo[i];
                        numeroCanales[i] = canalSuperior[i] - canalInferior[i] + 1;
                        numeroMuestras[i] = tasaMuestreo[i] * ventanaMuestreo * (canalSuperior[i] - canalInferior[i] + 1);     //Muestras totales
                        
                        this.handleADC[i] = MccDaq.MccService.WinBufAllocEx(numeroMuestras[i]);

                        if (this.handleADC[i] == IntPtr.Zero)
                        {
                            for (int j = i; j >= 0; j--)
                            {
                                MccDaq.MccService.WinBufFreeEx(this.handleADC[j]);
                            }
                        }

                        //TODO: return
                    }

                    //Numero de peridos de medicion por dia
                    int periodosDia = 3600 * 24 / (ventanaMuestreo);               //Numero de periodos por dia basado en tamaño de ventana.
                    for (int i = 0; i < numeroTarjetas; i++)
                    {
                        this.almacenador[i] = new AlmacenadorLecturas(config, "tarjeta_" + i.ToString(), tipoArchivo);
                        this.almacenador[i].iniciarAlmacenamiento(i, tarjeta[i], this.rangoMuestreo, numeroCanales[i], numeroMuestras[i], this.tasaMuestreo[i], this.ruta, this.almacenarHoraMedicion, periodosDia, this.divisorVoltaje, this.vueltasTransformador, this.medicionContinua);

                        //Timer de verificacion de status de medicion
                        temporizador[i] = new System.Timers.Timer(100);     //cada 10 ms
                        temporizador[i].Elapsed += new System.Timers.ElapsedEventHandler(temporizador_Elapsed);
                    }

                    this.iniciado = true;
                    this.fecha = DateTime.Now;
                    for (int i = 0; i < numeroTarjetas; i++)
                    {
#if !DEBUG
                        MccDaq.ErrorInfo ulstat2 = tarjeta[i].AInScan(canalInferior[i], canalSuperior[i], numeroMuestras[i], ref tasaMuestreoLocal[i], rangoMuestreo, this.handleADC[i], opcionesADC);
#endif
                        temporizador[i].Start();
                    }
                }     
            }           
        }

        //revisamos periodicamente el status de la medicon
        private void temporizador_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Timers.Timer temporizadorTmp = (System.Timers.Timer)sender;

            temporizadorTmp.Stop();

            //veamos de donde vino el evento..
            int i = 0;
            for (i = 0; i < temporizador.Length; i++)
            {
                if(temporizadorTmp.Equals(this.temporizador[i]))
                {
                    break;
                }
            }
            
            if (this.medicionContinua)
            {
                revisarStatusMedicionContinua(i);

                if (iniciado)
                {
                    temporizadorTmp.Start();
                }
            }
            else
            {
                revisarStatusMedicion(i);

                if (iniciado)
                {
                    temporizadorTmp.Start();
                }
            }
            
        }


        /*
         * Revisamos el status de una medicion continua y en segundo plano.
         */
        private void revisarStatusMedicionContinua(int id)
        {            

#if !DEBUG
            //Obtenemos el status...
            MccDaq.ErrorInfo stat = tarjeta[id].GetStatus(out status[id], out cantidadActual[id], out indiceActual[id], MccDaq.FunctionType.AiFunction);
#endif
            //MccDaq.ErrorInfo error2 = tarjeta[id].StopBackground(MccDaq.FunctionType.AiFunction);        //borrar
            DateTime fecha = DateTime.Now;

#if DEBUG
            Thread.Sleep(1000);
            short[] datosADC = new short[3000];

            for (short i = 0; i < datosADC.Length; i++)
            {
                datosADC[i] = (short)(1500 * (id+1));
            }

            this.almacenador[id].recibirLecturas(ref datosADC, DateTime.Now, 0);
#endif


#if !DEBUG
            if (!iniciado)
            {

                MccDaq.ErrorInfo error = tarjeta[id].StopBackground(MccDaq.FunctionType.AiFunction);

                //TODO: guardar mediciones faltantes..
                MccDaq.MccService.WinBufFreeEx(this.handleADC[id]);            //BUG... 
                temporizador[id].Stop();
                return;
            }

            //Console.Out.WriteLine("indiceActual: {0}, indiceAnterior: {1}", indiceActual[id], indiceAnterior[id]);
            //Revisamos mientras este tomando muestras
            if (status[id] == 1)
            {
                if (indiceActual[id] != -1)          //Si hubo transferencia de datos.
                {
                    if (indiceActual[id] > indiceAnterior[id])
                    {
                        //si el indice actual es MAYOR que la ultima vez que revisamos
                        //todavia no damos vuelta en el buffer, es decir, no se han
                        //sobre escrito las muestras
                        int numeroMuestrasObtenidas = indiceActual[id] - indiceAnterior[id];            //+1??
                        short[] datosADC = new short[numeroMuestrasObtenidas];

                        //MccDaq.ErrorInfo error = tarjeta[id].StopBackground(MccDaq.FunctionType.AiFunction);
                        //Pasamos los datos a un arreglo
                        MccDaq.ErrorInfo stat2 = MccDaq.MccService.WinBufToArray(handleADC[id], datosADC, indiceAnterior[id], numeroMuestrasObtenidas);

                        int numeroCanales = canalSuperior[id] - canalInferior[id] + 1;

                        
                        this.almacenador[id].recibirLecturas(ref datosADC, fecha, canalInicial[id]);

                        //Calculamos en que canal debemos almacenar la primera medición en la
                        //siguiente revisada        CHECK                      
                        canalInicial[id] = canalInicial[id] + (numeroMuestrasObtenidas % numeroCanales);

                        if (canalInicial[id] >= numeroCanales)
                        {
                            canalInicial[id] = canalInicial[id] % numeroCanales;
                        }

                        
                        //Pasamos el buffer para almacenamiento.
                        //this.almacenador.recibirLecturas
                        indiceAnterior[id] = indiceActual[id];
                        datosADC = null;
                       
                    }
                    else if (indiceActual[id] < indiceAnterior[id])
                    {
                        //System.Console.WriteLine("Se dio vuelta al buffer");

                        //en este caso, ya le dimos la vuelta al buffer
                        // el tamaño es igual al tamaño total - indice anterior (lo que sobre del buffer) + lo que avanzamos desde 0;
                        int tamañoBuffer = tasaMuestreo[id] * ventanaMuestreo * (canalSuperior[id] - canalInferior[id] + 1);
                        int numeroMuestrasObtenidas1 = tamañoBuffer - indiceAnterior[id]; 
                        int numeroMuestrasObtenidas2 = indiceActual[id];
                        int numeroCanales = (canalSuperior[id] - canalInferior[id] + 1);

                        short[] datosADC = new short[numeroMuestrasObtenidas1];
                        fecha = DateTime.Now;

                        //Pasamos los datos a un arreglo (la primera parte)
                        MccDaq.ErrorInfo stat2 = MccDaq.MccService.WinBufToArray(handleADC[id], datosADC, indiceAnterior[id], numeroMuestrasObtenidas1);

                        this.almacenador[id].recibirLecturas(ref datosADC, fecha, canalInicial[id]);

                        //
                        canalInicial[id] = canalInicial[id] + (numeroMuestrasObtenidas1 % numeroCanales);
                        if (canalInicial[id] >= numeroCanales)
                        {
                            canalInicial[id] = canalInicial[id] % numeroCanales;
                        }

                        //Pasamos la segunaa parte
                        datosADC = new short[numeroMuestrasObtenidas2];
                        stat2 = MccDaq.MccService.WinBufToArray(handleADC[id], datosADC, 0, numeroMuestrasObtenidas2);      //cuando damos la vuelta al buffer, empezamos de canal 0

                        float ticks = (1.0f / ((float)this.tasaMuestreo[id])) * 1000000000.0f / 100.0f;
                        int ticksInt = (int)ticks;                               //Se puede perder precisión si frecuencia no es even. 
                        fecha.Add(new TimeSpan(ticksInt));                      //Calculamos la fecha inicial de la primera medicion de este pedazo.

                        this.almacenador[id].recibirLecturas(ref datosADC, fecha, canalInicial[id]);

                        //Calculamos el njumero de canal a almacenar  para la siguiente revision.
                        canalInicial[id] = canalInicial[id] + (numeroMuestrasObtenidas1 % numeroCanales);
                        if (canalInicial[id] >= numeroCanales)
                        {
                            canalInicial[id] = canalInicial[id] % numeroCanales;
                        }

                        datosADC = null;
                        indiceAnterior[id] = indiceActual[id];

                    }
                }
            }
#endif
        }


        /*
         * Revisa si la medicion ya termino, y la reinicia si queremos continuar
         */
        private void revisarStatusMedicion(int id)
        {
            short status;
            int cuentaActual, indiceActual;

            //Si recibimos la orden de parar...
            if (iniciado == false)
            {

#if !DEBUG
                MccDaq.ErrorInfo error = tarjeta[id].StopBackground(MccDaq.FunctionType.AiFunction);
                
                //TODO: guardar mediciones faltantes..
                
                MccDaq.MccService.WinBufFreeEx(handleADC[id]);            //BUG

#endif
                temporizador[id].Stop();
                return;
            }
#if !DEBUG
            //Obtenemos el status de la medición.
            MccDaq.ErrorInfo error2 = tarjeta[id].GetStatus(out status, out cuentaActual, out indiceActual, MccDaq.FunctionType.AiFunction);

            //Si finalizó la recolección de datos.
            if (status == 0)
            {
                /*int tasaMuestrasLocal = this.tasaMuestreo;
                int numeroMuestras = tasaMuestrasLocal * ventanaMuestreo * (canalSuperior - canalInferior + 1);     //Muestras totales                                                                                             //Se dividen entre el numero de caneles. Si muestre 4 canales
                int numeroCanales = (canalSuperior - canalInferior + 1);*/
                
                //Fecha puede cambiar!!!!!!
                this.almacenador[id].recibirLecturas(this.handleADC[id], this.fecha);//, numeroMuestras, numeroCanales, this.rangoMuestreo, tarjetaDAS1000);

               //Finalizamos.
                //TODO: liberar memoria
            }
#endif
        }

        public void detenerMedicion()
        {
            this.iniciado = false;                              //primer finalizar alamcenamiento??? CHECK
            for (int i = 0; i < almacenador.Length; i++)
            {
                if (this.almacenador != null)
                {
                    if (this.almacenador[i] != null)
                    {
                        this.almacenador[i].finalizarAlmacenamiento();
                    }
                }
            }
        }

    }
}
