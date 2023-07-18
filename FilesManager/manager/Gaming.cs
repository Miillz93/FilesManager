using System.Diagnostics;
using Shared;
using System.Linq;
using System.Runtime.InteropServices;

namespace Manager;

public static class GamingManager
{
    public static async Task MovingGamingDocument(SampleData data){
        
        var playlist = await LoadGamingData(data);
        await FileManager.MoveAsync(playlist ?? new(), data.PathDestination ?? "");
               
    } 
    
    public static async Task<List<string>> LoadGamingData(SampleData data) 
    {
        List<string>? playlist;

        if(data.PathDestination is null ) return new() ;
        await FileManager.CreateDirectory(data.PathDestination);

        playlist = await PlaylistManager.GetPlaylist(data?.PathSource ?? "");


        if (playlist is null ^ data?.IncludeItem is null) return new();
    
        var playlistFilter = await PlaylistManager.GetIncludedPlaylist(playlist ?? new() , data?.IncludeItem ?? Array.Empty<string>()); 

        if(playlistFilter is null ^ data?.ExcludeItem is null) return new();

        var playlistExcluded =  await PlaylistManager.GetExcludedPlaylist(playlistFilter ?? new(), data?.ExcludeItem ?? Array.Empty<string>());

        return playlistExcluded;
    }


        


}