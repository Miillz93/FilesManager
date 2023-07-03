﻿namespace Shared;

public class Playlist
{
    public string? PlaylistName {get; set;} 
    public string[]? BasePath {get; set;} // Playlist endpoint
    public string? UniquePathSource {get; set;} //Playlist subfolder combine with base and Name Playlist
    public string? TrackFileUniquePath {get; set;} // Path of unique folder -> Feb
    public string? TrackFileDuplicatePath {get;set;}
    public string? PlaylistPathDestination {get; set;}
    public string? TracklistPathDestination {get; set;}
    public string[]? ExcludeFolderName {get; set;}
    public string[]? IncludeOnly {get; set;} 
    public string[]? PlaylistMixName {get; set;}
    public string[]? PathMixSource {get; set;}
    public int? MaxCount {get; set;}
}
