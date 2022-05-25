using System;
using System.Data.SqlClient;
namespace csharp_biblioteca_db
{
    internal class Program
    {
        static void Main(string[] args)
        {
//            string stringaDiConnessione =
//"Data Source=localhost;Initial Catalog=biblioteca;Integrated Security=True;Pooling=False";
//            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
//            {
//                conn.Open();
//            }







            Biblioteca b = new Biblioteca("Civica");



            //b.AggiungiScaffale("SS1");
            //b.AggiungiScaffale("SS2");
            //b.AggiungiScaffale("SS3");

            b.ScaffaleBiblioteca.ForEach(item => Console.WriteLine(item.Numero));

            //ed ora gli autori e i libri
            //il libro lo mettiamo in db direttamente dentro la new libro . gli autori con la Autori.add
            Autore a3 = new Autore("Nome 3", "Cognome 3");
            Libro l2 = new Libro("ISBN2", "Titolo 2", 2009, "Storia", 130, "s001");  //add libro e documento to db
            l2.Autori.Add(a3);
           
            Console.WriteLine("lista operazioni");
            Console.WriteLine("\t1: cerca libro per autore");
            Console.WriteLine("Cosa vuoi fare");
            string sAppo = Console.ReadLine();
            while(sAppo != "")
            {
                b.GestisciOperazioneBiblioteca(Convert.ToInt32(sAppo));
            }

            Libro l1 = new Libro("ISBN1", "Titolo 1", 2009, "Storia", 220,"scaffale");

            Autore a1 = new Autore("Nome 1", "Cognome 1");
            Autore a2 = new Autore("Nome 2", "Cognome 2");

            //Autori.Add(a1);
            //Autori.Add(a2);

            //Scaffale = s1;

            //b.Documenti.Add(l1);
           

           

          
            Autore a4 = new Autore("Nome 4", "Cognome 4");

          
            l2.Autori.Add(a4);

            //l2.Scaffale = s2;
            //b.Documenti.Add(l2);
           

            #region "DVD"
            DVD dvd1 = new DVD("Codice1", "Titolo 3", 2019, "Storia", 130,"Scaffale");

            dvd1.Autori.Add(a3);

            //dvd1.Scaffale = s3;
            //b.Documenti.Add(dvd1);
            #endregion

            Utente u1 = new Utente("Nome 1", "Cognome 1", "Telefono 1", "Email 1", "Password 1");

            //b.Utenti.Add(u1);

            Prestito p1 = new Prestito("P00001", new DateTime(2019, 1, 20), new DateTime(2019, 2, 20), u1, l1);
            Prestito p2 = new Prestito("P00002", new DateTime(2019, 3, 20), new DateTime(2019, 4, 20), u1, l2);

            //b.Prestiti.Add(p1);
            //b.Prestiti.Add(p2);

            Console.WriteLine("\n\nSearchByCodice: ISBN1\n\n");

            List<Documento> results = b.SearchByCodice("ISBN1");

            foreach (Documento doc in results)
            {
                Console.WriteLine(doc.ToString());

                if (doc.Autori.Count > 0)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Autori");
                    Console.WriteLine("--------------------------");
                    foreach (Autore a in doc.Autori)
                    {
                        Console.WriteLine(a.ToString());
                        Console.WriteLine("--------------------------");
                    }
                }
            }

            Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");

            List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");

            foreach (Prestito p in prestiti)
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("--------------------------");
            }

        }
    }

}