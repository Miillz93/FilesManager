namespace Shared;

public class Playlist
{
    public string? Name {get; set;}
    public string? UniquePath {get; set;}
    public string? TrackFileUniquePath {get; set;}
    public string? TrackFileDuplicatePath {get;set;}
    public List<string>? ExcludeFolderName {get; set;}
    public List<string>? MixPathCombinedName {get; set;}
    public List<string>? PathCombinedSource {get; set;}
}
