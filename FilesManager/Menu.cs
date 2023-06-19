using Shared;

namespace Manager;
public static class Menu{

    private static readonly string Message = "Application exit successfully....................;";
    
    public static void MainMenu(SampleData data){
        if (data == null) throw new ArgumentNullException();

        bool continued = true;

        while(continued)
        {        
            Console.WriteLine("Select one of the following");
            Console.WriteLine("0) Exit from Console");
            Console.WriteLine("1) Manage files ");
            Console.WriteLine("2) Create playlists");
            Console.WriteLine("3) Reload");

            int subIndex;
            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out int selector);

            if (!success) selector = -1;

            switch (selector)
            {
                case 0:
                    Console.WriteLine(Message);
                    Environment.Exit(0);
                    break;
                case 1:
                    subIndex = 1;

                    int check = SubMenuLevelOneFiles(data, subIndex);

                    if (check == 0) continued = false;
                    break;
                case 2:
                    subIndex = 2;
                    check = SubMenuLevelTwoPlayList(data, subIndex);

                    if (check == 0) continued = false;
                    break;
                case 3:
                    continued = false;
                    break;
                default:
                    Console.WriteLine("--------------------- Invalid number selected {0}", selector);
                    break;
            }

        };

    }

    public static int SubMenuLevelOneFiles (SampleData data, int index){

        bool continued = true;
        
        while(continued)
        {
            Console.WriteLine("Select one of the following action");
            Console.WriteLine("0) Exit from Console");
            Console.WriteLine("1) Copy Files");
            Console.WriteLine("2) Move Files");
            Console.WriteLine("3) Export");
            Console.WriteLine("4) Back");
            Console.WriteLine("5) Reload");
            
            string? strSelector = Console.ReadLine();
            bool success = int.TryParse(strSelector, out index);

            if(!success) index = -1;

            System.Console.WriteLine(Environment.NewLine);
            data = Helpers.ReloadJson();

            switch(index){
                case 0:
                    Console.WriteLine(Message);
                    Environment.Exit(0);

                    break;
                case 1:
                    Console.WriteLine("---------------------copying File");
                    Console.WriteLine(Environment.NewLine);
                    _ = FileManager.CopyOrMoveFileFromSourceFileAsync(data);                              

                    break;               
                case 2:
                    Console.WriteLine("------------------------Moving File");

                    break;                
                case 3:
                    Console.WriteLine("--------------------------Export Data");

                    break;
                case 4:
                    continued = false; 

                    break;
                case 5:
                    continued = true; 

                    break;
                default:
                    Console.WriteLine("----------------- Invalid number selected {0}", index);
                    break;
            }  
            Console.WriteLine(Environment.NewLine); 

        }
        
        Console.WriteLine("One level {0}", index);
        
        return index;
    }

    public static int SubMenuLevelTwoPlayList (SampleData data, int index){

        
        bool continued = true;

        while (continued)
        {
            Console.WriteLine("Choice playlist type");
            Console.WriteLine("0) Exit from Console");
            Console.WriteLine("1) Normal");
            Console.WriteLine("2) Mix");
            Console.WriteLine("3) Random");
            Console.WriteLine("4) Prev");

            string? strSelector = Console.ReadLine();

            bool success = int.TryParse(strSelector, out int selector);
            if(!success) selector = -1;

            data = Helpers.ReloadJson();

            switch(selector){
                case 0:
                    Console.WriteLine(Message);
                    Environment.Exit(0);
                    break;
                case 1:
                    Console.WriteLine("Normal Playlist with or without restriction");

                    break;
                case 2: 
                    Console.WriteLine("Mix Playlist with or without restriction");

                    break;
                case 3: 
                    Console.WriteLine("Create A random Playlist with or without restriction");


                    break;
                case 4: 
                    continued = false;

                    break;
                default:
                    Console.WriteLine("----------------------- Invalid number selected {0}", selector);
                    break;
            }   
            //Console.WriteLine(Environment.NewLine);
        }

        return index;
    }
}