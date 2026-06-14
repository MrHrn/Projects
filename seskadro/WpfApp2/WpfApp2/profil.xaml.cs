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
                var ownerWindow = Window.GetWindow(this);
                var win = new sifreyenile
                {
                    Owner = ownerWindow
                };

                // Eğer session.mail doluysa pencereye öntanımlı olarak yaz
                try
                {
                    if (!string.IsNullOrWhiteSpace(session.mail))
                        win.txtEmail.Text = session.mail;
                }
                catch { /* kontrolsüz erişim ihtimali varsa sessizce atla */ }

                // Kullanıcı rolünü combobox'ta seçmeye çalış
                try
                {
                    if (!string.IsNullOrWhiteSpace(session.kullanici_rol))
                    {
                        foreach (var item in win.kullanicitip.Items)
                        {
                            if (item is ComboBoxItem cbi && (cbi.Content?.ToString() ?? "").Equals(session.kullanici_rol, StringComparison.OrdinalIgnoreCase))
                            {
                                win.kullanicitip.SelectedItem = cbi;
                                break;
                            }
                            else if (item is string s && s.Equals(session.kullanici_rol, StringComparison.OrdinalIgnoreCase))
                            {
                                win.kullanicitip.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
                catch { /* güvenlik: eğer kontrol edilemezse atla */ }

                // Pencereyi aç
                win.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Şifre değiştirme penceresi açılamadı: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
