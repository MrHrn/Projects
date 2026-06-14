using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using seskadrow;

namespace WpfApp2
{
    public partial class karakterekle : Page
    {
        private int? _selectedSeslendirenId;
        private int? _selectedYapimId;
        private int? _selectedKarakterId;

        public karakterekle()
        {
            InitializeComponent();
            this.Loaded += KarakterEklePage_Loaded;
        }

        private void KarakterEklePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxes();
            dgKarakterler.ItemsSource = connection.VeriGetir("karakterler")?.DefaultView;
        }

        private void LoadComboBoxes()
        {
            try
            {
                using (var con = connection.BaglantiAc())
                {
                    // Seslendirenler
                    SqlDataAdapter daSes = new SqlDataAdapter(
                        "SELECT sanatci_id, sanatci_adi + ' ' + ISNULL(sanatci_soyadi,'') AS AdSoyad FROM Sanatci_bilgileri ORDER BY sanatci_adi",
                        con);
                    DataTable dtSes = new DataTable();
                    daSes.Fill(dtSes);
                    cmbSeslendiren.ItemsSource = dtSes.DefaultView;
                    cmbSeslendiren.DisplayMemberPath = "AdSoyad";
                    cmbSeslendiren.SelectedValuePath = "sanatci_id";

                    // Yapımlar
                    SqlDataAdapter daYapim = new SqlDataAdapter(
                        "SELECT yapimID, yapimAdi FROM yapimlar ORDER BY yapimAdi", con);
                    DataTable dtYapim = new DataTable();
                    daYapim.Fill(dtYapim);
                    cmbYapim.ItemsSource = dtYapim.DefaultView;
                    cmbYapim.DisplayMemberPath = "yapimAdi";
                    cmbYapim.SelectedValuePath = "yapimID";
                }

                if (cmbSeslendiren.Items.Count > 0) cmbSeslendiren.SelectedIndex = 0;
                if (cmbYapim.Items.Count > 0) cmbYapim.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ComboBox doldurma hatası:\n{ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

   


        private void dgKarakterler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKarakterler.SelectedItem is DataRowView row)
            {
                txtKarakterAdi.Text = row["karakterAdi"]?.ToString() ?? "";

                _selectedKarakterId = row["karakterID"] as int?;
                _selectedSeslendirenId = row["seslendirmenId"] as int?;
                _selectedYapimId = row["yapimId"] as int?;

                cmbSeslendiren.SelectedValue = _selectedSeslendirenId;
                cmbYapim.SelectedValue = _selectedYapimId;
            }
        }

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKarakterAdi.Text))
            {
                MessageBox.Show("Karakter adı boş olamaz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedSeslendirenId == null || _selectedYapimId == null)
            {
                MessageBox.Show("Lütfen seslendiren kişi ve yapım seçiniz.", "Uyarı",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (connection.kayıtkontrol("karakterler", "karakterAdi", txtKarakterAdi.Text.Trim()))
            {
                MessageBox.Show("Bu karakter adı zaten mevcut!", "Uyarı",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string sql = @"INSERT INTO karakterler (karakterAdi, yapimId, seslendirmenId) 
                   VALUES (@ad, @yapim, @ses)";

            try
            {
                using (var con = new SqlConnection(connection.ConnectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ad", txtKarakterAdi.Text.Trim());
                    cmd.Parameters.AddWithValue("@yapim", _selectedYapimId.Value);
                    cmd.Parameters.AddWithValue("@ses", _selectedSeslendirenId.Value);

                    con.Open();
                    int etkilenenSatir = cmd.ExecuteNonQuery();   // ← Önemli!

                    if (etkilenenSatir > 0)
                    {
                        connection.VeriGetir("karakterler");
                        MessageBox.Show("Karakter başarıyla eklendi.", "Başarılı",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        txtKarakterAdi.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenmedi! (Etki edilen satır = 0)", "Sorun",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Karakter eklenirken hata oluştu:\n{ex.Message}\n\nSQL: {sql}",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDuzenleKarakter_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedKarakterId == null)
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz karakteri tablodan seçin.", "Uyarı",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ... (düzenleme kodunu istersen buraya da aynı mantıkla yazabilirim)
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            // Mevcut geri dönüş kodun korunuyor
            Window hostWindow = Window.GetWindow(this);
            if (hostWindow is MainWindow anaPencere)
            {
                Frame mainFrame = anaPencere.GetMainFrame();
                Grid menuGrid = anaPencere.GetMenuGrid();

                if (mainFrame != null)
                {
                    mainFrame.Content = null;
                    mainFrame.Visibility = Visibility.Collapsed;
                }
                if (menuGrid != null)
                    menuGrid.Visibility = Visibility.Visible;
            }
        }

        private void cmbSeslendiren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSeslendirenId = cmbSeslendiren.SelectedValue as int?;
        }

        private void cmbYapim_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedYapimId = cmbYapim.SelectedValue as int?;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}