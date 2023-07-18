using Shared;
using Manager;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace Application;
public static class Menu {

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

            if(platformId == 1) {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Manage files 👜 \n---------------------");
                Console.WriteLine("0 ► Exit from Console ❌");
                Console.WriteLine("1 ► Copy Files ✒️");
                Console.WriteLine("2 ► Move Files 🧲");
                Console.WriteLine("3 ► Export Files 📗");
                Console.WriteLine("4 ► Delete Directory ⚡");
                Console.WriteLine("5 ► Reload 🟠");
                Console.WriteLine("6 ► Back ⏪");
            } else{
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Manage files 👜 \n---------------------");
                Console.WriteLine("0 ► Exit from Console ❌");
                Console.WriteLine("1 ► Move Files 🧲");
                Console.WriteLine("2 ► Delete Directory ⚡");
                Console.WriteLine("3 ► Reload 🟠");
                Console.WriteLine("4 ► Back ⏪");
            }
            
            
            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out index);

            if(!success) index = -1;

            await Task.Delay(500);
            data = await Helpers.ReloadJson(platformId);

            switch(index){
                case 0:
                    Environment.Exit(0);

                    break;
                case 1:
                    if(platformId == 1) {
                        Console.WriteLine("------------------------ Copying Files \n");

                        data.Action = "copy";
                        System.Console.WriteLine($"Path destination {data.PathDestination}");
                        Console.ReadKey();
                        if(platformId == 1 ) 
                            await FileManager.CopyOrMoveFileFromSourceFileAsync(data.PathDestination, data.FileMultiPath,data.VideoPath, data.SameSymbol, data.Action); 

                        Console.WriteLine(Environment.NewLine);
                    } else{
                        Console.WriteLine("------------------------ Moving Files \n");
                        Console.WriteLine($"Path destination {data.PathDestination}");
                        await GamingManager.MovingGamingDocument(data);

                    }
                    
                    Thread.Sleep(2500);   

                    break;               
                case 2:
                    if(platformId == 1) {
                        
                        data.Action = "move";

                        Console.WriteLine("------------------------ Moving Files \n");
                        
                        await FileManager.CopyOrMoveFileFromSourceFileAsync(data.PathDestination, data.FileMultiPath, data.VideoPath, data.SameSymbol, data.Action); 

                    }else{
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
                                    Console.WriteLine("--------------------- Operation Cancel ❌");
                                break;
                                default: 
                                    Console.WriteLine("--------------------- Invalid Character {0} ❌", deleted);
                                break;
                            }
                    }
                    
                    Thread.Sleep(2500);   

                    break;                
                case 3:
                    if(platformId == 1) {
                        Console.WriteLine("-------------------------- Export Data \n");
                        await Task.Delay(50);
                        if(data is {EmbeedPath : not null, EmbeedDestination: not null, EmbeedFileName: not null})
                            await FileManager.ExportPathToDocumentAsync(data.EmbeedPath, data.EmbeedDestination, data.EmbeedFileName);
                    } else {

                    }
                    Thread.Sleep(2500);   

                    break;                
                case 4:
                    if(platformId == 1) {
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
                                    Console.WriteLine("--------------------- Operation Cancel ❌");
                                break;
                                default: 
                                    Console.WriteLine("--------------------- Invalid Character {0} ❌", deleted);
                                break;
                            }
                    } else{
                        continued = false; 
                    }
                break;
                case 5:
                    if(platformId == 1) continued = true; 
                    else continued = false;
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
            data = await Helpers.ReloadJson(platformId);

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
            Console.WriteLine("Choice Playlist Type \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► BASIC 🎞️");
            Console.WriteLine("2 ► MIX 💎");
            Console.WriteLine("3 ► RANDOM ⌚");
            Console.WriteLine("4 ► Short 🎦 ");
            Console.WriteLine("5 ► Back ⏪");

            string message;

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson(platformId);

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    await SubMenuLevelTwoPlayListBasicType(data, index , platformId,"BASIC");
                    
                    break;
                case 2: 
                    await SubMenuLevelTwoPlayListMixType(data, index, platformId, "MIX");
  
                    break;
                case 3: 
                    await SubMenuLevelTwoPlayListRandomType(data, index, platformId, "RANDOM");

                    break;
                case 4: 
                    await SubMenuLevelTwoPlaylistShortType(data, index, platformId, "SHORT"); 
                    continued = false; 
                    
                    break;
                case 5: 
                    continued = false;

                    break;
                default:
                    Console.WriteLine("----------------------- Invalid number selected {0}", selector);
                    break;
            }   
        }

        return index;
    }

    public static async Task<int> SubMenuLevelTwoPlayListBasicType (SampleData data, int index, int platformId, [Optional] string origin)
    {
        bool continued = true;
        List<string> playlist;


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
            data = await Helpers.ReloadJson(platformId);

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    playlist = await PlaylistManager.GenerateGenericPlaylist(data, "one");
                    var load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    var export = Console.ReadLine();
                    
                    switch (export.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);

                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);
                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }
                    Thread.Sleep(1000); 
                    Console.WriteLine(Environment.NewLine);
            
                    break;
                case 2: 
                    playlist = await PlaylistManager.GenerateGenericPlaylist(data, "multi");
                    load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    export = Console.ReadLine();
                    
                    switch (export.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);
                            
                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);
                            
                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }
                    Thread.Sleep(1000); 
                    Console.WriteLine(Environment.NewLine);
  
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

    public static async Task<int> SubMenuLevelTwoPlayListMixType (SampleData data, int index, int platformId, [Optional] string origin)
    {
        bool continued = true;
        List<string> playlist;

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
            data = await Helpers.ReloadJson(platformId);

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    playlist =  await PlaylistManager.GenerateMixPlaylist(data, "one");
                    var load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    var export = Console.ReadLine();
                    
                    switch (export.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);
                            
                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);
                            
                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }

                    Thread.Sleep(1000); 
                    Console.WriteLine(Environment.NewLine);

                    break;
                case 2: 
                    playlist =  await PlaylistManager.GenerateMixPlaylist(data, "multi");
                    load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    export = Console.ReadLine();
                    
                    switch (export?.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);

                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);
                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }
                    

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


    public static async Task<int> SubMenuLevelTwoPlayListRandomType (SampleData data, int index, int platformId, [Optional] string origin)
    {
        bool continued = true;

        Console.Clear();
        Helpers.GetWelcomePage(platformId) ; 
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("RANDOM PLAYLIST \n---------------------");
        Console.WriteLine("0 ► Exit from Console ❌");
        Console.WriteLine("1 ► UNIQUE🎵");
        Console.WriteLine("2 ► MIX 🎶");
        Console.WriteLine("3 ► Reload 🟠");
        Console.WriteLine("4 ► Back ⏪");

        while (continued)
        {
            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson(platformId);

            List<string> playlist;

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    playlist = await PlaylistManager.GenerateRandomPlaylist(data, "one");
                    var load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload?.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    var export = Console.ReadLine();
                    
                    switch (export?.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);

                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);

                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }
                    Thread.Sleep(1000); 
                    Console.WriteLine(Environment.NewLine);
                    break;
                case 2: 
                    playlist = await PlaylistManager.GenerateRandomPlaylist(data, "multi");
                    load = true;
                    while(load) {

                        Console.WriteLine("\nDo You Want To Reload The Playlist ? Y/N");
                        var reload = Console.ReadLine();
                        
                        switch (reload?.ToLower())
                        {
                            case "y":
                                playlist = await PlaylistManager.Reload(playlist);

                            break;
                            case "n":
                                Thread.Sleep(1000);
                                Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                load = false;
                                
                            break;
                            default: 
                                Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);
                            break;
                        }

                    }

                    Console.WriteLine("\nExport Playlist ? Y/N");
                    export = Console.ReadLine();
                    
                    switch (export.ToLower())
                    {
                        case "y":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, true);
                        break;
                        case "n":
                            await PlaylistManager.ExportPlaylist(data, playlist, origin, false);

                        break;
                        default: 
                            Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", export);
                            Thread.Sleep(1000); 
                            Console.WriteLine("--------------------- Operation Cancel ❌"); 
                            Thread.Sleep(1000); 
                        break;
                    }

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
            data = await Helpers.ReloadJson(platformId);

            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Thread.Sleep(1500);
                    Console.WriteLine("\nCreate A Generic TrackList \n");
                    Thread.Sleep(1000);
                
                    playlist  = await TracklistManager.GenerateGenericTracklist(data);
                    
                    Thread.Sleep(1000);

                    if(playlist is not null) {
                        
                        var load = true;
                        while(load) {

                            Console.WriteLine("\nDo You Want To Reload The Tracklist ? Y/N");
                            var reload = Console.ReadLine();
                            
                            switch (reload.ToLower())
                            {
                                case "y":
                                    playlist = await PlaylistManager.Reload(playlist);

                                break;
                                case "n":
                                    Thread.Sleep(1000);
                                    Console.WriteLine("--------------------- Operation Cancel ❌"); 
                                    load = false;
                                    
                                break;
                                default: 
                                    Console.WriteLine("--------------------- Invalid Character {0} ❌ \n", reload);

                                break;
                            }

                        }
                        await TracklistManager.ExportTracklist(data, playlist);

                    }
                    
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


    public static async Task<int> SubMenuLevelTwoPlaylistShortType (SampleData data, int index, int platformId, [Optional] string origin)
    {
        bool continued = true;
        List<string> playlist;

        while (continued)
        {
            Console.Clear();
            Helpers.GetWelcomePage(platformId) ; 
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("SHORT PLAYLIST \n---------------------");
            Console.WriteLine("0 ► Exit from Console ❌");
            Console.WriteLine("1 ► UNIQUE🎵");
            Console.WriteLine("2 ► MIX 🎶");
            Console.WriteLine("3 ► Reload 🟠");
            Console.WriteLine("4 ► Back ⏪");
            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            await Task.Delay(100);
            data = await Helpers.ReloadJson(platformId);


            switch(selector){
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    var load = true;
                    while(load) {

                        Console.WriteLine("\nHow Many Elements Do You Need ? Y/N");
                        var reload = Console.ReadLine();
                        bool value =  int.TryParse(reload, out int result);
                        if(!value) result = -1;

                        switch(result){
                            case int n when n > 1 && n < 5: 
                                
                                data.Playlist!.ShortMaxCount = n;
                                playlist = await PlaylistManager.GenerateShortPlaylist(data, "one");
                                await PlaylistManager.ExportPlaylist(data, playlist, origin, true);
                                Console.WriteLine(Environment.NewLine);
                                load = false;

                            break;
                            default:
                                Console.WriteLine("--------------------- Invalid character {0} ❌ \n", result);
                            break;
                        }
                    }




                    break;
                case 2: 
                    load = true;
                    while(load) {

                        Console.WriteLine("\nHow Many Elements Do You Need ? Y/N");
                        var reload = Console.ReadLine();
                        bool value =  int.TryParse(reload, out int result);
                        if(!value) result = -1;

                        switch(result){
                            case int n when n > 1 && n < 5: 
                                data.Playlist!.ShortMaxCount =  n;
                                playlist = await PlaylistManager.GenerateShortPlaylist(data, "one");

                            break;
                            default:
                                Console.WriteLine("--------------------- Invalid character {0} ❌ \n", result);
                            break;
                        }
                    }
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
}