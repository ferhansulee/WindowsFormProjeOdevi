using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace WindowsFormProjeOdevi
{
    public class SporMerkezi
    {
        public List<Saha> Sahalar { get; set; } = new List<Saha>();
        public List<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
        public List<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();

        public SporMerkezi()
        {
            Sahalar = new List<Saha>
            {
                new FutbolSahasi { Id = 1, Ad = "Futbol Sahası 1", SaatlikUcret = 500 },
                new FutbolSahasi { Id = 2, Ad = "Futbol Sahası 2", SaatlikUcret = 500 },
                new BasketbolSahasi { Id = 3, Ad = "Basketbol Sahası 1", SaatlikUcret = 300 },
                new BasketbolSahasi { Id = 4, Ad = "Basketbol Sahası 1", SaatlikUcret = 300 },
                new VoleybolSahasi { Id = 5, Ad = "Voleybol Sahası 1", SaatlikUcret = 250 },
                new VoleybolSahasi { Id = 6, Ad = "Voleybol Sahası 1", SaatlikUcret = 250 },
                new TenisKortu { Id = 7, Ad = "Tenis Kortu 1", SaatlikUcret = 600 },
                new TenisKortu { Id = 8, Ad = "Tenis Kortu 1", SaatlikUcret = 600 }
            };
        }



        public string RezervasyonEkle(
     Saha saha,
     Kullanici kullanici,
     DateTime tarih,
     string saat)
        {
            if (saha == null || kullanici == null)
                return "Saha veya kullanıcı boş olamaz!";

            if (!TimeSpan.TryParse(saat, out TimeSpan ts))
                return "Saat formatı hatalı! (örn: 14:30)";

            DateTime yeniRezervasyon = tarih.Date + ts;

            bool cakisma = Rezervasyonlar.Any(r =>
                r.Saha.Id == saha.Id &&
                r.RezervasyonTarihi == yeniRezervasyon);

            if (cakisma)
                return "Bu saat dolu!";

            int yeniId = Rezervasyonlar.Count > 0
                ? Rezervasyonlar.Max(x => x.Id) + 1
                : 1;

            Rezervasyon yeni = new Rezervasyon
            {
                Id = yeniId,
                Saha = saha,
                Kullanici = kullanici,
                RezervasyonTarihi = yeniRezervasyon
            };

            Rezervasyonlar.Add(yeni);

            DosyayaKaydet();

            return "Rezervasyon oluşturuldu!";
        }
        public string RezervasyonSil(int rezervasyonId, Kullanici aktifKullanici)
        {
            var rez = Rezervasyonlar.FirstOrDefault(x => x.Id == rezervasyonId);

            if (rez == null)
                return "Rezervasyon bulunamadı";

            
            if (rez.Kullanici.Email != aktifKullanici.Email)
                return "Sadece kendi rezervasyonunu silebilirsin";

            Rezervasyonlar.Remove(rez);

            DosyayaKaydet();

            return "Rezervasyon silindi";
        }

        public string RezervasyonGuncelle(
            int id,
            string email,
            DateTime yeniTarih,
            string yeniSaat)
        {
            var rez = Rezervasyonlar.FirstOrDefault(r =>
                r.Id == id &&
                r.Kullanici.Email == email);

            if (rez == null)
                return "Sadece kendi rezervasyonunuzu güncelleyebilirsiniz!";

            TimeSpan ts = TimeSpan.Parse(yeniSaat);

            DateTime yeniRez = yeniTarih.Date + ts;

            bool cakisma = Rezervasyonlar.Any(r =>
                r.Saha.Id == rez.Saha.Id &&
                r.RezervasyonTarihi == yeniRez &&
                r.Id != id);

            if (cakisma)
                return "Bu saat dolu!";

            rez.RezervasyonTarihi = yeniRez;

            DosyayaKaydet();

            return "Rezervasyon güncellendi!";
        }

        public decimal UcretHesapla(Saha saha, Kullanici kullanici)
        {
            if (kullanici.Rol == Rol.Halk)
                return saha.SaatlikUcret;

            
            return 0;
        }

        public List<string> SaatleriGetir(Rol rol)
        {
            List<string> saatler = new List<string>();

            int baslangic;
            int bitis;

            if (rol == Rol.Ogrenci || rol == Rol.Personel)
            {
                baslangic = 7;
                bitis = 17;
            }
            else 
            {
                baslangic = 17;
                bitis = 24;
            }

            for (int i = baslangic; i < bitis; i+=2)
            {
                saatler.Add(i.ToString("D2") + ":00");
                
            }

            return saatler;
        }

        public void DosyayaKaydet()
        {
            string json = JsonConvert.SerializeObject(Rezervasyonlar, Formatting.Indented);
            File.WriteAllText("rezervasyonlar.json", json);
        }

        public void DosyadanYukle()
        {
            if (File.Exists("rezervasyonlar.json"))
            {
                string json = File.ReadAllText("rezervasyonlar.json");

                Rezervasyonlar = JsonConvert.DeserializeObject<List<Rezervasyon>>(json)
                    ?? new List<Rezervasyon>();
            }
        }

        public bool EmailKontrol(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }
    }
}
