using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Shared;
using System.Linq;
using System.Text.Json;

namespace Manager; 
public static class PlaylistManager
{
    /// <summary>
    /// Create A Unique Playlist Based On Unique Path OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task GenerateGenericPlaylist(SampleData data, string type) {
        var tracking = await FileManager.ReadContentWithSpecificInfos(data.Playlist?.TrackPlaylist ??"", 1);
        var playlist = await TracklistManager.CreateTracklistWithoutDuplicateDatas(data ?? new(), tracking, type, data!.Playlist!.PlaylistMaxCount);

        if(playlist.Count != 0){
            var newPath = await FileManager.CreateDocument(data?.Playlist?.TrackPlaylist ?? "", data?.Playlist?.PlaylistName ?? "");
            string path = await FileManager.GetDocument(data?.Playlist?.TrackPlaylist ??"", 1);
            var pathRoot = Path.GetDirectoryName(newPath);

            Thread.Sleep(1000);
            Console.WriteLine($"\n {playlist.Count} elements 'll Be Generate,  Make Sure To Not Cancel Otherwise You 'll Loose Your Progression 🔴 \n");
            Thread.Sleep(1000);

            foreach (var item in playlist)
            {
                await FileManager.ExportPathToDocumentAsync(path, item);
                Thread.Sleep(300);
                await FileManager.ExportPathToDocumentAsync(newPath, item);
                Thread.Sleep(300);
                var destinationPath = Path.Combine(pathRoot ??"", Path.GetFileName(item));
                await FileManager.CopyAsync(item, destinationPath);
            }

        }
        else await FileManager.CopyAsync(playlist, "");

    }


    public static async Task GenerateMixPlaylist(SampleData data, string type) {
        var tracking = await FileManager.ReadContentWithSpecificInfos(data.Playlist?.TrackPlaylist ??"", 1);
        var playlist = await TracklistManager.CreateTracklistWithoutDuplicateDatas(data ?? new(), tracking, type, data.Playlist.PlaylistMaxCount);

        if(playlist.Count != 0){
            var newPath = await FileManager.CreateDocument(data?.Playlist?.TrackPlaylist ?? "", data?.Playlist?.PlaylistName ?? "");
            var pathRoot = Path.GetDirectoryName(newPath);

            Thread.Sleep(1000);
            Console.WriteLine($"\n {playlist.Count} elements 'll Be Generate,  Make Sure To Not Cancel Otherwise You 'll Loose Your Progression 🔴 \n");
            Thread.Sleep(1000);

            foreach (var item in playlist)
            {
                await FileManager.ExportPathToDocumentAsync(newPath, item);
                Thread.Sleep(300);
                var destinationPath = Path.Combine(pathRoot ??"", Path.GetFileName(item));
                await FileManager.CopyAsync(item, destinationPath);
            }

        }
        else await FileManager.CopyAsync(playlist, "");

    }

    public static async Task GenerateRandomPlaylist(SampleData data, string type) {
        var playlistLoader = await LoadPlaylistData(data, type);
        var playlist = await GeneratePlaylist(playlistLoader, data.Playlist!.PlaylistMaxCount);
        if(playlist.Count != 0){
            var newPath = await FileManager.CreateDocument(data?.Playlist?.TrackPlaylist ?? "", data?.Playlist?.PlaylistName ?? "");
            var pathRoot = Path.GetDirectoryName(newPath);

            Thread.Sleep(1000);
            Console.WriteLine($"\n {playlist.Count} elements 'll Be Generate,  Make Sure To Not Cancel Otherwise You 'll Loose Your Progression 🔴 \n");
            Thread.Sleep(1000);

            foreach (var item in playlist)
            {
                await FileManager.ExportPathToDocumentAsync(newPath, item);
                Thread.Sleep(300);
                var destinationPath = Path.Combine(pathRoot ??"", Path.GetFileName(item));
                await FileManager.CopyAsync(item, destinationPath);
            }

        }
        else await FileManager.CopyAsync(playlist, "");

    }




    /// <summary>
    ///  Load Data By Filtered Data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task <List<string>> LoadPlaylistData(SampleData data,  string choiceType)
    {
        List<string>? playlist;

        if(data.LogPathDestination is null) return new();
        await FileManager.CreateDocuments(data.LogPathDestination);

        if(data?.Playlist?.BasePath is null ^ data?.Playlist?.UniquePathSource is null) return new(); 

        if (choiceType == "one") playlist = await GetPlaylist(data?.Playlist?.UniquePathSource ?? "");
        else playlist = await GetPlaylist(data?.Playlist?.BasePath ?? new string[1]);

        if (playlist is null && data?.Playlist?.IncludeOnly is null) return new();
    
        var playlistFilter = await GetIncludedPlaylist(playlist ?? new() , data?.Playlist?.IncludeOnly ?? new string[1]); 
        
        if(playlistFilter is null && data?.Playlist?.ExcludeFolderName is null) return new();

        var playlistExcluded =  await GetExcludedPlaylist(playlist ?? new(), data?.Playlist?.ExcludeFolderName ?? Array.Empty<string>());

        if(data?.Playlist?.CopyType is null) return new();

        var copyright = await FileManager.GetFilesWithSpecificInfoAsync(data.LogPathDestination, data.Playlist.CopyType);
        var noCopyrightContents = await FileManager.ReadContentWithSpecificInfos(copyright);
        var notCopyrighted = await IsNotDuplicated(playlistExcluded, noCopyrightContents);

        return notCopyrighted;
    }

    /// <summary>
    /// Check If Datas Already Exist
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="filesContent"></param>
    /// <returns></returns>
    public static async Task<List<string>> IsNotDuplicated(List<string> elements, List<string> filesContent){
        
        await Task.Delay(10);
        var contentFilter = elements.Where(element => !filesContent.Any(item => element.Contains(item, StringComparison.OrdinalIgnoreCase))).ToList();

        return contentFilter;
    }

        public static async Task<List<string>> IsNotDuplicated(List<string> elements, string[] filesContent){
        
        await Task.Delay(10);
        var contentFilter = elements.Where(element => !filesContent.Contains(element)).ToList();

        return contentFilter;
    }

    public static async Task<List<string>> GeneratePlaylist(List<string> elements, int counter){

        Random rand = new(); List<string> newElements = new();
        var newEelements = elements.OrderBy(_ => rand.Next()).Take(counter).ToList();
        int count = 1;

        foreach (var item in newEelements)
        {
            newElements.Add(item); 
            // Console.WriteLine($"{count}  ---- {item}");
            count++;
        }

        return newElements;

    }

    public static async Task<List<string>> Reload(List<string> elements){

        Random rand = new(); List<string> newElements = new();
        var newEelements = elements.OrderBy(_ => rand.Next());
        int count = 1;

        foreach (var item in newEelements)
        {
            newElements.Add(item); 
            Console.WriteLine($"{count}  ---- {item}");
            count++;
        }

        return newElements;

    }



    /// <summary>
    /// Add A Filter By Included Only Some Data
    /// </summary>
    /// <param name="mainList"></param>
    /// <param name="IncludeOnly"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetIncludedPlaylist(List<string> mainList, string[] IncludeOnly)
    {
        var includedPlaylist =  new List<string>();
        
        if(IncludeOnly.Length == 0) return mainList;
        
        includedPlaylist.AddRange(mainList.Where(playlist => IncludeOnly.Any(symbol => playlist.Contains(symbol, StringComparison.OrdinalIgnoreCase))));
        
        return includedPlaylist;
    }

    /// <summary>
    /// Add A Filter By Excluded Some Data
    /// </summary>
    /// <param name="mainPlaylist"></param>
    /// <param name="ExcludeFolderName"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetExcludedPlaylist(List<string> mainPlaylist, string[] ExcludeFolderName)
    {
        var excludedPlaylist =  new List<string>();

        excludedPlaylist.AddRange(mainPlaylist.Where(playlist => !ExcludeFolderName.Any(symbol => playlist.Contains(symbol,StringComparison.OrdinalIgnoreCase))));

        return excludedPlaylist;
    }

    /// <summary>
    /// Get A List Of Elements from One path
    /// </summary>
    /// <param name="uniquePathSource"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetPlaylist(string uniquePathSource){
        
        var listing = new List<string>();
        var directories = await FileManager.GetDirectories(uniquePathSource, true, false);
        var filesList =  GetFiles(directories);
        
        listing.AddRange(filesList);
        return listing;
    }

    /// <summary>
    /// Get A List Of Elements based On multiple path
    /// </summary>
    /// <param name="basePath"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetPlaylist(string[] basePath)
    {
        var listing = new List<string>();

        foreach (var item in basePath)
        {
            var directories = await FileManager.GetDirectories(item, true, false);
            var filesList =  GetFiles(directories);
            listing.AddRange(filesList);
        }
  
        return listing;
    }

    private static List<string> GetFiles(Dictionary<int, string> directories)
    {
        var filesList = new List<string>();
        
        foreach (var files in directories.Where(files => Directory.Exists(files.Value)))
        {
            var directory = Directory.GetFiles(files.Value).ToList();
            filesList.AddRange(directory);
        }

        return filesList;
    }

}
