using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormProjeOdevi
{
    public class Saha
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int SaatlikUcret { get; set; }

        public virtual string SahaBilgisi()
        {
            return $"{Ad} - {SaatlikUcret} TL";
        }
        public override string ToString()
        {
            return Ad;
        }
    }

    public class FutbolSahasi : Saha
    {
        public override string SahaBilgisi()
        {
            return base.SahaBilgisi() + " | Futbol";
        }
    }

    public class BasketbolSahasi : Saha
    {
        public override string SahaBilgisi()
        {
            return base.SahaBilgisi() + " | Basketbol";
        }
    }

    public class VoleybolSahasi : Saha
    {
        public override string SahaBilgisi()
        {
            return base.SahaBilgisi() + " | Voleybol";
        }
    }

    public class TenisKortu : Saha
    {
        public override string SahaBilgisi()
        {
            return base.SahaBilgisi() + " | Tenis";
        }
    }
}
