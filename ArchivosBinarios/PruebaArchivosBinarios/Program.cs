using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchivosBinarios;
using System.IO;

namespace PruebaArchivosBinarios
{
    class Program
    {
        static void Main(string[] args)
        {
            LectorEscritorArchivoBinario lecEsc = new LectorEscritorArchivoBinario("C:\\Users\\Eduardo\\Documents\\prueba.dat");

            //Abro archivo en modo Append, para escribirlo. Es creado si no existe.
            lecEsc.abrirArchivo(FileMode.Append);


            //Escribo un string..
            //Escribo un float...
            lecEsc.escribir(29.92493423948f);
            lecEsc.escribir(10139.32f);
            lecEsc.escribir(0.002342f);
            lecEsc.escribir((byte)4);
            lecEsc.escribir((short)15);

            float[] ed = { 34.234f, 4324.23f, 2342.43f };

            lecEsc.escribir(ed);

            lecEsc.cerrarArchivo();

            lecEsc.abrirArchivo(FileMode.Open);

            float valor = lecEsc.leerFloat();
            valor = lecEsc.leerFloat();
            valor = lecEsc.leerFloat();
            byte valor2 = lecEsc.leerByte();
            short valor3 = lecEsc.leerShort();

            valor = lecEsc.leerFloat();
            valor = lecEsc.leerFloat();
            valor = lecEsc.leerFloat();

            lecEsc.cerrarArchivo();
        }
    }
}
