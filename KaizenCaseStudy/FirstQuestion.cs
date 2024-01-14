using System;
using System.Security.Cryptography;
using System.Text;

class FirstQuestion
{
    private const string Characters = "ACDEFGHKLMNPRTXYZ234579";
    private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    public static string GenerateCode()
    {
        var code = new char[8];
        for (int i = 0; i < 8; i++)
        {
            byte[] randomBytes = new byte[1]; // Rastgele sayı üretimi için 1 byte'lık dizi
            rng.GetBytes(randomBytes); // RNGCryptoServiceProvider ile rastgele byte alınır
            int randomIndex = randomBytes[0] % Characters.Length; // Alınan byte, kullanılacak karakter setinin boyutu ile mod alınarak bir index oluşturur
            code[i] = Characters[randomIndex]; // İlgili karakter setinden seçilen karakterle kod dizisini doldurur
        }

        // Ekstra güvenlik için, kodun ilk 7 hanesini alınıp  8. hane olarak hash'in bir parçası eklenir
        string firstPart = new string(code, 0, 7); // Kodun ilk 7 karakterini alır
        code[7] = GenerateCheckDigit(firstPart); // 8. karakter olarak, ilk 7 karakterin hash değerinin bir parçasını kullanır
        return new string(code);
    }

    public static bool IsCodeValid(string code)
    {
        if (code.Length != 8 || !code.All(c => Characters.Contains(c))) // Kodun uzunluğunu kontrol eder ve her karakterin belirlenen karakter setinde olup olmadığını kontrol eder
            return false;

        // Kodun son hanesini doğrulanır
        char checkDigit = GenerateCheckDigit(code.Substring(0, 7)); // Kodun ilk 7 karakterini alıp, bunun hash değerinin bir parçasını hesaplar
        return checkDigit == code[7]; // Hesaplanan kontrol hanesi ile kodun son karakterini karşılaştırır ve eşitse geçerli, değilse geçersiz olarak döner

    }

    private static char GenerateCheckDigit(string input)
    {
        using (var sha256 = SHA256.Create()) // SHA256 hash algoritmasını kullanarak bir SHA256 nesnesi oluşturur
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input)); // Girdi string'ini byte dizisine dönüştürür ve SHA256 hash değerini hesaplar
            int index = hash[0] % Characters.Length; // Hash'in ilk byte'ını alır ve karakter setinin uzunluğu ile mod alarak bir index hesaplar
            return Characters[index]; // Hesaplanan index'e karşılık gelen karakteri karakter setinden seçer ve döndürür
        }
    }
}
