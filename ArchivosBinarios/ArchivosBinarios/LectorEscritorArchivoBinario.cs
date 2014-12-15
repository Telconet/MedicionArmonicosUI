using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ArchivosBinarios
{
    public class LectorEscritorArchivoBinario
    {
        private String ruta;
        private FileStream archivo;
        private FileMode modoArchivo;
        private long puntero;


        public LectorEscritorArchivoBinario(String rutaArchivo)
        {
            this.ruta = rutaArchivo;
            this.archivo = null;
            this.puntero = 0 ;
        }

        /*
         * Metodo para abrir archivo en modo de designado .
         * por la variable modo.
         */
        public bool abrirArchivo(FileMode modo, long tamano)
        {
            try{
                this.archivo = File.Open(ruta, modo);
                this.modoArchivo = modo;
                this.archivo.SetLength(tamano);
                

                return true;
            }
            catch(IOException e){
               
                this.archivo = null;
                return false;
            }
        }

        /**
         * Metodo para cerrar el archivo
         */
        public void cerrarArchivo()
        {
            try
            {
                if (this.archivo != null)
                {
                    this.archivo.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

        /**
         * Metodo que escribe un float de 4 bytes al archivo. Ya que la arquitectura x86 y x86-64
         * es LITTLE ENDIAN, el numero binario se guarda al reves (primero se almacenan los digitos
         * menos significativos). Al leerlo, debe ser convertido en BIG ENDIAN, para obtener el 
         * numero correcto.
         */
        public bool escribir(float valor)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    escritorBinario.Write(valor);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
         * Metodo que escribe un grupo de floats del arreglo valores al archivo.
         */
        public bool escribir(float[] valores)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    for(int i = 0; i < valores.Length; i++)
                    {
                        escritorBinario.Write(valores[i]);
                    }
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /**
         * Escribe un byte al stream.
         */
        public bool escribir(byte valor)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    escritorBinario.Write(valor);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
         * Metodo que escribe un grupo de bytes del arreglo valores al archivo.
         */
        public bool escribir(byte[] valores)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    for (int i = 0; i < valores.Length; i++)
                    {
                        escritorBinario.Write(valores[i]);
                    }
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /**
         * Escribe un entero de 4 bytes al stream entero al stream
         */
        public bool escribir(int valor)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    escritorBinario.Write(valor);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
         * Metodo que escribe un grupo de enteros de 4 bytes del arreglo valores al archivo.
         */
        public bool escribir(int[] valores)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    for (int i = 0; i < valores.Length; i++)
                    {
                        escritorBinario.Write(valores[i]);
                    }
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool escribir(short valor)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    escritorBinario.Write(valor);
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
         * Metodo que escribe un grupo de enteros de 4 bytes del arreglo valores al archivo.
         */
        public bool escribir(short[] valores)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);
                    for (int i = 0; i < valores.Length; i++)
                    {
                        escritorBinario.Write(valores[i]);
                    }
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /**
         * Metodo que escribe un string.
         * Codificacion del texto es UTF8. Primero se escribe la longitud del texto en formato UTF7, longitud variable.
         */
        public bool escribir(String texto)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryWriter escritorBinario = new BinaryWriter(this.archivo, Encoding.UTF8);                   
                    escritorBinario.Write(texto);
                    return true;
                }
                else return false;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        /**
         * Metodo que lee un float de 4 bytes al archivo.
         */
        public float leerFloat()
        {
            try
            {
                if (archivo != null)
                {
                    BinaryReader lectorBinario = new BinaryReader(this.archivo, Encoding.UTF8);
                    float valor = lectorBinario.ReadSingle();
                    
                    return valor;
                }
                else return 0;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return 0;
            }
        }

        /**
         * Metodo que lee un byte del archivo.
         */
        public byte leerByte()
        {
            try
            {
                if (archivo != null)
                {
                    BinaryReader lectorBinario = new BinaryReader(this.archivo, Encoding.UTF8);
                    byte valor = lectorBinario.ReadByte();

                    return valor;
                }
                else return 0;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return 0;
            }
        }

        /**
         * Metodo que lee un entero de 4 byte del archivo.
         */
        public int leerInt()
        {
            try
            {
                if (archivo != null)
                {
                    BinaryReader lectorBinario = new BinaryReader(this.archivo, Encoding.UTF8);
                    int valor = lectorBinario.ReadInt32();

                    return valor;
                }
                else return 0;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return 0;
            }
        }

        /**
         * Metodo que lee un entero de 2 bytes con signo del archivo.
         */
        public short leerShort()
        {
            try
            {
                if (archivo != null)
                {
                    BinaryReader lectorBinario = new BinaryReader(this.archivo, Encoding.UTF8);
                    short valor = lectorBinario.ReadInt16();

                    return valor;
                }
                else return 0;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return 0;
            }
        }


        /**
         * Metodo que lee un float de 4 bytes al archivo.
         */
        public String leerString(int longitud)
        {
            try
            {
                if (archivo != null)
                {
                    BinaryReader lectorBinario = new BinaryReader(this.archivo, Encoding.UTF8);
                    char[] caracteres = lectorBinario.ReadChars(longitud);
                    return new string(caracteres);
                }
                else return null;
            }
            catch (IOException e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }

        /**
         * Avanze el cursor dentro del archivo por n bytes
         */
        public void avanzarCursor(long n)
        {
            if (this.archivo != null)
            {
                long nuevaPosicion = this.archivo.Position + n;

                if (nuevaPosicion < this.archivo.Length)
                {
                    this.archivo.Position = nuevaPosicion;
                }
            }
        }

        public void Flush()
        {
            try
            {
                if (archivo != null)
                {
                    archivo.Flush();
                }
            }
            catch (IOException e)
            {
            }
        }

        
    }
}
