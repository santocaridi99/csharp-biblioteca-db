﻿using System;
using System.Data.SqlClient;
using System.Windows;
using System.IO;

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
           

           







            //MessageBox.Show("Errore");

            Biblioteca b = new Biblioteca("Civica");



            //aggiunto al db  dei nomi cognomi e titoli di autore

            //List<Autore> lAutoriProva = new List<Autore>();

            //StreamReader reader = new StreamReader("elenco.txt");
            //string linea;
            //while ((linea = reader.ReadLine()) != null)
            //{
            //    //una linea è, ad esempio: giuseppe mazzini e altri autori:a carlo alberto di savoja
            //    var vett = linea.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            //    string s = vett[0];
            //    var cn = s.Split(new char[] { ' ' });
            //    string nome = cn[0];
            //    string cognome = "";
            //    try
            //    {
            //        cognome = s.Substring(cn[0].Length + 1);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    string titolo = vett[1];
            //    Console.WriteLine("Nome: {0}, Cognome: {1}, Titolo: {2}", nome, cognome, titolo);
            //    string email = nome + "@email.it";
            //    Autore AutoreMioLibro = new Autore(nome, cognome, email);
            //    lAutoriProva.Add(AutoreMioLibro);
            //    b.AggiungiLibro(db.GetUniqueId(), titolo,1800, "prova" ,200 , "SS1", lAutoriProva);
            //}
            //reader.Close();


            Console.WriteLine("Inserisci un opzione");
            Console.WriteLine("0-Aggiungi Autore  Libro");
            Console.WriteLine("1-Aggiungi Autore  dvd");
            Console.WriteLine("2-Stampa tutti i libri e autori");
            Console.WriteLine("3-Cerca per nome e cognome");




            //b.AggiungiScaffale("SS1");
            //b.AggiungiScaffale("SS2");
            //b.AggiungiScaffale("SS3");

            switch (Console.ReadLine())
            {
                case "0":
                    Console.WriteLine("Nuovo Libro");
                    Console.WriteLine("nome dell'autore");
                    string nomeAutore = Console.ReadLine();
                    Console.WriteLine("cognome autore");
                    string cognomeAutore = Console.ReadLine();
                    Console.WriteLine("mail");
                    string mailAutore = Console.ReadLine();

                    List<Autore> lAutoriLibro = new List<Autore>();
                    Autore AutoreMioLibro = new Autore(nomeAutore, cognomeAutore, mailAutore);
                    lAutoriLibro.Add(AutoreMioLibro);

                    Console.WriteLine("titolo del libro");
                    string titolo = Console.ReadLine();
                    Console.WriteLine("Inserisci anno pubblicazione del  libro");
                    int anno = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Inserisci la tipologia del libro");
                    string settore = Console.ReadLine();
                    Console.WriteLine("Inserisci il numero di pagine del libro");
                    int numeroPagine = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Scegli lo scaffale esempio: SS1,SS2,SS3");
                    string scaffale = Console.ReadLine();

                    b.AggiungiLibro(db.GetUniqueId(), titolo, anno, settore, numeroPagine, scaffale, lAutoriLibro);



                    break;
                case "1":
                    Console.WriteLine("Nuovo Libro");
                    Console.WriteLine("nome dell'autore");
                    string nomeAutore2 = Console.ReadLine();
                    Console.WriteLine("cognome autore");
                    string cognomeAutore2 = Console.ReadLine();
                    Console.WriteLine("mail");
                    string mailAutore2 = Console.ReadLine();

                    List<Autore> lAutoriDVD = new List<Autore>();
                    Autore AutoreMioDVD = new Autore(nomeAutore2, cognomeAutore2, mailAutore2);
                    lAutoriDVD.Add(AutoreMioDVD);

                    //inserimento DVD

                    Console.WriteLine("nome dvd");
                    string nomeDvd = Console.ReadLine();
                    Console.WriteLine("Inserisci anno pubblicazione del  DVD");
                    int annoDvd = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Inserisci la tipologia del DVD");
                    string settoreDvd = Console.ReadLine();
                    Console.WriteLine("Inserisci durata dvd");
                    int durata = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Scegli lo scaffale esempio: SS1,SS2,SS3");
                    string scaffaleDvd = Console.ReadLine();

                    b.AggiungiDvd(db.GetUniqueId(), nomeDvd, annoDvd, settoreDvd, durata, scaffaleDvd, lAutoriDVD);

                    break;


                case "2":
                    db.StampaLibriAutori();

                    break;
                case "3":
                    Console.WriteLine("Inserire Nome autore da cercare");
                    string nomeSearch = Console.ReadLine();
                    Console.WriteLine("Inserire Cognome autore da cercare");
                    string cognomeSearch = Console.ReadLine();

                    var listaSearch = new List<List<string>>();
                    listaSearch = db.SearchByAutore(nomeSearch, cognomeSearch);
                    db.StampaLibriAutoriRicerca(listaSearch);


                    break;

                default:
                    Console.WriteLine("Non è un opzione disponibile");
                    break;
            }


           


         



            b.ScaffaleBiblioteca.ForEach(item => Console.WriteLine(item.Numero));

            //ed ora gli autori e i libri
            //il libro lo mettiamo in db direttamente dentro la new libro . gli autori con la Autori.add
            //Autore a3 = new Autore("Nome 3", "Cognome 3","nome@gmail.com");
            //Libro l2 = new Libro(1245, "Titolo 2", 2009, "Storia", 130, "s001");  //add libro e documento to db
            //l2.Autori.Add(a3);
           
            //Console.WriteLine("lista operazioni");
            //Console.WriteLine("\t1: cerca libro per autore");
            //Console.WriteLine("Cosa vuoi fare");
            //string sAppo = Console.ReadLine();
            //while(sAppo != "")
            //{
            //    b.GestisciOperazioneBiblioteca(Convert.ToInt32(sAppo));
            //}

            //Libro l1 = new Libro("ISBN1", "Titolo 1", 2009, "Storia", 220,"scaffale");

            //Autore a1 = new Autore("Nome 1", "Cognome 1");
            //Autore a2 = new Autore("Nome 2", "Cognome 2");

            ////Autori.Add(a1);
            ////Autori.Add(a2);

            ////Scaffale = s1;

            ////b.Documenti.Add(l1);
           

           

          
            //Autore a4 = new Autore("Nome 4", "Cognome 4");

          
            //l2.Autori.Add(a4);

            ////l2.Scaffale = s2;
            ////b.Documenti.Add(l2);
           

            //#region "DVD"
            //DVD dvd1 = new DVD("Codice1", "Titolo 3", 2019, "Storia", 130,"Scaffale");

            //dvd1.Autori.Add(a3);

            ////dvd1.Scaffale = s3;
            ////b.Documenti.Add(dvd1);
            //#endregion

            //Utente u1 = new Utente("Nome 1", "Cognome 1", "Telefono 1", "Email 1", "Password 1");

            ////b.Utenti.Add(u1);

            //Prestito p1 = new Prestito("P00001", new DateTime(2019, 1, 20), new DateTime(2019, 2, 20), u1, l1);
            //Prestito p2 = new Prestito("P00002", new DateTime(2019, 3, 20), new DateTime(2019, 4, 20), u1, l2);

            ////b.Prestiti.Add(p1);
            ////b.Prestiti.Add(p2);

            //Console.WriteLine("\n\nSearchByCodice: ISBN1\n\n");

            //List<Documento> results = b.SearchByCodice("ISBN1");

            //foreach (Documento doc in results)
            //{
            //    Console.WriteLine(doc.ToString());

            //    if (doc.Autori.Count > 0)
            //    {
            //        Console.WriteLine("--------------------------");
            //        Console.WriteLine("Autori");
            //        Console.WriteLine("--------------------------");
            //        foreach (Autore a in doc.Autori)
            //        {
            //            Console.WriteLine(a.ToString());
            //            Console.WriteLine("--------------------------");
            //        }
            //    }
            //}

            //Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");

            //List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");

            //foreach (Prestito p in prestiti)
            //{
            //    Console.WriteLine(p.ToString());
            //    Console.WriteLine("--------------------------");
            //}

        }
    }

}