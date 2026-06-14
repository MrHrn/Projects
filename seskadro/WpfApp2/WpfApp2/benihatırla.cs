using System.IO;
using System.Text.Json;

public class benihatırla
{
    private static readonly string dosyaYolu = "login_bilgileri.json";

    public string? Mail { get; set; }
    public string? Parola { get; set; }

    // Kaydet
    public static void Kaydet(string mail, string parola)
    {
        var data = new benihatırla { Mail = mail, Parola = parola };

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(dosyaYolu, json);
    }

    // Oku
    public static benihatırla? Oku()
    {
        if (!File.Exists(dosyaYolu))
            return null;

        string json = File.ReadAllText(dosyaYolu);
        return JsonSerializer.Deserialize<benihatırla>(json);
    }

    // Sil (isteğe bağlı)
    public static void Sil()
    {
        if (File.Exists(dosyaYolu))
            File.Delete(dosyaYolu);
    }
}