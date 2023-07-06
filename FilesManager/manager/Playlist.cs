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
    public static async Task CreateGenericPlaylist(SampleData data, [Optional] int choiceType) {

        
        
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

     /// <summary>
    /// Create A Directory For Playlist logs
    /// </summary>
    /// <param name="logPathDestination"></param>
    /// <param name="playlistName"></param>
    /// <returns></returns>
    public static async Task<string> CreateLogPathDirectory(string logPathDestination, string playlistName){
        
        await Task.Delay(100); 
        
        var path = Path.Combine(logPathDestination,playlistName);
        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        return path;

    }


}
