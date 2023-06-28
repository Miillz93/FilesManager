
namespace Manager;

public static class Platform
{
    public static int Id { get; set; }
    private static readonly string Message = "\nApplication exit successfully....................👍 \n";

    public static async Task<int> LoadPlatformAsync(){
        
        int selector = 0;
        bool continued = true; 
        while (continued){

            Console.Clear();
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Choose One Platform \n---------------------");
            Console.WriteLine("0 ► Exit");
            Console.WriteLine("1 ► Music 🎹");
            Console.WriteLine("2 ► Gaming 🕹️");

            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out selector);

            if (!success) selector = -1;

            switch (selector)
            {
                case 0:
                    Console.WriteLine(Message);
                    Environment.Exit(0);
                    break;
                case 1:
                    Id = selector;
                    // var data = await Helpers.ReloadJson();
                    await GetPlatformAsync(Id);

                    break;
                case 2:
                    Id = selector;
                    // data = await Helpers.ReloadJson();
                    await GetPlatformAsync(Id);

                    break;
                default:
                    Console.WriteLine("--------------------- Invalid Platform indentifier {0} ❌", selector);
                    Thread.Sleep(2500);
                    break;
            }
        }

        return selector;

    }
    public static async Task GetPlatformAsync(int id){

        bool continued = true; 
         while (continued){

            switch (id)
            {
                case 1:
                    Id = id;
                    var data = await Helpers.ReloadJson();
                    int check = await Menu.MainMenuAsync(data, id);

                    if (check == 3) continued = false;
                    break;
                case 2:
                    Id = id;
                    data = await Helpers.ReloadJson();
                    check = await Menu.MainMenuAsync(data, id); 
                    if (check == 3) continued = false;
                    break;
                default:
                    Console.WriteLine("--------------------- Invalid number selected {0} ❌ \n", id);
                    break;
            }
        }

    }

}
