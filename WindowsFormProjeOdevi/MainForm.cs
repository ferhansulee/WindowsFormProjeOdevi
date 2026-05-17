using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormProjeOdevi
{
    public partial class MainForm : Form
    {
        SporMerkezi sistem;

        Kullanici aktifKullanici;

        public MainForm(Kullanici kullanici, SporMerkezi sistem)
        {
            InitializeComponent();

            this.sistem = sistem;
            aktifKullanici = kullanici;

            sistem.DosyadanYukle();

            lblHosgeldin.Text =
                $"HOŞGELDİNİZ  {aktifKullanici.Ad} ";

            cmbSaha.DataSource = sistem.Sahalar;
            cmbSaha.DisplayMember = "Ad";
            cmbSaha.ValueMember = "Id";
            dtTarih.MinDate = DateTime.Today;
            cmbSaat.DataSource = sistem.SaatleriGetir(aktifKullanici.Rol);

            GridDoldur();

            GridAyarla();
        }

        void GridAyarla()
        {
            gridRezervasyonlar.BackgroundColor = Color.White;
            gridRezervasyonlar.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            gridRezervasyonlar.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            gridRezervasyonlar.MultiSelect = false;

            gridRezervasyonlar.RowHeadersVisible = false;

            gridRezervasyonlar.EnableHeadersVisualStyles = false;

            gridRezervasyonlar.ColumnHeadersDefaultCellStyle.BackColor =
                Color.DarkBlue;

            gridRezervasyonlar.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            gridRezervasyonlar.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
        }

        void GridDoldur()
        {
            gridRezervasyonlar.DataSource = null;

            gridRezervasyonlar.DataSource = sistem.Rezervasyonlar
                .Select(r => new
                {
                    r.Id,
                    Kullanici = r.Kullanici.Ad,
                    Saha = r.Saha.Ad,
                    Tarih = r.RezervasyonTarihi
                })
                .ToList();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            int sahaId = (int)cmbSaha.SelectedValue;
            DateTime tarih = dtTarih.Value.Date;

            string saatText = cmbSaat.Text;

            var saha = sistem.Sahalar.FirstOrDefault(x => x.Id == sahaId);

            decimal ucret = sistem.UcretHesapla(saha, aktifKullanici);

            string sonuc = sistem.RezervasyonEkle(
                saha,
                aktifKullanici,
                tarih,
                saatText
                );

           

            if (sonuc == "Rezervasyon oluşturuldu!")
            {
                if (aktifKullanici.Rol == Rol.Ogrenci ||
                    aktifKullanici.Rol == Rol.Personel)
                {
                    MessageBox.Show(
                        sonuc +
                        "\nÖğrenci/Personel kartınızı getirmeyi unutmayınız!"
                    );
                }
                else
                {
                    MessageBox.Show(sonuc);
                }
            }
            else
            {
                MessageBox.Show(sonuc);
            }


            GridDoldur();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (gridRezervasyonlar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Rezervasyon seçin!");
                return;
            }
            int id = (int)gridRezervasyonlar.SelectedRows[0].Cells["Id"].Value;

            string sonuc = sistem.RezervasyonSil(id, aktifKullanici);

            MessageBox.Show(sonuc);

            GridDoldur();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (gridRezervasyonlar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seçim yap!");
                return;
            }

            int id = (int)gridRezervasyonlar.SelectedRows[0].Cells["Id"].Value;

            DateTime yeniTarih = dtTarih.Value.Date;
            string yeniSaat = cmbSaat.Text;

            string sonuc = sistem.RezervasyonGuncelle(
                id,
                aktifKullanici.Email,
                yeniTarih,
                yeniSaat
            );

            MessageBox.Show(sonuc);

            GridDoldur();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbSaha.DataSource = sistem.Sahalar;

            cmbSaha.Format += cmbSaha_Format;
        }
        private void cmbSaha_Format(object sender, ListControlConvertEventArgs e)
        {
            var saha = (Saha)e.ListItem;

            if (aktifKullanici.Rol == Rol.Halk)
            {
                e.Value = $"{saha.Ad} - {saha.SaatlikUcret} TL";
            }
            else
            {
                e.Value = saha.Ad;
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();

            login.Show();

            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtTarih_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblHosgeldin_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
