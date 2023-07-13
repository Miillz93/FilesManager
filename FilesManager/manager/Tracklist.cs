using Shared;

namespace Manager;

public static class TracklistManager
{

    /// <summary>
    /// Generate A Tracklist
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task<List<string>> GenerateGenericTracklist(SampleData data)
    {
        var tracking = await FileManager.ReadContentWithSpecificInfos(data.Playlist?.TrackTracklist ?? "", 2);
        var playlist = await CreateTracklistWithoutDuplicateDatas(data ?? new(), tracking, "multi", data!.Playlist!.MaxCount);
        return playlist;
    }

    public static async Task ExportTracklist(SampleData data, List<string> playlist)
    {
        if (playlist.Count != 0)
        {
            var newPath = await FileManager.CreateDocument(data?.Playlist?.TrackTracklist ?? "", data?.Playlist?.PlaylistName ?? "", "");
            var path = await FileManager.GetDocument(data?.Playlist?.TrackTracklist ?? "", 2);
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
                var destinationPath = Path.Combine(pathRoot ?? "", Path.GetFileName(item));
                await FileManager.CopyAsync(item, destinationPath);
            }
        }
        else await FileManager.CopyAsync(playlist, "");
    }

    public static async Task<List<string>> CreateTracklistWithoutDuplicateDatas(SampleData data, List<string> fileToRead, string type, int counter)
    {
        var playlistLoader = await PlaylistManager.LoadPlaylistData(data, type);        
        // var noDuplicatePlaylist = await PlaylistManager.IsNotDuplicated(playlistLoader ?? new(), fileToRead.ToArray());
        var noDuplicatePlaylist = await PlaylistManager.IsNotDuplicated(playlistLoader ?? new(), fileToRead);
        Console.WriteLine($"\n{noDuplicatePlaylist.Count} Elements Found...✅ \n");
        var generated = await PlaylistManager.GeneratePlaylist(noDuplicatePlaylist, counter);

        return generated;
    }

    public static async Task<List<string>> CreateTracklistWithoutFilter(SampleData data, List<string> fileToRead, string type, int counter)
    {
        var playlistLoader = await PlaylistManager.LoadPlaylistDataWithoutFilter(data, type);
        
        var noDuplicatePlaylist = await PlaylistManager.IsNotDuplicated(playlistLoader ?? new(), fileToRead.ToArray());
        Console.WriteLine($"\n{noDuplicatePlaylist.Count} Elements Found...🔴 \n");

        var generated = await PlaylistManager.GeneratePlaylist(noDuplicatePlaylist, counter);

        return generated;
    }
}
