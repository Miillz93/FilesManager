using Shared;

namespace Manager;

public static class TracklistManager
{

    /// <summary>
    /// Generate A Tracklist
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task GenerateGenericTracklist(SampleData data) {
        var tracking = await FileManager.ReadContentWithSpecificInfos(data.Playlist?.TrackTracklist ??"", 2);
        var playlist = await CreateTracklistWithoutDuplicateDatas(data ?? new(), tracking, "multi");

        if(playlist.Count != 0){
            var newPath = await FileManager.CreateDocument(data?.Playlist?.TrackTracklist ?? "", data?.Playlist?.PlaylistName ?? "");
            string path = await FileManager.GetDocument(data?.Playlist?.TrackTracklist ??"", 2);
            var pathRoot = Path.GetDirectoryName(newPath);

            Thread.Sleep(1000);
            Console.WriteLine($"\n {playlist.Count} elements 'll Be Generate,  Make Sure To Not Cancel Otherwise You 'll Loose Your Progression 🔴 \n");
            Thread.Sleep(1000);

            foreach (var item in playlist)
            {
                await FileManager.ExportPathToDocumentAsync(path, item);
                Thread.Sleep(300);
                await FileManager.ExportPathToDocumentAsync(newPath, item);
                Thread.Sleep(300);
                var destinationPath = Path.Combine(pathRoot ??"", Path.GetFileName(item));
                await FileManager.CopyAsync(item, destinationPath);
            }

        }
        else await FileManager.CopyAsync(playlist, "");
        

    }

    public static async Task<List<string>> CreateTracklistWithoutDuplicateDatas(SampleData data, List<string> fileToRead, string type)
    {
        var playlistLoader = await PlaylistManager.LoadPlaylistData(data, type);
        // Check if playlist Already Use 
        
        var noDuplicatePlaylist = await PlaylistManager.IsNotDuplicated(playlistLoader ?? new(), fileToRead.ToArray());

        //Playlist generate
        var generated = await PlaylistManager.GeneratePlaylist(noDuplicatePlaylist, data!.Playlist!.MaxCount);

        return generated;
    }
}
