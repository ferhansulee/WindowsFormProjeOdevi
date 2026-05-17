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
    public partial class LoginForm : Form
    {
        SporMerkezi sistem = new SporMerkezi();

        public LoginForm()
        {
            InitializeComponent();

            cmbRol.DataSource = Enum.GetValues(typeof(Rol));
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text))
            {
                MessageBox.Show("Ad boş olamaz!");
                return;
            }

            if (!sistem.EmailKontrol(txtEmail.Text))
            {
                MessageBox.Show("Geçersiz email!");
                return;
            }

            Kullanici kullanici = new Kullanici
            {
                Ad = txtAd.Text,
                Email = txtEmail.Text,
                Rol = (Rol)cmbRol.SelectedItem
            };

            MainForm form = new MainForm(kullanici, sistem);

            form.Show();

            this.Hide();
        }

        
        
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "E-mail giriniz...";
                txtEmail.ForeColor = Color.Gray;
            }
        }
        private void lblAd_Click(object sender, EventArgs e)
        {

        }

        private void lblEmail_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
      
    }
}
