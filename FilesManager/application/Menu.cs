using Shared;
using Manager;

namespace Application;
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
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► Manage files 👜");
            Console.WriteLine("2 ► Create playlists 💡");
            Console.WriteLine("3 ► Reload 🟠");
            Console.WriteLine("4 ► Back ⏪");

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
                    check = await SubMenuLevelTwoPlayListType(data, subIndex, platformId);

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
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► Copy Files ✒️");
            Console.WriteLine("2 ► Move Files 🧲");
            Console.WriteLine("3 ► Export Files 📗");
            Console.WriteLine("4 ► Delete Directory ⚡");
            Console.WriteLine("5 ► Reload 🟠");
            Console.WriteLine("6 ► Back ⏪");
            
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

                    if(platformId == 1 ) 
                        await FileManager.CopyOrMoveFileFromSourceFileAsync(data.PathDestination, data.FileMultiPath,data.VideoPath, data.SameSymbol, data.Action); 
                    
                    Thread.Sleep(2500);   

                    Console.WriteLine(Environment.NewLine);

                    break;               
                case 2:
                    data.Action = "move";

                    Console.WriteLine("------------------------ Moving Files \n");
                    
                    if(platformId == 1) 
                        await FileManager.CopyOrMoveFileFromSourceFileAsync(data.PathDestination, data.FileMultiPath, data.VideoPath, data.SameSymbol, data.Action); 

                    Thread.Sleep(2500);   

                    break;                
                case 3:

                    Console.WriteLine("-------------------------- Export Data \n");
                    await Task.Delay(50);
                    if(data is {EmbeedPath : not null, EmbeedDestination: not null, EmbeedFileName: not null})
                        await FileManager.ExportPathToDocumentAsync(data.EmbeedPath, data.EmbeedDestination, data.EmbeedFileName);
                    Thread.Sleep(2500);   

                    break;                
                case 4:
                    Console.WriteLine("--------------------------Delete Files \n");
                    await Task.Delay(500);
                    Console.WriteLine($" \"{data.EmbeedPath}\" and all subdirectories ll'be deleted. 🔴 \n");
                    await Task.Delay(2000);
                    if(data is {EmbeedPath: not null})
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

    public static async Task<int> SubMenuLevelTwoPlayListType (SampleData data, int index, int platformId){
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Choice playlist type \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► Tracklist🎵");
            Console.WriteLine("2 ► Playlist 🎶");
            Console.WriteLine("3 ► Reload 🟠");
            Console.WriteLine("4 ► Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await SubMenuLevelTwoTrackList(data, index, platformId);

                    break;
                case 2: 
                    await SubMenuLevelTwoPlayList(data, index, platformId);
  
                    break;                
                case 3: 
                    continued = true;
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

    /// <summary>
    /// Manage Playlist Operations
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="platformId"></param>
    /// <returns></returns>
    public static async Task<int> SubMenuLevelTwoPlayList (SampleData data, int index, int platformId){

        
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Choice playlist type \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► BASIC 🎞️");
            Console.WriteLine("2 ► MIX 💎");
            Console.WriteLine("3 ► RANDOM ⌚");
            Console.WriteLine("4 ► Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await SubMenuLevelTwoPlayListBasicType(data, index , platformId);
                    
                    break;
                case 2: 
                    await SubMenuLevelTwoPlayListMixType(data, index, platformId);
  
                    break;
                case 3: 
                    Console.ReadKey();

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

    public static async Task<int> SubMenuLevelTwoPlayListBasicType (SampleData data, int index, int platformId){
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("BASIC PLAYLIST \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► UNIQUE🎵");
            Console.WriteLine("2 ► MIX 🎶");
            Console.WriteLine("3 ► Reload 🟠");
            Console.WriteLine("4 ► Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await PlaylistManager.GenerateGenericPlaylist(data, "one");
                    break;
                case 2: 
                    await PlaylistManager.GenerateGenericPlaylist(data, "multi");
  
                    break;                
                case 3: 
                    continued = true;
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


    public static async Task<int> SubMenuLevelTwoPlayListMixType (SampleData data, int index, int platformId){
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("MIX PLAYLIST \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► UNIQUE🎵");
            Console.WriteLine("2 ► MIX 🎶");
            Console.WriteLine("3 ► Reload 🟠");
            Console.WriteLine("4 ► Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await PlaylistManager.GenerateMixPlaylist(data, "one");
                    break;
                case 2: 
                    await PlaylistManager.GenerateMixPlaylist(data, "multi");
  
                    break;                
                case 3: 
                    continued = true;
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



    public static async Task<int> SubMenuLevelTwoTrackList (SampleData data, int index, int platformId){

        
        bool continued = true;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Select TrackList \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► Generate 🎞️");
            // Console.WriteLine("2) Mix -> Based On Multiple Source 💎");
            Console.WriteLine("2 ► Back ⏪");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            List<string> playlist;

            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Thread.Sleep(1500);

                    Console.WriteLine("Create A Generic TrackList");
                    Thread.Sleep(1500);
                
                    await TracklistManager.GenerateGenericTracklist(data);
                    
                    break;
                case 2: 
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