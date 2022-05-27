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

            string cmdAutori;
            foreach (Autore autore in lAutori)
            {
                long lCodiceAutore = 0;
                int iInsertFlag = 0;
                cmdAutori = String.Format("select codice from Autori where Nome='{0}' and Cognome='{1}' and mail='{2}'", autore.Nome, autore.Cognome, autore.sMail);
                using (SqlCommand select = new SqlCommand(cmdAutori, conn))
                {
                    using (SqlDataReader reader = select.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                        }
                        else
                        {
                            lCodiceAutore = autore.iCodiceAutore;
                            iInsertFlag = 1;

                        }
                        reader.Close();

                    }

                }
                if(iInsertFlag == 1)
                {
                    string cmd4 = String.Format("INSERT INTO AUTORI (codice,Nome,Cognome,mail) values({0},'{1}','{2}','{3}')",lCodiceAutore, autore.Nome, autore.Cognome, autore.sMail); ;
                    using (SqlCommand insert = new SqlCommand(cmd4, conn))
                    {
                        try
                        {
                            var numrows = insert.ExecuteNonQuery();
                            if (numrows != 1)
                            {
                                //doSql(conn, "rollback transaction");
                                //conn.Close();
                                //throw new System.Exception("Valore di ritorno errato seconda query");
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex.Message);
                            //doSql(conn, "rollback transaction");
                            //conn.Close();
                            //return 0;
                        }
                    }

                }
                string cmd3 = string.Format(@"INSERT INTO AUTORI_DOCUMENTI(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, libro.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            //doSql(conn, "rollback transaction");
                            //conn.Close();
                            //throw new System.Exception("Valore di ritorno errato seconda query");
                        }
                    }
                    catch (Exception ex)
                    {
                    //    Console.WriteLine(ex.Message);
                    //    doSql(conn, "rollback transaction");
                    //    conn.Close();
                        //return 0;
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

        internal static int dvdAdd(DVD dvd, List<Autore> lAutori)
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
      VALUES({0}, '{1}', '{2}', '{3}', 'DVD', '{4}')", dvd.Codice, dvd.Titolo, dvd.Settore, dvd.Stato.ToString(), dvd.Scaffale.Numero);
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
            var cmd1 = string.Format(@"insert into dbo.DVD(codice, Durata) VALUES({0},{1})", dvd.Codice, dvd.Durata);
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

            string cmdAutori;
            foreach (Autore autore in lAutori)
            {
                long lCodiceAutore = 0;
                int iInsertFlag = 0;
                cmdAutori = String.Format("select codice from Autori where Nome='{0}' and Cognome='{1}' and mail='{2}'", autore.Nome, autore.Cognome, autore.sMail);
                using (SqlCommand select = new SqlCommand(cmdAutori, conn))
                {
                    using (SqlDataReader reader = select.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                        }
                        else
                        {
                            lCodiceAutore = autore.iCodiceAutore;
                            iInsertFlag = 1;

                        }
                        reader.Close();

                    }

                }
                if (iInsertFlag == 1)
                {
                    string cmd4 = String.Format("INSERT INTO AUTORI (codice,Nome,Cognome,mail) values({0},'{1}','{2}','{3}')", lCodiceAutore, autore.Nome, autore.Cognome, autore.sMail);
                    using (SqlCommand insert = new SqlCommand(cmd4, conn))
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

                }
                string cmd3 = string.Format(@"INSERT INTO AUTORI_DOCUMENTI(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, dvd.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
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
            }

            doSql(conn, "commit transaction");
            conn.Close();
            return 0;
        }


        internal static List<List<string>> SearchByAutore(string nome, string cognome)
        {
            var data = new List<List<string>>();
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");

            var cmd = String.Format(@"select * from Libro inner join DOCUMENTI on Libro.codice = DOCUMENTI.codice inner join AUTORI_DOCUMENTI 
                                     on DOCUMENTI.codice = AUTORI_DOCUMENTI.codice_documento inner join AUTORI 
                                        on AUTORI_DOCUMENTI.codice_autore = Autori.codice
                                        where Autori.Nome = '{0}' and Autori.Cognome = '{1}'", nome, cognome);
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var ls = new List<string>();

                        ls.Add(reader.GetInt64(0).ToString());
                        ls.Add(reader.GetInt32(1).ToString());
                        ls.Add(reader.GetInt64(2).ToString());
                        ls.Add(reader.GetString(3));
                        ls.Add(reader.GetString(4));
                        ls.Add(reader.GetString(5));
                        ls.Add(reader.GetString(6));
                        ls.Add(reader.GetString(7));
                        ls.Add(reader.GetInt64(8).ToString());
                        ls.Add(reader.GetInt64(9).ToString());
                        ls.Add(reader.GetInt64(10).ToString());
                        ls.Add(reader.GetString(11));
                        ls.Add(reader.GetString(12));
                        ls.Add(reader.GetString(13));



                        data.Add(ls);
                    }


                }


            }
            conn.Close();
            return data;
        }

        //metodo per prendere tutti i libri e autori
        internal static List<List<string>> getLibriAutori()
        {
            var data = new List<List<string>>();

            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");

            var cmd = String.Format(@"select * from Libro inner join DOCUMENTI on Libro.codice = DOCUMENTI.codice inner join AUTORI_DOCUMENTI 
                                     on DOCUMENTI.codice = AUTORI_DOCUMENTI.codice_documento inner join AUTORI 
                                        on AUTORI_DOCUMENTI.codice_autore = AUTORI.codice");


            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())

                {


                    while (reader.Read())
                    {

                        var ls = new List<string>();

                        ls.Add(reader.GetInt64(0).ToString());
                        ls.Add(reader.GetInt32(1).ToString());
                        ls.Add(reader.GetInt64(2).ToString());
                        ls.Add(reader.GetString(3));
                        ls.Add(reader.GetString(4));
                        ls.Add(reader.GetString(5));
                        ls.Add(reader.GetString(6));
                        ls.Add(reader.GetString(7));
                        ls.Add(reader.GetInt64(8).ToString());
                        ls.Add(reader.GetInt64(9).ToString());
                        ls.Add(reader.GetInt64(10).ToString());
                        ls.Add(reader.GetString(11));
                        ls.Add(reader.GetString(12));
                        ls.Add(reader.GetString(13));



                        data.Add(ls);

                    }

                }
            }

            conn.Close();


            return data;

        }


        internal static void StampaLibriAutori()
        {

            var dati = new List<List<string>>();
            dati = getLibriAutori();

            foreach (var item in dati)
            {

                Console.WriteLine(string.Format(@"Codice Libro: {0},Numero Pagine: {1},Titolo: {2},Settore: {3}, 
                        Stato:{4}, Scaffale {5}, Codice Autore {6}, Nome Autore {7}, Cognome Autore {8}, Mail Autore {9} ",
                    item[0], item[1], item[3], item[4], item[5], item[7], item[8], item[11], item[12], item[13]));


            }

        }
        internal static void StampaLibriAutoriRicerca(List<List<string>> list)
        {

            if (list.Count == 0)
            {
                Console.WriteLine("Nessun Elemento presente");
            }

            foreach (var item in list)
            {

                Console.WriteLine(string.Format(@"Codice Libro: {0},Numero Pagine: {1},Titolo: {2}, Settore: {3}, 
                        Stato:{4}, Scaffale {5}, Codice Autore {6}, Nome Autore {7}, Cognome Autore {8}, Mail Autore {9} ",
                    item[0], item[1], item[3], item[4], item[5], item[7], item[8], item[11], item[12], item[13]));


            }

        }



    }

}
