# D-Nem Projesi

## Veritabanı Kurulumu (Önemli!)

Bu proje **SQL Server LocalDB** kullanmaktadır. Projeyi ilk çalıştırdığınızda veritabanını kurmanız gerekmektedir.

### 1. Veritabanı Scriptlerini Çalıştırma

1. Visual Studio'da projeyi açın.
2. **SQL Server Object Explorer** panelini açın:  
   `View → SQL Server Object Explorer`
3. Sol tarafta **`(localdb)\MSSQLLocalDB`** yazan yere sağ tıklayın ve **"New Query"** seçeneğini seçin.
4. Aşağıdaki sırayla scriptleri çalıştırın:

   **Adım 1:** Önce veritabanı ve tablo yapılarını oluşturan scripti çalıştırın.  
   (`sesKadro_Tablolar.sql` veya benzeri dosya)

   **Adım 2:** Daha sonra verileri (Admin, Sanatçı, Kullanıcı, Karakter vb.) ekleyen scripti çalıştırın.  
   (`sesKadro_Veri.sql` veya benzeri dosya)

> **Not:** Scriptler başarıyla çalıştıysa altta `Veritabanı ve tüm veriler başarıyla oluşturuldu.` mesajını görmelisiniz.

---

### 2. Connection String Ayarı

Projede veritabanı bağlantısı `connection.cs` sınıfı üzerinden yönetilmektedir.

1. `connection.cs` dosyasını açın.
2. `ConnectionString` property’sini bulun.
3. Bilgisayarınızdaki SQL Server instance’ına göre aşağıdaki gibi güncelleyin:

```csharp
public static string ConnectionString = 
    "Server=(localdb)\\MSSQLLocalDB;Database=sesKadro;Trusted_Connection=True;MultipleActiveResultSets=true;";