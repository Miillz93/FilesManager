using System.Runtime.InteropServices;
using Shared;


namespace Manager; 
public static class PlaylistManager
{
    /// <summary>
    /// Create A Unique Playlist Based On Unique Path OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task CreateGenericPlaylist() {
        

        throw new NotImplementedException();

    }

    public static async Task<Dictionary<int, string>> GetPlaylist(string[] playlistPath){
        var filesDirectories = new Dictionary<int, string>();
            int counter = 0;
            foreach (var item in playlistPath)
            {
                var directories = await FileManager.GetDirectories(item, true, false);
                
                foreach (var files in directories)
                {
                FileAttributes attr = File.GetAttributes(files.Value);
                    
                    if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                        {
                            filesDirectories.Add(counter, files.Value);
                        }
                    counter++;
                }
            }

        return filesDirectories;
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
