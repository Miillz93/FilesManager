using Manager;

namespace Application;

public static class Theme
{
    public static async Task<int> LoadThemeAsync(int id)
    {
        int selector = 0;
        bool continued = true; 
        while (continued){



            Console.Clear();
            Helpers.GetWelcomePage(id);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Choose Theme \n---------------------");
            Console.WriteLine("1 ► Default");
            Console.WriteLine("2 ► Light Mode");
            Console.WriteLine("3 ► Back");
            Console.WriteLine("4 ► Exit");

            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out selector);

            if (!success) selector = -1;

            switch (selector)
            {
                case 1:
                    Console.ResetColor();
                    break;                 
                case 2:
                    await LoadWriteMode();
                    break;
                case 3:
                    continued = false;
                    break;
                case 4:  
                    Environment.Exit(0);
                break;

                default:
                    Console.WriteLine("--------------------- Invalid Platform indentifier {0} ❌", selector);
                    Thread.Sleep(2500);
                    break;
            }
        }

        return selector;
    }

    public static async Task LoadWriteMode(){
        Console.BackgroundColor = ConsoleColor.White;
         Console.ForegroundColor = ConsoleColor.Black;
    }
}