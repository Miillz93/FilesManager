using Shared;
using System.Text.Json;

namespace filesmanager;

internal static class Helpers
{
    private static readonly string JsonPath = Path.Combine(Environment.CurrentDirectory, "sample.json");
    private static DateTime OldTracking { get; set; }
    private static DateTime JsonFileTracking { get; set; }


    public static DateTime GetFileLastChange(){
        var jsonFileTracking = File.GetLastWriteTime(JsonPath);

        return jsonFileTracking;

    }

    public static SampleData DeserializeJson (string jsonPath){
        SampleData? sampleData;

        string jsonString = File.ReadAllText(jsonPath);        
        sampleData = JsonSerializer.Deserialize<SampleData>(jsonString);

        return sampleData ?? throw new NullReferenceException() ;
    }

    public static SampleData ReloadJson(){
        SampleData data;

        OldTracking = JsonFileTracking;
        Console.WriteLine("Old------------------- {0}", JsonFileTracking);

        JsonFileTracking = GetFileLastChange();
        Console.WriteLine("newest------------------- {0}", JsonFileTracking);

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
