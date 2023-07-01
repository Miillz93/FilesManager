namespace Shared;

public class Playlist
{
    public string? PlaylistName {get; set;} 
    public string? BasePath {get; set;} // Playlist endpoint
    public string? UniquePathSource {get; set;} //Playlist subfolder combine with base and Name Playlist
    public string? TrackFileUniquePath {get; set;} // Path of unique folder -> Feb
    public string? TrackFileDuplicatePath {get;set;}
    public List<string>? ExcludeFolderName {get; set;}
    public List<string>? IncludeOnly {get; set;} 
    public List<string>? PlaylistMixName {get; set;}
    public List<string>? PathMixSource {get; set;}
    public int? MaxCount {get; set;}
}
