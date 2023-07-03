using Shared;


namespace Manager; 
public static class PlaylistManager
{
    /// <summary>
    /// Create A Unique Playlist Based On Unique Path OR Without A Filter
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static SampleData? CreatePlaylist(SampleData data) {
        if(data == null) return null;

        return data;

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
