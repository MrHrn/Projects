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
    /// yapimturuekle.xaml etkileşim mantığı
    /// </summary>
    public partial class yapimturuekle : Page
    {
        public yapimturuekle()
        {
            InitializeComponent();
            listele();
        }
        void listele()
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT turAdi FROM yapimTurleri", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgYapimTurleri.ItemsSource = dt.DefaultView;
        }
        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                // Page'in host ettiği pencereyi al (Application.Current.MainWindow'a güvenme)
                Window hostWindow = Window.GetWindow(this);
                if (hostWindow is MainWindow anaPencere)
                {
                    Frame mainFrame = anaPencere.GetMainFrame();
                    Grid menuGrid = anaPencere.GetMenuGrid();

                    if (mainFrame != null)
                    {
                        // Frame içeriğini temizle ve gizle
                        mainFrame.Content = null;
                        mainFrame.Visibility = Visibility.Collapsed;
                    }

                    if (menuGrid != null)
                        menuGrid.Visibility = Visibility.Visible;
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

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO yapimTurleri (turAdi) VALUES (@yapimTuru)", con);
            cmd.Parameters.AddWithValue("@yapimTuru", txtYapimTuru.Text);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Yapım türü başarıyla eklendi.");
                txtYapimTuru.Clear();
                listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yapım türü eklenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnDuzenleTur_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
