using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MedicionArmonicosUI
{
    class Configuracion
    {
        private String archivo;

        public enum Parametro
        { 
            ip_bd, 
            clave_bd, 
            nombre_bd, 
            usuario_bd, 
            freq_muestreo,
            canal_adc,
            tiempo_muestreo,
            divisor_voltaje,
            ruta_archivo,
            relacion_vueltas_trans,
            hora_medicion
        }

        public Configuracion(String archivo)
        {
            this.archivo = archivo;
        }

        /**
         * Obtener el parametro indicado por param,
         * del archivo de configuración.
         */
        public string obtenerParametro(Parametro param)
        {
            try
            {
                StreamReader lectorArchivo = new StreamReader(this.archivo);
                String linea;
                string parametro = null;
                string nombreParametro = null;

                if (Enum.IsDefined(typeof(Parametro), param))
                {
                    nombreParametro = Enum.GetName(typeof(Parametro), param);
                }
             

                while ((linea = lectorArchivo.ReadLine()) != null)
                {
                    if (linea.Contains(nombreParametro))
                    {
                        parametro = (linea.Split('='))[1].Trim();
                        return parametro;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
