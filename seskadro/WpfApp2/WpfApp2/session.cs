using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Net.Mail;
using Microsoft.Data.SqlClient;
using seskadrow;

namespace WpfApp2
{
    class session
    {
        //sbzq uaos jjex zade uygulama şifresi
        //katkı puanı ve rütbe sadece kullanıcı rolündeki ziyaretçiler içindir
        public static bool oturumdurumu { get; set; } = false;
        public static string kullanici_rutbe { get; set; } = string.Empty;
        public static string kullanici_rol { get; set; }
        public static string kullanicikatki_puani { get; set; } = string.Empty;//bu veriyi veri tabanına kaydederken int türüne dönüştür, kodda sorun oluşturduğu için string olarak oluşturuldu
        public static int id { get; set; } = -1;
        public static string mail { get; set; } = string.Empty;
        public static string ad { get; set; } = string.Empty;
        public static string soyad { get; set; } = string.Empty;

        public static void butonlariguncelle(Button yapımbutonu, Button kadrolistelemebutonu, Button karaktereklemebutonu, Button yapımturueklemebutonu)
        {
            switch (kullanici_rol)
            {

                case "Admin":
                    yapımbutonu.IsEnabled = true;
                    kadrolistelemebutonu.IsEnabled = true;
                    karaktereklemebutonu.IsEnabled = true;
                    yapımbutonu.IsEnabled = true;
                    yapımturueklemebutonu.IsEnabled = true;
                    break;
                case "Seslendirme Sanatçısı":
                    yapımbutonu.IsEnabled = true;
                    kadrolistelemebutonu.IsEnabled = true;
                    karaktereklemebutonu.IsEnabled = true;
                    yapımbutonu.IsEnabled = true;
                    yapımturueklemebutonu.IsEnabled = false;
                    yapımturueklemebutonu.ClearValue(Button.BackgroundProperty);
                    yapımturueklemebutonu.Background = Brushes.Black;
                    break;
                case "Kullanıcı":
                    yapımbutonu.IsEnabled = false;
                    yapımturueklemebutonu.IsEnabled = false;
                    yapımbutonu.Background = Brushes.Black;
                    yapımturueklemebutonu.Background = Brushes.Black;
                    break;
            }
        }

        public static void recoverpassword(string mail, string geciciparola, ComboBoxItem tur)
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            con.Open(); // Bağlantıyı en başta açalım
            string tabloadi = "";
            string mailkolon = "";
            string parolakolon = "";
            string ad = "";
            string adDegeri = "";
            // Kullanıcı türüne göre tablo ve kolon isimleri belirlenir
            if (tur.Content.ToString() == "Seslendirme Sanatçısı")
            {
                tabloadi = "Sanatci_bilgileri";
                parolakolon = "sanatci_parola";
                ad = "sanatci_adi";
                mailkolon = "sanatci_mail";
            }

            else if (tur.Content.ToString() == "Kullanıcı")
            {
                tabloadi = "Kullanici_bilgileri";
                parolakolon = "kullanici_parola";
                ad = "kullanici_ad";
                mailkolon = "kullanici_mail";
            }

            else if (tur.Content.ToString() == "Admin")
            {
                tabloadi = "Adminler";
                parolakolon = "adminparola";
                ad = "adminAdi";
                mailkolon = "adminmail";
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir kullanıcı türü seçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            // Kolon isimleri belirlendikten sonra, kullanıcıyı doğrulamak ve adını almak için SQL sorgusu hazırlanır
            string selectSql = $"select * from {tabloadi} where {mailkolon} = @mail";
            using (SqlCommand cmd = new SqlCommand(selectSql, con))
            {
                cmd.Parameters.AddWithValue("@mail", mail);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) 
                    {
                        adDegeri = dr[ad].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Bu e-posta adresi ile kayıt bulunamadı.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Kullanıcı yoksa işlemi bitir
                    }
                }
            }
            // oluşturulan geçici parola hashlenir ve veri tabanında güncellenir, ardından mail gönderilir
            string hash = parola_islemleri.HashPassword(geciciparola);
            string updateSql = $"update {tabloadi} set {parolakolon} = @geciciparola where {mailkolon} = @mail";

            using (SqlCommand cdm = new SqlCommand(updateSql, con))
            {
                cdm.Parameters.AddWithValue("@geciciparola", hash);
                cdm.Parameters.AddWithValue("@mail", mail);

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // Mail mesajı sınıfı objemizin konfigrasyonu, kısaca kargo paketinin hazırlanması
                using (MailMessage mailMessage = new MailMessage())
                {
                    
                    mailMessage.From = new MailAddress("oyuncumarasli@gmail.com");
                    mailMessage.To.Add(mail);
                    mailMessage.Subject = "Şifre Sıfırlama Talebi";
                    mailMessage.Body = $"Merhaba {adDegeri},\n\nGeçici şifreniz: {geciciparola}\nLütfen giriş yaptıktan sonra şifrenizi değiştirin.";

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        // SMTP istemcisi ayarları, kısaca kargo aracının özellikleri
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential("oyuncumarasli@gmail.com", "rooijfddjjagscle");
                        smtpClient.UseDefaultCredentials = false;


                        try
                        {
                            // veri tabanı güncellenir
                            cdm.ExecuteNonQuery();
                            // kargo paketi hazırlanır ve gönderilir
                            smtpClient.Send(mailMessage);
                            MessageBox.Show("Şifre sıfırlama maili gönderildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Mail gönderirken hata: " + ex.Message);
                        }
                    }

                }

            }

        }
    }
}
