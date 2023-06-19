using Shared;


namespace Manager; 
public static class PlaylistManager
{
    public static SampleData? CreateUniquePlaylist(SampleData data) {
        if(data == null) return null;

        return data;


    }

    public static string? CreateMixPlaylist(SampleData data) {
        if(data == null) return string.Empty;

        return string.Empty;
    }

    public static string CreateRandomPlaylist(SampleData data) {
    if(data == null) throw new NullReferenceException();

    return string.Empty;
    }

    //Random playlist using embeed file with exclude folder
    //Create a mix with or without duplicate data in log
}
