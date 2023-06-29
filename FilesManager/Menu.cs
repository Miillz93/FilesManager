using Shared;

namespace Manager;
public static class Menu{

    public static async Task<int> MainMenuAsync(SampleData data, int platformId){
        if (data == null) throw new ArgumentNullException();

        int subIndex = 0;
        bool continued = true; 

        while(continued)
        {   
            Console.Clear();

            Helpers.GetWelcomePage(platformId);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Select one of the following \n---------------------");
            Console.WriteLine("0) Exit from Console ❌");
            Console.WriteLine("1) Manage files 👜");
            Console.WriteLine("2) Create playlists 💡");
            Console.WriteLine("3) Reload 🟠");
            Console.WriteLine("4) Back ⏪");

            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out int selector);
            Console.WriteLine("");


            if (!success) selector = -1;

            switch (selector)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    subIndex = 1;

                    int check = await SubMenuLevelOneFilesAsync(data, subIndex, platformId);

                    if (check == 0) continued = false;
                    break;
                case 2:
                    subIndex = 2;
                    check = await SubMenuLevelTwoPlayList(data, subIndex, platformId);

                    if (check == 0) continued = false;
                    break;                
                case 3:
                    continued = true;

                    break;
                case 4:
                    subIndex = 3;
                    continued = false;
                    break;
                default:
                    Console.WriteLine("--------------------- Invalid number selected {0} ❌ \n", selector);
                    Thread.Sleep(2500);
                    break;
            }

        };

        return subIndex;
    }

    public static async Task<int> SubMenuLevelOneFilesAsync (SampleData data, int index, int platformId){

        bool continued = true; 
        while(continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Manage files 👜 \n---------------------");
            Console.WriteLine("0) Exit from Console ❌");
            Console.WriteLine("1) Copy Files ✒️");
            Console.WriteLine("2) Move Files 🧲");
            Console.WriteLine("3) Export Files 📗");
            Console.WriteLine("4) Delete Directory ⚡");
            Console.WriteLine("5) Reload 🟠");
            Console.WriteLine("6) Back ⏪");
            
            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out index);

            if(!success) index = -1;

            await Task.Delay(500);
            data = await Helpers.ReloadJson();

            switch(index){
                case 0:
                    Environment.Exit(0);

                    break;
                case 1:
                    
                    Console.WriteLine("------------------------ Copying Files \n");

                    data.Action = "copy";

                    if(platformId == 1 ) await FileManager.CopyOrMoveFileFromSourceFileAsync(data); 
                    else 
                    
                    Thread.Sleep(2500);   

                    Console.WriteLine(Environment.NewLine);
                    // continued = false;

                    break;               
                case 2:
                    data.Action = "move";

                    Console.WriteLine("------------------------ Moving Files \n");
                    
                    if(platformId == 1) await FileManager.CopyOrMoveFileFromSourceFileAsync(data); 
                    else
                    Thread.Sleep(2500);   

                    break;                
                case 3:

                    Console.WriteLine("-------------------------- Export Data \n");
                    await Task.Delay(50);
                    await FileManager.ExportPathToFileAsync(data);
                    Thread.Sleep(2500);   

                    break;                
                case 4:
                    Console.WriteLine("--------------------------Delete Files \n");
                    await Task.Delay(500);
                    Console.WriteLine($" \"{data.EmbeedPath}\" and all subdirectories ll'be deleted. 🔴 \n");
                    await Task.Delay(2000);
                    _ = await FileManager.GetDirectories(data.EmbeedPath, false, true);
                    await Task.Delay(500);
                    Console.WriteLine("Do You Still Want To Remove The Folder ? Y/N");
                    var deleted = Console.ReadLine();
        
                    switch (deleted.ToLower())
                    {
                        case "y":
                            await FileManager.DeleteDirectory(data.EmbeedPath);
                        break;
                        case "n":
                            Thread.Sleep(1000);
                            Console.WriteLine("--------------------- operation cancel ❌");
                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid character {0} ❌", deleted);
                        break;
                    }
                    Thread.Sleep(2500);
                break;
                case 5:
                    continued = true; 

                    break;
                case 6:
                    continued = false; 

                    break;
                default:
                    Console.WriteLine("-------------------------- Invalid number selected {0} ", index);
                    Thread.Sleep(2500);

                    break;
            }  

        }
        
        return index;
    }

    public static async Task<int> SubMenuLevelTwoPlayList (SampleData data, int index, int platformId){

        
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Choice playlist type \n---------------------");
            Console.WriteLine("0) Exit from Console ❌");
            Console.WriteLine("1) Normal 🎞️");
            Console.WriteLine("2) Mix 💎");
            Console.WriteLine("3) Random ⌚");
            Console.WriteLine("4) Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(500);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Console.WriteLine("Normal Playlist with or without restriction");

                    break;
                case 2: 
                    Console.WriteLine("Mix Playlist with or without restriction");

                    break;
                case 3: 
                    Console.WriteLine("Create A random Playlist with or without restriction");


                    break;
                case 4: 
                    continued = false;

                    break;
                default:
                    Console.WriteLine("----------------------- Invalid number selected {0}", selector);
                    break;
            }   
        }

        return index;
    }
}