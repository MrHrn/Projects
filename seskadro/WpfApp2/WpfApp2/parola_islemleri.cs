using Microsoft.Data.SqlClient;
using seskadrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    public class parola_islemleri
    {
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public static string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 8); // Parolayı hash'le
            return hash;
        }
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash); // Parolayı doğrula
        }
        public static void updatePassword(string newPassword)
        {
            // Sadece 1 defa çalıştırıp veritabanını temizle
            string sifirlanmisSifre = newPassword;
            string yeniHash = BCrypt.Net.BCrypt.HashPassword(sifirlanmisSifre, workFactor: 8);

            using (SqlConnection con = new SqlConnection(connection.ConnectionString))
            {
                con.Open();
                string sql = "UPDATE Sanatci_bilgileri SET sanatci_parola = @newHash WHERE sanatci_mail = @mail";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@newHash", yeniHash);
                    cmd.Parameters.AddWithValue("@mail", "x@gmail.com"); 
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Sanatçının parolası başarıyla '123' olarak sıfırlandı!");
        }
    }
}
