using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormProjeOdevi
{
    public enum Rol
    {
        Ogrenci,
        Personel,
        Halk
    }

    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Email { get; set; }
        public Rol Rol { get; set; }
    }
}
