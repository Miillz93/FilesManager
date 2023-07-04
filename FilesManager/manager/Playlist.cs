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
    public static async Task CreateGenericPlaylist(SampleData data, int choiceType = 1) {

        if(data.Playlist?.BasePath is null ) return;

    }

    /// <summary>
    /// Looking for a file with a specific name
    /// </summary>
    /// <param name="path"></param>
    /// <param name="copyrightType"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetFilesWithSpecificInfoAsync(string path, string[] copyrightType)
    {
        var fileInfo = new List<string>();

        if(copyrightType is null) return new();

        foreach (var item in copyrightType)
        {
            var directoryInfo = Directory.GetFiles(path, $"*{item}*", SearchOption.AllDirectories);

            if(directoryInfo is null) return new();   
            
            fileInfo.AddRange(directoryInfo);
        }

        return fileInfo;
    }



    public static async Task<List<string>> GetIncludedPlaylist(List<string> mainList, string[] IncludeOnly)
    {
        var includedPlaylist =  new List<string>();
        
        if(mainList.Count == 0 && IncludeOnly.Length == 0) return new();
        
        includedPlaylist.AddRange(mainList.Where(playlist => IncludeOnly.Any(symbol => playlist.Contains(symbol, StringComparison.OrdinalIgnoreCase))));
        
        return includedPlaylist;
    }

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

        foreach (var file in filesList)
        {
            listing.Add(file);
        }   

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
        var directory = new List<string>() ; 

        foreach (var files in directories)
        {
            if (Directory.Exists(files.Value))
            {
                directory = Directory.GetFiles(files.Value).ToList();
                foreach (var root in directory)
                {
                    filesList.Add(root);
                }
            }
        }

        return filesList;
    }

    /// <summary>
    /// Create A Mix Playlist Based On Base Path With OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string? CreateMixPlaylist(SampleData data) {
        if(data == null) return string.Empty;

        return string.Empty;
    }

    public static async Task <string> CreateRandomPlaylist() {
        throw new NotImplementedException();
    }


    public static async Task<string> CreateRandomPlaylist(SampleData data) {
        throw new NotImplementedException();
    }

    //Random playlist using embeed file with exclude folder
    //Create a mix with or without duplicate data in log
}
