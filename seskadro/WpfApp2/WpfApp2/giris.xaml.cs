using Microsoft.Data.SqlClient;
using seskadrow;
using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// giris.xaml etkileşim mantığı
    /// </summary>
    public partial class giris : Window
    {
        public giris()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string secilenTur = "";
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    con.Open();
                    string sorgu;
                    string idkolon = null;
                    string adkolon = null;
                    string soyadkolon = null;
                    string puankolon = null;
                    string rutbe = null;
                    string parolakolon = null;
                    string aktiflik = null;

                    if (turcombo.SelectedItem is ComboBoxItem selectedItem)
                    {
                        secilenTur = selectedItem.Content?.ToString()?.Trim() ?? "";
                    }
                    switch (secilenTur)
                    {
                        case "Admin":
                            sorgu = "select * from Adminler where adminmail = @mail";
                            idkolon = "id";
                            adkolon = "adminadi";
                            soyadkolon = "adminsoyadi";
                            parolakolon = "adminparola";
                            session.kullanici_rol = secilenTur;
                            break;
                        case "Seslendirme Sanatçısı":
                            sorgu = "select * from Sanatci_bilgileri where sanatci_mail = @mail";
                            idkolon = "sanatci_id";
                            adkolon = "sanatci_adi";
                            soyadkolon = "sanatci_soyadi";
                            parolakolon = "sanatci_parola";
                            aktiflik = "aktiflik_durumu";
                            session.kullanici_rol = secilenTur;

                            break;
                        case "Kullanıcı":
                            sorgu = "select * from Kullanici_bilgileri where kullanici_mail = @mail";
                            idkolon = "kullanici_id";
                            adkolon = "kullanici_ad";
                            soyadkolon = "kullanici_soyad";
                            rutbe = "rutbe";
                            puankolon = "katkipuani";
                            session.kullanici_rol = secilenTur;
                            parolakolon = "kullanici_parola";
                            break;
                        default:
                            sorgu = null;
                            break;

                    }
                    using (SqlCommand cmd = new SqlCommand(sorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@mail", eposta.Text);
                        SqlDataReader rd = cmd.ExecuteReader();

                        if (rd.Read())
                        {
                            var storedPassword = rd[parolakolon].ToString();
                            
                            if (parola_islemleri.VerifyPassword(parola.Password, storedPassword))
                            {
                                benihatırla.Kaydet(eposta.Text.Trim(), parola.Password);
                                if (rd[idkolon] != DBNull.Value)
                                {
                                    session.oturumdurumu = true;
                                    session.id = Convert.ToInt32(rd[idkolon]);
                                    session.mail = eposta.Text;
                                    session.ad = rd[adkolon].ToString();
                                    session.soyad = rd[soyadkolon].ToString();
                                    
                                    if (secilenTur == "Kullanıcı")
                                    {
                                        session.kullanicikatki_puani = rd[puankolon].ToString();
                                        session.kullanici_rutbe = rd[rutbe].ToString();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Kayıt bulundu ama ID bilgisi eksik");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("E-posta veya parolanız hatalı", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            if (secilenTur == "Seslendirme Sanatçısı")
                            {
                                string aktiflikDurumu = rd[aktiflik].ToString();
                                if (aktiflikDurumu == "aktif_değil")
                                {
                                    MessageBox.Show("Hesabınız aktif değil. Lütfen yöneticinizle iletişime geçin.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                            }
                            MainWindow main = new MainWindow();
                            this.Close();
                            main.Show();
                        }
                        else
                        {
                            MessageBox.Show("E-posta veya parolanız hatalı", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("HATA: " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            turcombo.SelectedIndex = 0;

            // Eğer daha önce giriş bilgileri kaydedilmişse, onları yükle
            var hatirla = benihatırla.Oku();
            if (hatirla != null)
            {
                eposta.Text = hatirla.Mail;
                parola.Password = hatirla.Parola;
            }
        }

        // Şifremi unuttum butonuna tıklanınca sifreyenile penceresini açar
        private void SifremiUnuttum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new sifreyenile
                {
                    Owner = this
                };
                // modal istersen ShowDialog(); kullan
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Şifre yenileme penceresi açılamadı: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
