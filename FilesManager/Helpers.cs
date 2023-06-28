using Shared;
using System.Text.Json;

namespace Manager;

public static class Helpers
{
    private static string JsonPath = Path.Combine(Directory.GetCurrentDirectory(), "sample.json");
    private static DateTime OldTracking { get; set; }
    private static DateTime JsonFileTracking { get; set; }


    public static DateTime GetFileLastChange(){
        var jsonFileTracking = File.GetLastWriteTime(JsonPath);

        return jsonFileTracking;

    }

    public static void ShowSimplePercentage()
        {
            for (int i = 0; i <= 100; i++) {
                Console.Write($"\rProgress: {i}%   ");
                Thread.Sleep(60);
            }

            Console.WriteLine("\rDone!          ");
        }

    public static SampleData DeserializeJson (string jsonPath){
        SampleData? sampleData;

        string jsonString = File.ReadAllText(jsonPath);        
        sampleData = JsonSerializer.Deserialize<SampleData>(jsonString);

        return sampleData ?? throw new NullReferenceException() ;
    }

    public static async Task<SampleData> ReloadJson(){
        SampleData data;

        OldTracking = JsonFileTracking;
        // Console.WriteLine("Old------------------- {0}", JsonFileTracking);

        JsonFileTracking = GetFileLastChange();
        // Console.WriteLine("newest------------------- {0}", JsonFileTracking);

        if (JsonFileTracking != OldTracking)
        {
            data = DeserializeJson(JsonPath);
            //Console.WriteLine("From reload {0}", data.Playlist?.Name);
            return data;
        }

        data = DeserializeJson(JsonPath);

        return data;

    }

    
}
