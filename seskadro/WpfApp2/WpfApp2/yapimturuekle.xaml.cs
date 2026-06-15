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
        int id;
        public yapimturuekle()
        {
            InitializeComponent();
            listele();
        }
        void listele()
        {
            dgYapimTurleri.ItemsSource = connection.VeriGetir("yapimTurleri").DefaultView;
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

            if (string.IsNullOrWhiteSpace(txtYapimTuru.Text))
            {
                MessageBox.Show("Lütfen bir yapım türü girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (connection.kayıtkontrol("yapimTurleri", "turAdi", txtYapimTuru.Text))
            {
                MessageBox.Show("Bu yapım türü zaten mevcut.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
            if(id == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir yapım türü seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(string.IsNullOrWhiteSpace(txtYapimTuru.Text))
            {
                MessageBox.Show("Lütfen bir yapım türü girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            try
            {
                string sql = "UPDATE yapimTurleri SET turAdi = @yapimTuru WHERE turId = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@yapimTuru", txtYapimTuru.Text);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                listele();
                MessageBox.Show("Yapım türü başarıyla düzenlendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yapım türü düzenlenirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgYapimTurleri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgYapimTurleri.SelectedItem is DataRowView row)
            {
                txtYapimTuru.Text = row["turAdi"]?.ToString() ?? "";
                id = row["turId"] != null ? Convert.ToInt32(row["turId"]) : 0; // ID'yi al

            }
        }
    }
}
