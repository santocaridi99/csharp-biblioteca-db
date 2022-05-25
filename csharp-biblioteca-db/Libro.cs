using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    public class Libro : Documento
    {
        public int NumeroPagine { get; set; }

        public Libro(string Codice, string Titolo, int Anno, string Settore, int NumeroPagine ,string nomeScaffale) : base(Codice, Titolo, Anno, Settore,nomeScaffale)
        {
            this.NumeroPagine = NumeroPagine;
            //lo inserisco in db , insieme al documento!
            db.libroAdd(this);
        }

        public override string ToString()
        {
            return string.Format("{0}\nNumeroPagine:{1}",
                base.ToString(),
                this.NumeroPagine);
        }
    }
}
