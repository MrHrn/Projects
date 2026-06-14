using Microsoft.Data.SqlClient;
using seskadrow;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// mevcutkadrolar.xaml etkileşim mantığı
    /// </summary>
    public partial class mevcutkadrolar : Page
    {
        public mevcutkadrolar()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection cek = new SqlConnection(connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT yapimAdi FROM yapimlar", cek);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbYapimSec.ItemsSource = dt.DefaultView;
            cmbYapimSec.DisplayMemberPath = "yapimAdi";
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Page'in host ettiği pencereyi güvenilir şekilde al
                Window hostWindow = Window.GetWindow(this);
                if (hostWindow is MainWindow anaPencere)
                {
                    // Ana penceredeki merkezi metodu kullan
                    anaPencere.ShowMenuAndHideFrame();
                    return;
                }

                // Fallback: NavigationService kullanmak yerine menu gösterimini sağlamaya çalış
                if (this.NavigationService != null && this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Ana pencere bulunamadı, geri işlem yapılamıyor.", "Hata");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Geri dönüş sırasında hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAra_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            string sorgu = @"select karakterAdi as 'Karakter Adı' , sb.sanatci_adi+ ' '+sb.sanatci_soyadi as 'Sanatçı Adı Soyadı' from Karakterler as k 
                                left join yapimlar as y on y.yapimID = k.yapimId
                                left join Sanatci_bilgileri as sb on sb.sanatci_id = seslendirmenId where y.yapimAdi = @yapimadi"
                                ;
            string yapimadi = cmbYapimSec.Text;
            SqlCommand cmd = new SqlCommand(sorgu, (SqlConnection)con);
            cmd.Parameters.AddWithValue("@yapimadi", yapimadi);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgKadrolar.ItemsSource = dt.DefaultView;
        }
    }
}
