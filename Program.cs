using System;
using System.Data;
using System.Data.SqlClient;

namespace ConexionSQL
{
    class Program
    {
        static void Main(string[] args)
        {

            SqlConnectionStringBuilder SqlConnectionStringBuilder_ = new SqlConnectionStringBuilder();
            SqlConnectionStringBuilder_.DataSource = "D3SKTOP-F32C562\\S0LEXPRESS";
            SqlConnectionStringBuilder_.InitialCatalog = "BaseDatosProgramacion";
            SqlConnectionStringBuilder_.IntegratedSecurity = true; 
            SqlConnection SqlConnection_ = new SqlConnection(SqlConnectionStringBuilder_.ConnectionString);
            SqlConnection_.Open();
            bool Continuar = true;
            do {
                Console.WriteLine("Que deseas hacer? Opciones: \n1.) Teclee el numero 1, para insertar un nuevo elemento \n 2.)Presione 2 para listar todos los lenguajes \n 3.) presione 3 para editar un elemento \n 4.) presione 4 para eliminar un elemento");
                int opcion = int.Parse(Console.ReadLine());
                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("Has seleccionado insertar un nuevo elemento");
                        Console.WriteLine("\nPor favor ingrese el nombre del nuevo lenguaje de programacion a agregar");
                        try {
                            string NuevoLenguaje = Console.ReadLine();
                            SqlCommand SqlCommandAgregar = new SqlCommand("insert into  dbo.LenguajesProgramacion(Lenguaje) VALUES('" + NuevoLenguaje + "')", SqlConnection_);
                            SqlCommandAgregar.ExecuteNonQuery();
                            Console.WriteLine("El nuevo item fue agregado exitosamente.-");
                        } catch (Exception Error)
                        {
                            Console.WriteLine("Ocurrio un error:" + Error.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Has seleccionado listar todos los elementos");
                        LeerTodosElementos(SqlConnection_);
                        break;
                    case 3:
                        Console.WriteLine("Has seleccionado editar un elemento");                       
                        EjecutarAccion(SqlConnection_, "Editar");
                        break;
                    case 4:
                        Console.WriteLine("Has seleccionado eliminar un elemento");
                        EjecutarAccion(SqlConnection_, "Eliminar");
                        break;
                    default: break;
                }
                Console.WriteLine("Deseas continuar Ejecutando el programa? Presione S para continuar y cualquier otra tecla para salir");
                string Seleccion = Console.ReadLine();
                if (Seleccion != "S")
                {
                    Continuar = false;
                }
            } while (Continuar==true);



            SqlConnection_.Close();

        }

        private static void EjecutarAccion(SqlConnection SqlConnection_, string v)
        {
            LeerTodosElementos(SqlConnection_);
            Console.WriteLine("Para poder "+ v + " un elemento, escribe el Id del elemento a "+v+", y presiona enter.");
            int IdLenguaje = Convert.ToInt32(Console.ReadLine());
            LeerElemento(SqlConnection_, IdLenguaje);
            try
            {
                SqlCommand SqlCommandEdicion;
                if (v == "Eliminar")
                {
                     SqlCommandEdicion = new SqlCommand("Delete from LenguajesProgramacion  WHERE IdLenguaje='" + IdLenguaje + "'", SqlConnection_);
                }
                else { 
                    Console.WriteLine("Escriba el elemento nuevo a "+ v + ".-");
                    string EdicionElemento = Console.ReadLine();
                     SqlCommandEdicion = new SqlCommand("Update LenguajesProgramacion set Lenguaje='" + EdicionElemento + "' WHERE IdLenguaje='" + IdLenguaje + "'", SqlConnection_);
                }

                SqlCommandEdicion.ExecuteNonQuery();
                Console.WriteLine("Elemento exitosamente "+ v);
            }
            catch (Exception Error)
            {
                Console.WriteLine("Error ocurrio al "+ v + ":" + Error.Message);
            }
        }

        private static void LeerTodosElementos(SqlConnection SqlConnection_)
        {
            SqlCommand SqlCommand_ = new SqlCommand("Select * from dbo.LenguajesProgramacion", SqlConnection_);
            SqlDataReader SqlDataReader_ = SqlCommand_.ExecuteReader();
            Console.WriteLine("Id del lenguaje " + "/ Nombre del lenguaje");
            while (SqlDataReader_.Read())
            {
                Console.WriteLine(SqlDataReader_.GetInt32(0).ToString() + " / " + SqlDataReader_.GetString(1));
            }
            SqlDataReader_.Close();
        }

        private static void LeerElemento(SqlConnection SqlConnection_,int IdElemento)
        {
            try
            {
                SqlCommand SqlCommandLeerUnElemento = new SqlCommand("Select * from dbo.LenguajesProgramacion Where IdLenguaje=" + IdElemento.ToString(), SqlConnection_);
                SqlDataReader SqlDataReaderElementoEspecifico = SqlCommandLeerUnElemento.ExecuteReader();
                Console.WriteLine("Id del lenguaje " + "/ Nombre del lenguaje");
                while (SqlDataReaderElementoEspecifico.Read())
                {
                    Console.WriteLine(SqlDataReaderElementoEspecifico.GetInt32(0).ToString() + " / " + SqlDataReaderElementoEspecifico.GetString(1));
                }
                SqlDataReaderElementoEspecifico.Close();
            }catch(Exception Error)
            {
                Console.WriteLine("Ha ocurrido un error seleccionando el elemento:" + Error.Message);
            }

        }

    }
}
