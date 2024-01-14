using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            // Menü ekranı
            Console.WriteLine("Menü:");
            Console.WriteLine("1. Kupon Hazırla");
            Console.WriteLine("2. Kupon Kontrol");
            Console.WriteLine("3. Json Analiz");
            Console.WriteLine("4. Çıkış");
            Console.Write("Seçiminizi yapın (1 veya 2 veya 3 veya 4): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateCupon();
                    break;
                case "2":
                    CheckCupon();
                    break;
                case "3":
                    Question2();
                    break;
                case "4":
                    Console.WriteLine("Çıkış yapılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim. Lütfen 1, 2, 3 veya 4 tuşlarından birini seçin.");
                    break;
            }
        }
    }

    static void GenerateCupon()
    {
        Console.WriteLine("Kaç tane kod üretmek istiyorsunuz?");
        if (int.TryParse(Console.ReadLine(), out int count))
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(FirstQuestion.GenerateCode());
            }
        }
        else
        {
            Console.WriteLine("Geçersiz sayı girdiniz.");
        }
    }

    static void CheckCupon()
    {
        Console.WriteLine("Kontrol etmek için bir kod girin:");
        string codeToCheck = Console.ReadLine();
        Console.WriteLine(FirstQuestion.IsCodeValid(codeToCheck) ? "Girilen kod geçerlidir." : "Girilen kod geçersizdir.");


    }

    static void Question2()
    {
        List<ResponseModel> ModelList = SecondQuestion.GetJson();


        ModelList = ModelList.Skip(1).ToList();


        List<ResponseModel> SortedList = ModelList.OrderBy(o => o.boundingPoly.vertices[0].y).ToList();

        List<String> ParsedList = SecondQuestion.ParseList(SortedList);
        int count = 1;
        foreach (var item in ParsedList)
        {
            Console.WriteLine(count + " " + item);
            count++;
        }
    }
}