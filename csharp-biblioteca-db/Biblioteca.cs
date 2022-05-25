using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    public class Biblioteca
    {
        public string Nome { get; set; }
        public List<Scaffale> ScaffaleBiblioteca { get; set; }
        

  






    public Biblioteca(string Nome)
        {
            this.Nome = Nome;
            this.ScaffaleBiblioteca = new List<Scaffale>();   
        }


        public void AggiungiScaffale(string sNomeScaffale)
        {
            Scaffale s1 = new Scaffale(sNomeScaffale);
            ScaffaleBiblioteca.Add(s1);
        }

        public int GestisciOperazioneBiblioteca(int iCodiceOperazione)
        {
            List<Documento> lResult;
            string sAppo;
            switch (iCodiceOperazione)
            {
                case 1:
                    Console.WriteLine("Inserisci Autore");
                    sAppo = Console.ReadLine();
                    lResult = SearchByAutore(sAppo);
                    if(lResult == null)
                    {
                        return 1;
                    }
                    else
                    {
                        StampaListaDocumenti(lResult);
                    }
                    break;
            }
            return 0;
        }

        public void StampaListaDocumenti(List<Documento>lListaDoc)
        {
            return;
        }

        public List<Documento> SearchByAutore(string  autore)
        {
            Console.WriteLine("Codice da implementare");
            //Connettiti al db
            //lancia query
            //prendi risultati
            //stampa risultati

            //query SELECT titolo,settore,stato,tipo 
            //from DOCUMENTI ,AUTORI_DOCUMENTI
            //INNER JOIN(AURORI_DOCUMENTI on DOCUMENTI.codice_documento = AUTORI_DOCUMENTI.'inserire chiave'
            // where autori_documenti.codice_autore = 
            return null;
        }


        public List<Documento> SearchByCodice(string Codice)
        {
            Console.WriteLine("Codice da implementare");
            return null;
        }

        public List<Documento> SearchByTitolo(string Titolo)
        {
            Console.WriteLine("Codice da implementare");
            return null;
        }

        public List<Prestito> SearchPrestiti(string Numero)
        {
            Console.WriteLine("Codice da implementare");
            return null;
        }

        public List<Prestito> SearchPrestiti(string Nome, string Cognome)
        {
            Console.WriteLine("Codice da implementare");
            return null;
        }
    }
}
