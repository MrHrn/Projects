using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// profil.xaml etkileşim mantığı
    /// </summary>
    public partial class profil : Page
    {
        public profil()
        {
            InitializeComponent();

            // İlk doldurma
            DoldurFromSession();

            // Eğer session çalışma sırasında değişiyorsa Loaded'da tekrar güncelle
            this.Loaded += Profil_Loaded;
        }

        private void Profil_Loaded(object? sender, RoutedEventArgs e)
        {
            DoldurFromSession();
        }

        private void DoldurFromSession()
        {
            // session alanlarının null olma ihtimaline karşı güvenli atama
            txtIsim.Text = session.ad ?? "-";
            txtSoyIsim.Text = session.soyad ?? "-";
            txtMail.Text = session.mail ?? "-";

            // Rol kontrolü: sadece "Kullanıcı" rolündeyse katkı puanı ve rütbe görünür
            var rol = session.kullanici_rol ?? string.Empty;
            bool isKullanici = rol.Equals("Kullanıcı", StringComparison.OrdinalIgnoreCase);

            if (isKullanici)
            {
                txtKatkiLabel.Visibility = Visibility.Visible;
                txtKatki.Visibility = Visibility.Visible;
                rankHeader.Visibility = Visibility.Visible;
                label4.Visibility = Visibility.Visible;

                txtKatki.Text = string.IsNullOrWhiteSpace(session.kullanicikatki_puani) ? "-" : session.kullanicikatki_puani;
                label4.Text = string.IsNullOrWhiteSpace(session.kullanici_rutbe) ? "-" : session.kullanici_rutbe;
            }
            else
            {
                txtKatkiLabel.Visibility = Visibility.Collapsed;
                txtKatki.Visibility = Visibility.Collapsed;
                rankHeader.Visibility = Visibility.Collapsed;
                label4.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAnasayfa_Click(object sender, RoutedEventArgs e)
        {
            // Ana sayfaya dön: ana penceredeki merkezi metodu kullan
            var host = Window.GetWindow(this) as MainWindow;
            if (host != null)
            {
                host.ShowMenuAndHideFrame();
                return;
            }

            // Fallback: NavigationService ile geri gitmeyi dene
            if (this.NavigationService != null && this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            // Aynı mantık gerekiyorsa kullanılabilir
            var host = Window.GetWindow(this) as MainWindow;
            host?.ShowMenuAndHideFrame();
        }

        // Yeni: şifre değiştirme butonu — sifreyenile penceresini açar ve e-postayı doldurmayı dener
        private void btnSifreDegistir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var host = Window.GetWindow(this) as MainWindow;
                if (host == null)
                {
                    MessageBox.Show("Ana pencere bulunamadı.", "Hata");
                    return;
                }

                // Menü görünürlüğünü opsiyonel olarak yönet (istediğin gibi)
                var menu = host.GetMenuGrid();
                if (menu != null)
                    menu.Visibility = Visibility.Collapsed;   // veya Visible tut

                // Window olarak aç
                var sifrePencere = new sifredeg();
                sifrePencere.ShowDialog();           // Normal açılır (arkada diğer pencereler çalışır)
                                               // sifrePencere.ShowDialog();  // Modal açmak istersen bunu kullan (önceki pencere kilitlenir)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Şifre değiştirme penceresi açılamadı:\n{ex.Message}",
                                "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
