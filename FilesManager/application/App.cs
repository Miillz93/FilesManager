using Manager;
using Shared;

namespace Application;

public static class App
{
    public static async Task LoadApplicationAsync(){
        
        string root= Path.Combine(Path.GetTempPath(), "temp.txt");
        
        if(! File.Exists(root))
            await FileManager.CreateDocument(root);
        

        while (true)
        {
            string[] text = File.ReadAllLines(root);

            Console.Clear();

            if(text.Length == 0)
            {
                InitApplication(root);

            }
            else if(text.Length == 2) 
            {
                Helpers.MusicJson = text[0];
                Helpers.GamingJson = text[1];
                var index = await Platform.LoadPlatformAsync();

                  if(index == -1) break;
            }
            else
            {
                Console.WriteLine($"Please Remove \"{root}\" Then Relaunch The App");
                Thread.Sleep(2500); break;
            }
        }

        

    }

    private static void InitApplication(string root)
    {
        while (true)
        {
            using StreamWriter sw = File.AppendText(root);
            System.Console.WriteLine(Environment.NewLine);
            Thread.Sleep(1000);
            Console.WriteLine("\nBefore We Started...");
            Thread.Sleep(1000);
            Console.WriteLine("Insert Json Music File Configuration: ");
            var json = Console.ReadLine();
            if (json is null ^ !File.Exists(json))
            {
                if (File.ReadAllText(json ?? "").Length < 25) continue;
                Console.Clear();
                continue;
            }

            System.Console.WriteLine($"Music Source: {json}");
            sw.WriteLine(json);
            Helpers.MusicJson = json;

            Console.WriteLine("\nInsert Json Gaming File Configuration:");
            var json1 = Console.ReadLine();

            if (json1 is null ^ !File.Exists(json1) ^ json1!.Equals(json, StringComparison.OrdinalIgnoreCase))
            {
                if (File.ReadAllText(json1 ?? "").Length < 20) continue;
                Console.Clear();
                continue;
            }

            Console.WriteLine($"Gaming Source: {json1}");

            sw.WriteLine(json1);
            File.SetAttributes(root, File.GetAttributes(root) | FileAttributes.ReadOnly);
            Helpers.GamingJson = json1;
            break;
        }

        Console.ReadKey();
    }
}
