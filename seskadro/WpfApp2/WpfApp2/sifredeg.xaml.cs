using seskadrow;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// sifredeg.xaml etkileşim mantığı
    /// </summary>
    public partial class sifredeg : Window
    {
        public sifredeg()
        {
            InitializeComponent();
        }


        private async void gonder_Click(object sender, RoutedEventArgs e)
        {
            gonder.IsEnabled = false; // Gönder butonunu devre dışı bırak
            if (await connection.ag_kontrolu() == false)
            {
                gonder.IsEnabled = true; // Ağ kontrolü başarısızsa butonu tekrar etkinleştir
                return;
            }
            if (tip.SelectedItem is ComboBoxItem secilenOge)
            {
                string secilenTip = secilenOge.Content.ToString();

                session.recoverpassword(txtEmail.Text, parola_islemleri.CreatePassword(10), tip.SelectedItem as ComboBoxItem);
            }
            else
            {
                MessageBox.Show("Lütfen bir kullanıcı tipi seçin!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.Close();
        }
    }
}
