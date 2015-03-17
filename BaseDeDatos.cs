/**
 * 
 * @author Eduardo Murillo
 * © Telconet 2015
 * 
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MySql.Data.Types;
//using MySql.Data.MySqlClient;


namespace MedicionArmonicosUI
{
    class BaseDeDatos
    {
        //private MySqlConnection conexion;
        private string host;
        private string BaseDatos;
        private string usuario;
        private string contrasena;

        public BaseDeDatos(String host, String BaseDatos, String usuario, String contrasena)
        {
            this.BaseDatos = BaseDatos;
            this.host = host;
            this.usuario = usuario;
            this.contrasena = contrasena;
            
        }

        /**
         * Metodo para conectarnos a la base de datos. Usa los parametros dados 
         * en la creación del objeto.
         */
        public void conectar()
        {
            try
            {
                string strProveedor = "SERVER=" + this.host + ";DATABASE=" + this.BaseDatos + ";UID=" + this.usuario + ";PASSWORD=" + this.contrasena;
                //this.conexion = new MySqlConnection(strProveedor);
                //this.conexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /**
         * Metodo que indica si hay una conexion activa a la base de datos
         */
        public bool estaConectado()
        {
            return false;
            //return conexion.Ping();
        }

        /**
         * Metodo que cierra la conexion a la base de datos.
         */
        public void cerrar()
        {
            /*
            if (this.conexion != null)
            {
                this.conexion.Close();
            }*/
        }

        /*public MySqlDataReader ejecutarConsulta(String consulta)
        {
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, this.conexion);
                MySqlDataReader datos = comando.ExecuteReader(); 

                return datos;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                return null;
            }
        }

        public int ejecutarNoConsulta(string consulta)
        {
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, this.conexion);
                int res = comando.ExecuteNonQuery();
                
                return res;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                return -1;
            }
        }*/


        /**
         * Metodo que verifica si existe la tabla especificada.
         */
        /*public bool existeTabla(string nombreTabla)
        {
            try
            {
                //System.Data.DataTable dbs= this.conexion.GetSchema();

                MySqlDataReader dr = ejecutarConsulta("show tables");

                while (dr.Read())
                {
                    string tabla = dr.GetString("Tables_in_" + this.BaseDatos);
                    if (tabla.Equals(nombreTabla))
                    {
                        dr.Close();
                        return true;
                    }

                }

                dr.Close();
                return false;
            }
            catch (Exception e)
            {

                Console.Out.WriteLine(e.Message);
                return false;
            }
            finally
            {
              //cerrar dr???
            }
        }*/
    }
}
