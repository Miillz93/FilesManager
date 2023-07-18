using Shared;
using System.Diagnostics;
using System.Text.Json;

namespace Manager;

public static class Helpers
{
    private static DateTime OldTracking { get; set; }
    private static DateTime JsonFileTracking { get; set; }
    public static string? GamingJson { get; set; }
    public static string? MusicJson { get; set; }


    public static DateTime GetFileLastChange(int platformId){
        DateTime jsonFileTracking;
        
        if(platformId == 1) jsonFileTracking = File.GetLastWriteTime(MusicJson ?? "");
        else jsonFileTracking = File.GetLastWriteTime(GamingJson ?? "");

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

    public static async Task<SampleData> ReloadJson(int platformId){
        SampleData data;

        OldTracking = JsonFileTracking;

        JsonFileTracking = GetFileLastChange(platformId);

        if (JsonFileTracking != OldTracking)
        {
            if(platformId == 1) data = DeserializeJson(MusicJson ?? "");
            else  data = DeserializeJson(GamingJson ?? "");
            return data;
        }

        if(platformId == 1) data = DeserializeJson(MusicJson);
        else data = DeserializeJson(GamingJson);

        return data;

    }
}
