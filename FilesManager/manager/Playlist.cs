using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Shared;
using System.Linq;

namespace Manager; 
public static class PlaylistManager
{
    /// <summary>
    /// Create A Unique Playlist Based On Unique Path OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task CreatePlaylist(SampleData data, [Optional] int choiceType) {



    }

    
    /// <summary>
    ///  Load Data By Filtered Data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task <Dictionary<string, List<string>>> LoadPlaylistData(SampleData data) {
        
        var dict = new Dictionary<string, List<string>>();

        if(data.LogPathDestination is null) return new();
        await FileManager.GenerateDocuements(data.LogPathDestination);

        if(data?.Playlist?.PlaylistPathDestination is null ^ data?.Playlist?.PlaylistName is null) return new();
        string path = await FileManager.CreateDirectory(data?.Playlist?.PlaylistPathDestination ?? "", data?.Playlist?.PlaylistName ?? "", 1);
        dict.Add("path", new List<string>{path});

        if(data?.Playlist?.BasePath is null ) return new(); 

        if(data.Playlist.UniquePathSource is null) return new();
        var playlist = await GetPlaylist(data.Playlist.UniquePathSource);
        dict.Add("playlist", playlist);
    
        if(playlist is null && data.Playlist.IncludeOnly is null) return new();
    
        var playlistFilter = await GetIncludedPlaylist(playlist , data.Playlist.IncludeOnly); 
        dict.Add("playlistFilter", playlistFilter);
        
        if(playlistFilter is null && data.Playlist.ExcludeFolderName is null) return new();

        var playlistExcluded =  await GetExcludedPlaylist(playlist, data.Playlist.ExcludeFolderName ?? Array.Empty<string>());
        dict.Add("playlistExcluded", playlistExcluded);

        if(data.Playlist.CopyType is null) return new();

        var copyright = await FileManager.GetFilesWithSpecificInfoAsync(data.LogPathDestination, data.Playlist.CopyType);
        dict.Add("copyright", copyright);
        // for copyright
        var noCopyrightContents = await FileManager.ReadContentWithSpecificInfos(copyright);
        var notCopyrighted = await IsNotDuplicated(playlistExcluded, noCopyrightContents);
        dict.Add("notCopyrighted", notCopyrighted);

        return dict;
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
            foreach (var file in filesList)
            {
                listing.Add(file);
            }
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
