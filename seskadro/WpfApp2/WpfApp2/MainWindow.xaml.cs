using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }
        public Frame GetMainFrame() => FindName("MainFrame") as Frame;
        public Grid GetMenuGrid() => FindName("MenuGrid") as Grid;

        // Merkezi geri/menü gösterme metodu
        public void ShowMenuAndHideFrame()
        {
            var frame = GetMainFrame();
            var menu = GetMenuGrid();

            if (frame != null)
            {
                // Gezinme geçmişini saklamak istemezseniz içeriği temizleyin
                frame.Content = null;
                frame.Visibility = Visibility.Collapsed;
            }

            if (menu != null)
            {
                menu.Visibility = Visibility.Visible;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            adsoyad.Text = session.ad + " " +  session.soyad;
            session.butonlariguncelle(yapimekle, kadrolistele, karakterekleme, yapimtur);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?",
                                                 "Çıkış Onayı",
                                                 MessageBoxButton.YesNo);
            if (m == MessageBoxResult.Yes)
            {
                session.ad = "";
                session.soyad = "";
                session.oturumdurumu = false;
                session.kullanici_rutbe = "";
                session.kullanicikatki_puani = "";
                session.kullanici_rol = "";
                session.mail = "";
                session.id = -1;
                giris g = new giris();
                this.Close();
                g.Show();
            }
            else
            {
                MessageBox.Show("Çıkış yapılmadı");
            }

        }
      
        private void anasayfa_Activated(object sender, EventArgs e)
        {
            adsoyad.Text = session.ad + " " + session.soyad;
            session.butonlariguncelle(yapimekle, kadrolistele, karakterekleme, yapimtur);
        }

        private void karakterekle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Daha doğrudan ve güvenli erişim: yardımcı metotları kullan
                Frame frame = GetMainFrame();
                Grid menuGrid = GetMenuGrid();

                if (frame == null)
                {
                    MessageBox.Show("MainFrame hala bulunamadı!\n\nMainWindow.xaml.cs dosyasında public Frame MainFrame { get; set; } tanımladığından emin ol.", "Hata");
                    return;
                }

                // Menü gizle, frame göster
                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Collapsed;

                frame.Visibility = Visibility.Visible;

                // Aynı sayfaya tekrar navigasyon yapmayı önle
                if (frame.Content == null || frame.Content.GetType() != typeof(karakterekle))
                {
                    var page = new karakterekle();
                    bool navigated = frame.Navigate(page);
                    if (!navigated)
                    {
                        MessageBox.Show("Sayfaya gezinilemedi (frame.Navigate false).", "Hata");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Hata:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Avatar tıklama: profil sayfasına geçiş
        private void btnAvatar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame frame = GetMainFrame();
                Grid menuGrid = GetMenuGrid();

                if (frame == null)
                {
                    MessageBox.Show("MainFrame bulunamadı, profil sayfasına gidilemiyor.", "Hata");
                    return;
                }

                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Collapsed;

                frame.Visibility = Visibility.Visible;

                if (frame.Content == null || frame.Content.GetType() != typeof(profil))
                {
                    frame.Navigate(new profil());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Profil sayfasına geçişte hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Frame'in içeriği değiştiğinde menüyü otomatik yönet
            var frame = GetMainFrame();
            var menu = GetMenuGrid();

            if (frame == null || menu == null)
                return;

            if (frame.Content == null)
            {
                menu.Visibility = Visibility.Visible;
                frame.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Bir sayfa yüklendi: menüyü gizle, frame'i göster
                menu.Visibility = Visibility.Collapsed;
                frame.Visibility = Visibility.Visible;
            }
        }

        private void kadrolistele_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Daha doğrudan ve güvenli erişim: yardımcı metotları kullan
                Frame frame = GetMainFrame();
                Grid menuGrid = GetMenuGrid();

                if (frame == null)
                {
                    MessageBox.Show("MainFrame hala bulunamadı!\n\nMainWindow.xaml.cs dosyasında public Frame MainFrame { get; set; } tanımladığından emin ol.", "Hata");
                    return;
                }

                // Menü gizle, frame göster
                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Collapsed;

                frame.Visibility = Visibility.Visible;

                // Aynı sayfaya tekrar navigasyon yapmayı önle
                if (frame.Content == null || frame.Content.GetType() != typeof(mevcutkadrolar))
                {
                    var page = new mevcutkadrolar();
                    bool navigated = frame.Navigate(page);
                    if (!navigated)
                    {
                        MessageBox.Show("Sayfaya gezinilemedi (frame.Navigate false).", "Hata");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Hata:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void yapimtur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Daha doğrudan ve güvenli erişim: yardımcı metotları kullan
                Frame frame = GetMainFrame();
                Grid menuGrid = GetMenuGrid();

                if (frame == null)
                {
                    MessageBox.Show("MainFrame hala bulunamadı!\n\nMainWindow.xaml.cs dosyasında public Frame MainFrame { get; set; } tanımladığından emin ol.", "Hata");
                    return;
                }

                // Menü gizle, frame göster
                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Collapsed;

                frame.Visibility = Visibility.Visible;

                // Aynı sayfaya tekrar navigasyon yapmayı önle
                if (frame.Content == null || frame.Content.GetType() != typeof(yapimturuekle))
                {
                    var page = new yapimturuekle();
                    bool navigated = frame.Navigate(page);
                    if (!navigated)
                    {
                        MessageBox.Show("Sayfaya gezinilemedi (frame.Navigate false).", "Hata");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Hata:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void yapimekle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame frame = GetMainFrame();
                Grid menuGrid = GetMenuGrid();

                if (frame == null)
                {
                    MessageBox.Show("MainFrame bulunamadı! MainWindow.xaml içinde x:Name=\"MainFrame\" olduğundan emin olun.", "Hata");
                    return;
                }

                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Collapsed;

                frame.Visibility = Visibility.Visible;

                if (frame.Content == null || frame.Content.GetType() != typeof(yapimekle))
                {
                    var page = new yapimekle();
                    bool navigated = frame.Navigate(page);
                    if (!navigated)
                    {
                        MessageBox.Show("Yapım ekleme sayfasına geçilemedi (frame.Navigate false).", "Hata");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}