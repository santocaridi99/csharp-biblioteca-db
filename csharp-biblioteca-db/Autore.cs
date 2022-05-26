using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    public class Autore : Persona
    {
        public string sMail;
        public int iCodiceAutore;
        public Autore(string Nome, string Cognome, string sPosta) : base(Nome, Cognome)
        {

            this.sMail = sPosta;
            this.iCodiceAutore = GeneraCodiceAutore();
        }
        public int GeneraCodiceAutore()
        {
            return 10000 + this.Nome.Length +this.Cognome.Length+this.sMail.Length;
        }
    }
}
