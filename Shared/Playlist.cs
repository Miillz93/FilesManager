namespace Shared;

public class Playlist
{
    public string? Name {get; set;} // Playlist Name
    public string? Base {get; set;} // Playlist endpoint
    public string? UniquePath {get; set;} //Playlist subfolder combine with base and Name Playlist
    public string? TrackFileUniquePath {get; set;} // Path of unique folder -> Feb
    public string? TrackFileDuplicatePath {get;set;}
    public List<string>? ExcludeFolderName {get; set;}
    public List<string>? IncludeOnly {get; set;} 
    public List<string>? MixPathCombinedName {get; set;}
    public List<string>? PathCombinedSource {get; set;}
    public int? MaxCount {get; set;}
}
