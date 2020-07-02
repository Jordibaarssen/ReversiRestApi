using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public class SpelDTO
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public ICollection<Speler> Spelers { get; set; }
        public string Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        public string Status { get; set; }
    }
}
