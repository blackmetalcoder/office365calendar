using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365Calendar.Models
{
    class Handelse
    {
        public string Datum { get; set; }
        public string Start { get; set; }
        public string Slut { get; set; }
        public string Amne { get; set; }
        public string Langd { get; set; }
        public string Text { get; set; }
        public int Manad { get; set; }
        public int Dag { get; set; }
        public string Tzon { get; set; }
    }
}
