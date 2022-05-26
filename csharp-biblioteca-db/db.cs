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

        internal static long GetUniqueId()
        {
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");
            string cmd = "UPDATE codiceunico SET codice = codice + 1 OUTPUT INSERTED.codice";
            long id;
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    reader.Read();
                    id = reader.GetInt64(0);
                }
            }
            conn.Close();
            return id;
        }


        internal static bool doSql(SqlConnection conn,string sql)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sql, conn))
            {
                try
                {
                   sqlCmd.ExecuteNonQuery();
                   return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    conn.Close();
                    return false;
                }
            }

        }

        //metodo : implementare una select che selezioni tutti i libri e gli autori
        //usando un join

        //internal static List<List<string>> getLibriAutori()
        //{
        //    var conn = Connect();
        //    if(conn == null)
        //    {
        //        throw new Exception("Unable to connect to the database");
        //    }
        //    var cmd = String.Format(@"sekect * from DOCUMENTI inner join Libro on DOCUMENTI.codice = libro.codice
        //    inner join AUTORI_DOCUMENTI on DOCUMENTI.codice = AUTORI_DOCUMENTI.codice_documento
        //    inner join AUTORI on AUTORI_DOCUMENTI.codice_autore = AUTORI.codice");


        //} 

        


        //il libro inserisce sia in Doc
        internal static int libroAdd(Libro libro, List<Autore> lAutori)
        {
            //devo collegarmi e inviare un comando di insert del nuovo scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new System.Exception("Unable to connect to database");
            }
            var ok = doSql(conn, "begin transaction \n");
            if (!ok)
                throw new System.Exception("Errore in begin transaction");


            var cmd = string.Format(@"insert into dbo.DOCUMENTI(codice, Titolo, Settore, Stato, Tipo, Scaffale)
      VALUES({0}, '{1}', '{2}', '{3}', 'LIBRO', '{4}')", libro.Codice,libro.Titolo,libro.Settore,libro.Stato.ToString(),libro.Scaffale.Numero);
            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        conn.Close();
                        throw new System.Exception("Valore di ritorno errato prima query");
                    }
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    doSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }
            }
            var cmd1 = string.Format(@"insert into dbo.Libro(Codice, Numero_pag) VALUES({0},{1})",libro.Codice,libro.NumeroPagine);
            using (SqlCommand insert = new SqlCommand(cmd1, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        doSql(conn, "rollback transaction");
                        conn.Close();
                        throw new System.Exception("Valore di ritorno errato seconda query");
                    }
                       
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    doSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }
            }
            string cmd2;
            foreach (Autore autore in lAutori)
            {
                cmd2 = string.Format(@"INSERT INTO AUTORI(Codice ,Nome, Cognome, mail) values({0},'{1}','{2}','{3}') ",autore.iCodiceAutore, autore.Nome, autore.Cognome, autore.sMail);
                using (SqlCommand insert = new SqlCommand(cmd2, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            doSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato quarta query");
                        }
                           
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        conn.Close();
                        return 0;
                    }
                }
            }
            string cmd3;
            foreach (Autore autore in lAutori)
            {
                cmd3 = string.Format(@"INSERT INTO AUTORI_DOCUMENTI(codice_autore, codice_documento) values('{0}','{1}')", autore.iCodiceAutore, libro.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1) {
                            doSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato quinta query");
                        }
                           
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        doSql(conn, "rollback transaction");
                        conn.Close();
                        return 0;
                    }
                }
            }
            doSql(conn, "commit transaction");
            conn.Close();
            return 0;
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
