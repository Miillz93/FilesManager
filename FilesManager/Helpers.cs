using Shared;
using System.Diagnostics;
using System.Text.Json;

namespace Manager;

public static class Helpers
{
    private static string JsonPath = Path.Combine(Directory.GetCurrentDirectory(), "sample/sample.json");
    private static DateTime OldTracking { get; set; }
    private static DateTime JsonFileTracking { get; set; }

    public static DateTime GetFileLastChange(){
        var jsonFileTracking = File.GetLastWriteTime(JsonPath);

        return jsonFileTracking;

    }

    public static void GetWelcomePage(int id) {
        string message;

        if(id == 1) {
            // message = "-----------------------------------------------------------";
            // message += "|                      Music PLATFORM                      |";
            // message +="-----------------------------------------------------------";
            message = "------------------------------------------------------------ \n";
            message += "|                      MUSIC PLATFORM                      | \n";
            message +="------------------------------------------------------------";

            Console.WriteLine(message);
        }else if(id == 2) {
            // message = "-----------------------------------------------------------";
            // message += "|                     GAMING PLATFORM                      |";
            // message +="-----------------------------------------------------------";
            message = "------------------------------------------------------------ \n";
            message += "|                     GAMING PLATFORM                      | \n";
            message +="------------------------------------------------------------";

            Console.WriteLine(message);
        }else{
            // message = "-----------------------------------------------------------";
            // message += "|                     GAMING PLATFORM                      |";
            // message +="-----------------------------------------------------------";
            message = "------------------------------------------------------------ \n";
            message += "|                          THEMES                          | \n";
            message +="------------------------------------------------------------";
            
            Console.WriteLine(message);
        }

    }

    public static void LoadSpinner(CancellationToken token){

        int counter = 0; 

        for (int i=0; i < 10000;i++) {
            Thread.Sleep(50);
            switch (counter % 4)
            {
                case 0:  Console.Write("\r/"); break;
                case 1: Console.Write("\r-"); break;
                case 2: Console.Write("\r\\"); break;
                case 3: Console.Write("\r|"); break;
            }
            counter++;
            
            token.ThrowIfCancellationRequested();
        }
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

        JsonFileTracking = GetFileLastChange();

        if (JsonFileTracking != OldTracking)
        {
            data = DeserializeJson(JsonPath);
            return data;
        }

        data = DeserializeJson(JsonPath);

        return data;

    }
}
