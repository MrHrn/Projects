using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data;
using Microsoft.Data.SqlClient;
using seskadrow;
using WpfApp2;
using System.Net.NetworkInformation;
using System.IO;

namespace seskadrow
{
    public static class connection
    {
        
        public static string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database= sesKadro;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static DataTable VeriGetir(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"SELECT * FROM {tableName}";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri getirilirken bir hata oluştu: " + ex.Message);
                    return null;
                }
            }
        }
        public static SqlConnection BaglantiAc()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();  // Bağlantıyı aç
            return conn;
        }
        public static void puanislemi(string mail, int puan, int id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string sql = "UPDATE Kullanici_bilgileri SET katkipuani = @puan WHERE kullanici_mail = @mail and kullanici_id = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@puan", puan);
                        cmd.Parameters.AddWithValue("@mail", mail);
                        cmd.ExecuteNonQuery();
                        session.kullanicikatki_puani = puan.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Puan güncellenirken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    con.Close();
                    MessageBox.Show("Puanınız başarıyla güncellendi!");
                }

            }
        }
        public static bool kayıtkontrol(string tablo, string colon, string text)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {tablo} where {colon} = @value", con);
            cmd.Parameters.AddWithValue("@value", text);
            try
            {
                con.Open();
                bool result = false;
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("İşlem tamamlandı");
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri kontrolü sırasında bir hata oluştu: " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        public static void rutbeislemi(string mail, string rutbe)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    int puan = Convert.ToInt32(session.kullanicikatki_puani);
                    if (puan == 40 || puan > 40)
                    {
                        rutbe = "Kadro Ustası";
                    }
                    else if (puan > 30)
                    {
                        rutbe = "Usta";
                    }
                    else if (puan > 20)
                    {
                        rutbe = "Kalfa";
                    }
                    else if (puan > 10)
                    {
                        rutbe = "Çırak";
                    }
                    else if (puan >= 0)
                    {
                        rutbe = "Çaylak";
                    }
                    else
                    {
                        rutbe = "Çaylak";
                    }

                    con.Open();
                    string sql = "UPDATE Kullanici_bilgileri SET rutbe = @rutbe WHERE kullanici_mail = @mail and kullanici_id = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@rutbe", rutbe);
                        cmd.Parameters.AddWithValue("@mail", mail);
                        cmd.Parameters.AddWithValue("@id", session.id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Rütbe güncellenirken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    con.Close();
                    MessageBox.Show("Rütbeniz başarıyla güncellendi!");
                }
            }

        }
        public static async Task<bool> ag_kontrolu()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000);
                    if (reply.Status != IPStatus.Success)
                    {
                        MessageBox.Show("İnternet bağlantısı yok. Lütfen bağlantınızı kontrol edin.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bağlantı kontrolü sırasında hata: " + ex.Message);
                return false;
            }
            return true;
        }
    }
}
