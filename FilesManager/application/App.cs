using Manager;

namespace Application;

public static class App
{
    public static async Task LoadApplicationAsync(){
        bool check = true;

       //while (check) {
        var data = await Helpers.ReloadJson();
        Console.WriteLine(Environment.NewLine);

        await FileManager.CopyOrMoveFileFromSourceFileAsync(data.PathDestination, data.FileMultiPath, 
                                    data.VideoPath, data.SameSymbol, data.Action); 

        // var index = await Platform.LoadPlatformAsync();
        // if(index == -1) check = false;

        Console.WriteLine(Environment.NewLine);
    //    }

    }

}
