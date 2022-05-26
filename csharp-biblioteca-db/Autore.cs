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
        public long iCodiceAutore;
        public Autore(string Nome, string Cognome, string sPosta) : base(Nome, Cognome)
        {

            this.sMail = sPosta;
            this.iCodiceAutore = GeneraCodiceAutore();
        }
        public long GeneraCodiceAutore()
        {
            return db.GetUniqueId();
        }
    }
}
