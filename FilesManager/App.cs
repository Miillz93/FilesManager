using Manager;

namespace Application;

public static class App
{
    public static async Task LoadApplicationAsync(){
        bool check = true;

        //while (check) {
            var data = await Helpers.ReloadJson();
            Console.WriteLine(Environment.NewLine);

            //await FileManager.CopyOrMoveFileFromSourceFileAsync(data);
            var index = await Menu.MainMenuAsync(data);
            if(index == 0) check = false;
            Console.WriteLine(Environment.NewLine);
       // }

    }

}
