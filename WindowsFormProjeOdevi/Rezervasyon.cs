using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormProjeOdevi
{
    public class Rezervasyon
    {
        public int Id { get; set; }
        public Kullanici Kullanici { get; set; }
        public Saha Saha { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
        public decimal Ucret { get; set; }
    }
}
