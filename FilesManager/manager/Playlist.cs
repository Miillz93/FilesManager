using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Shared;


namespace Manager; 
public static class PlaylistManager
{
    /// <summary>
    /// Create A Unique Playlist Based On Unique Path OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task CreateGenericPlaylist(SampleData data, int choiceType = 1) {

        
        // playlist = await GetPlaylist(data.Playlist.UniquePathSource);        
        throw new NotImplementedException();

    }

    public static async Task<List<string>> GetIncludedPlaylist(List<string> mainList, string[] IncludeOnly)
    {
        var includedPlaylist =  new List<string>();
        int counter = 0;

        foreach (var playlist in mainList)
        {

            if(playlist.Length != 0  && IncludeOnly.Length != 0){

                if(IncludeOnly.Any(symbol => playlist.Contains(symbol, StringComparison.OrdinalIgnoreCase))) {
                    includedPlaylist.Add(playlist);
                }
            }
            else includedPlaylist.Add(playlist);
        }

        return includedPlaylist;
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
