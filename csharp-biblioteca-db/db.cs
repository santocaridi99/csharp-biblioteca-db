using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    internal class db
    {
        private static string stringaDiConnessione =
"Data Source=localhost;Initial Catalog=biblioteca;Integrated Security=True;Pooling=False";

        //trona un oggetto sqlconnection conn che servità per la connessione
        private static SqlConnection Connect()
        {
                SqlConnection conn = new SqlConnection(stringaDiConnessione);
                try
                {
                    conn.Open();    
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                return conn;
            
        }

        //il libro inserisce sia in Doc
        internal static void libroAdd(Libro libro)
        {
            //collegarmi e inviare un comando di insert del nuovo scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            ////"insert into Scaffale (Scaffale) valuer('aaa')"
            //var cmd = String.Format("insert into Libro (Scaffale) values ('{0}')", nuovo);
            //using (SqlCommand insert = new SqlCommand(cmd, conn))
            //{
            //    try
            //    {
            //        var numrows = insert.ExecuteNonQuery();
            //        return numrows;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        return 0;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}
        }

        internal static int scaffaleAdd(string nuovo)
        {
            //collegarmi e inviare un comando di insert del nuovo scaffale
            var conn = Connect();
            if(conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            //"insert into Scaffale (Scaffale) valuer('aaa')"
            var cmd = String.Format("insert into SCAFFALE (Scaffale) values ('{0}')", nuovo);
            using (SqlCommand insert = new SqlCommand(cmd,conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    return numrows;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        internal static List<string> scaffaliGet()
        {
            List<string> ls= new List<string>();
            //collegarmi e inviare un comando di insert del nuovo scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            //"insert into Scaffale (Scaffale) valuer('aaa')"
            var cmd = String.Format("select Scaffale from SCAFFALE");
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
               using(SqlDataReader reader = select.ExecuteReader())
                {
                    //Console.WriteLine(reader.FieldCount);
                    while (reader.Read())
                    {
                        ls.Add(reader.GetString(0));
                    }
                }
            }
            conn.Close();
            return ls;    
        }
        //nel caso ci siano più attributi, allora potete utilizzare le tuple
        internal static List<Tuple<int, string, string, string, string, string>> documentiGet()
        {
            var ld = new List<Tuple<int, string, string, string, string, string>>();
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");
            var cmd = String.Format("select codice, Titolo, Settore, Stato, Tipo, Scaffale from Documenti");  //Li prendo tutti
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = new Tuple<Int32, string, string, string, string, string>(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5));
                        ld.Add(data);
                    }
                }
            }
            conn.Close();
            return ld;
        }
    }
}
