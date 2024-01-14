using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

class SecondQuestion
{

    public static List<ResponseModel> GetJson()
    {
        var jsonPath  = "..\\..\\..\\response.json";
        using (StreamReader streamReader = new(jsonPath)) // StreamReader ile dosya okuma işlemi başlatılıyor
        {
            var json = streamReader.ReadToEnd();
            List<ResponseModel> responses = JsonConvert.DeserializeObject<List<ResponseModel>>(json); // JSON içeriği ResponseModel listesine dönüştürülüyor
            return responses;
        }
    }

    public static List<String> ParseList(List<ResponseModel> responseList)
    {
        List<String> output = new List<string>();
        List<ResponseModel> tempList = new List<ResponseModel>();
        String tempStr = "";

        // İlk elemanın sınırlarını belirleyen y eksenindeki üst ve alt sınırlar hesaplanıyor
        double upperBound = (responseList[0].boundingPoly.vertices[0].y + responseList[0].boundingPoly.vertices[1].y) / 2;
        double lowerBound = (responseList[0].boundingPoly.vertices[2].y + responseList[0].boundingPoly.vertices[3].y) / 2;
        foreach (ResponseModel item in responseList)
        {
            // Her elemanın orta noktası hesaplanıyor
            double midPoint = (item.boundingPoly.vertices[0].y + item.boundingPoly.vertices[1].y + +item.boundingPoly.vertices[2].y + +item.boundingPoly.vertices[3].y) / 4;


            if (midPoint > upperBound && midPoint < lowerBound) // Eğer eleman belirlenen sınırlar içindeyse
            {
                tempList.Add(item); // Geçici listeye ekleniyor
            }

            else // Eğer eleman belirlenen sınırların dışındaysa
            {

                tempList = tempList.OrderBy(o => o.boundingPoly.vertices[0].x).ToList(); // Geçici liste x eksenine göre sıralanıyor

                foreach (var i in tempList)
                {
                    tempStr += i.description + " "; // Açıklamalar birleştiriliyor
                }
                tempStr = tempStr.Remove(tempStr.Length - 1); // Son boşluk kaldırılıyor


                output.Add(tempStr); // Birleştirilmiş string çıktı listesine ekleniyor

                // Yeni sınırlar belirleniyor
                upperBound = (item.boundingPoly.vertices[0].y + item.boundingPoly.vertices[1].y) / 2;
                lowerBound = (item.boundingPoly.vertices[2].y + item.boundingPoly.vertices[3].y) / 2;

                tempStr = "";
                tempList.Clear();

                tempList.Add(item);

            }
        }

        // Listenin sonunda kalan elemanlar için
        foreach (var i in tempList)
        {
            tempStr += i.description + " ";
        }
        tempStr = tempStr.Remove(tempStr.Length - 1);
        output.Add(tempStr);


        return output;
    }

}

public class BoundingPoly
{
    public List<Vertex> vertices { get; set; }
}

public class ResponseModel
{
    public string locale { get; set; }
    public string description { get; set; }
    public BoundingPoly boundingPoly { get; set; }
}

public class Vertex
{
    public int x { get; set; }
    public int y { get; set; }
}
