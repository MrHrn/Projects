using Microsoft.Data.SqlClient;
using seskadrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// bilgiduzenle.xaml etkileşim mantığı
    /// </summary>
    public partial class bilgiduzenle : Window
    {
        public bilgiduzenle()
        {
            InitializeComponent();
        }
        string tur = session.kullanici_rol;
        string tabloadi = "";
        string ad = "";
        string soyad = "";
        string sosyalmedyaa = "";
        string mailkolon = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtAd.Text = session.ad;
            txtSoyad.Text = session.soyad;
            txtEmail.Text = session.mail;
            sosyalmedya.Text = session.sosyalMedya;
     

            // rol belirleme
            if (tur == "Seslendirme Sanatçısı")
            {
                tabloadi = "Sanatci_bilgileri";
                ad = "sanatci_adi";
                soyad = "sanatci_soyadi";
                sosyalmedyaa = "sanatci_sosyalmedya";
                mailkolon = "sanatci_mail";
            }

            else if (tur == "Kullanıcı")
            {
                tabloadi = "Kullanici_bilgileri";
                ad = "kullanici_ad";
                soyad = "kullanici_soyad";
                mailkolon = "kullanici_mail";
                sosyalmedyaa = "kullanici_sosyalmedya";
            }

            else if (tur == "Admin")
            {
                tabloadi = "Adminler";
                ad = "adminAdi";
                mailkolon = "adminmail";
                soyad = "adminSoyadi";
                sosyalmedyaa = "adminsosyal";
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir kullanıcı türü seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            try
            { 
                string sql = $"UPDATE {tabloadi} SET {ad}=@ad, {soyad}=@soyad, {sosyalmedyaa}=@sosyalmedya, {mailkolon} = @mail WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                cmd.Parameters.AddWithValue("@sosyalmedya", sosyalmedya.Text);
                cmd.Parameters.AddWithValue("@mail", txtEmail.Text);
                cmd.Parameters.AddWithValue("@id", session.id);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bilgileriniz başarıyla güncellendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                session.ad = txtAd.Text;
                session.soyad = txtSoyad.Text;
                session.sosyalMedya = sosyalmedya.Text;
                session.mail = txtEmail.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
