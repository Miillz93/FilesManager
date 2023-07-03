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
        }else {
            // message = "-----------------------------------------------------------";
            // message += "|                     GAMING PLATFORM                      |";
            // message +="-----------------------------------------------------------";
            message = "------------------------------------------------------------ \n";
            message += "|                     GAMING PLATFORM                      | \n";
            message +="------------------------------------------------------------";

            Console.WriteLine(message);
        }

    }

    public static async Task LoadSpinner(){

        int counter = 0; 

        var sw = new Stopwatch();
        sw.Start();
        
        for (int i=0; i < 1000;i++) {

            switch (counter % 4)
            {
                case 0:  Console.Write("\r/"); break;
                case 1: Console.Write("\r-"); break;
                case 2: Console.Write("\r\\"); break;
                case 3: Console.Write("\r|"); break;
            }

            counter++;
            await Task.Delay(200);
            //Console.SetCursorPosition(0, Console.CursorTop);
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
