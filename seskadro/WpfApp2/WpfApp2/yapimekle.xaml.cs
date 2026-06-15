using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using seskadrow;
using System.Data; // SQL bağlantısı için gerekli namespace

namespace WpfApp2
{
    /// <summary>
    /// yapimekle.xaml etkileşim mantığı
    /// </summary>
    public partial class yapimekle : Page
    {
        int id;
        public yapimekle()
        {
            InitializeComponent();

            // Sayfa yüklendiğinde combobox'u doldur
            this.Loaded += Yapimekle_Loaded;
        }

        private void Yapimekle_Loaded(object? sender, RoutedEventArgs e)
        {
            list(); // Yapımlar listesini yükle
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM yapimTurleri", con);
            try
            {
                con.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                cmbYapimTuru.ItemsSource = dataTable.DefaultView;
                cmbYapimTuru.DisplayMemberPath = "turAdi"; // Görüntülenecek sütun
                cmbYapimTuru.SelectedValuePath = "turId"; // Seçilen değerin alınacağı sütun
            }
            catch (Exception ex)
            {
                MessageBox.Show("Türler yüklenirken hata: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            if(txtYapimAdi.Text == "" || txtYapimYili.Text == "" || cmbYapimTuru.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (connection.kayıtkontrol("yapimlar", "yapimAdi", txtYapimAdi.Text))
            {
                MessageBox.Show("Bu yapım zaten mevcut.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string ad = txtYapimAdi.Text?.Trim() ?? "";
            int yil = Convert.ToInt32(txtYapimYili.Text);
            int tur = Convert.ToInt32(cmbYapimTuru.SelectedValue);

            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Yapım adı girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (yil == null)
            {
                MessageBox.Show("Yapım yılı girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kaydetme mantığını siz yazacaksınız (DB insert vb.)
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO yapimlar (yapimAdi, yapimYili, turu) VALUES (@ad, @yil, @tur)", con);
            cmd.Parameters.AddWithValue("@ad", ad);
            cmd.Parameters.AddWithValue("@yil", yil);
            cmd.Parameters.AddWithValue("@tur", tur);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Yapım başarıyla kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaydetme işlemi sırasında hata: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            // Ana pencereye dön — merkezi metod varsa onu kullan
            var host = Window.GetWindow(this) as MainWindow;
            if (host != null)
            {
                host.ShowMenuAndHideFrame();
                return;
            }

            // Fallback: navigation geçmişi varsa geri git
            if (this.NavigationService != null && this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        void list()
        {
            dgYapimlar.ItemsSource = connection.VeriGetir("yapimlar").DefaultView;
        }
        private void btnDuzenle_Click(object sender, RoutedEventArgs e)
        {
            if(txtYapimAdi.Text == "" || txtYapimYili.Text == "" || cmbYapimTuru.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (connection.kayıtkontrol("yapimlar", "yapimAdi", txtYapimAdi.Text))
            {
                MessageBox.Show("Bu yapım zaten mevcut.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            try
            {
                string sql = "UPDATE yapimlar SET yapimAdi = @ad, yapimYili = @yil, turu = @tur WHERE yapimId = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ad", txtYapimAdi.Text?.Trim() ?? "");
                cmd.Parameters.AddWithValue("@yil", Convert.ToInt32(txtYapimYili.Text));
                cmd.Parameters.AddWithValue("@tur", Convert.ToInt32(cmbYapimTuru.SelectedValue));
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                list(); // Düzenleme sonrası listeyi güncelle
                MessageBox.Show("Yapım başarıyla düzenlendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Düzenleme işlemi sırasında hata: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgYapimlar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgYapimlar.SelectedItem is DataRowView row)
            {
                txtYapimAdi.Text = row["yapimAdi"]?.ToString() ?? "";
                txtYapimYili.Text = row["yapimYili"]?.ToString() ?? "";
                cmbYapimTuru.SelectedValue = row["turu"] as int?;
                id = row["yapimId"] as int? ?? 0;
            }
            }
        }
    }

