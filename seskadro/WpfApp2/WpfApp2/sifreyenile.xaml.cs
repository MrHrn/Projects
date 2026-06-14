using seskadrow;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// sifreyenile.xaml etkileşim mantığı
    /// </summary>
    public partial class sifreyenile : Window
    {
        public sifreyenile()
        {
            InitializeComponent();
        }

        private async void btnGonder_Click(object sender, RoutedEventArgs e)
        {
            btnGonder.IsEnabled = false; // Gönder butonunu devre dışı bırak
            if (await connection.ag_kontrolu() == false)
            {
                btnGonder.IsEnabled = true; // Ağ kontrolü başarısızsa butonu tekrar etkinleştir
                return;
            }
            if (kullanicitip.SelectedItem is ComboBoxItem secilenOge)
            {
                string secilenTip = secilenOge.Content.ToString();

                session.recoverpassword(txtEmail.Text, parola_islemleri.CreatePassword(10), kullanicitip.SelectedItem as ComboBoxItem);
            }
            else
            {
                MessageBox.Show("Lütfen bir kullanıcı tipi seçin!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.Close();
        }

        private void btnAnasayfa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = Application.Current.Windows.OfType<giris>().FirstOrDefault();
                if (login != null)
                {
                    // Zaten açıksa öne getir
                    login.Show();
                    login.Activate();
                }
                else
                {
                    // Açık değilse yeni oluştur
                    var g = new giris();
                    g.Show();
                }

                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Anasayfaya dönülemiyor: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
