// See https://aka.ms/new-console-template for more information
System.Console.WriteLine("Start Program.................................");


bool continued = true;
int selector = 0        ;

// while (continued)
// {

//     Console.WriteLine("Select one of the following");
//     Console.WriteLine("1 - Moves files");
//     Console.WriteLine("0 - Exit from Console App");
//     string strSelector = Console.ReadLine();

//     try{
//         selector = int.Parse(strSelector);
//     }catch(FormatException){
//         Console.WriteLine("Bad format");
//     }

//     switch(selector){
//         case 0:
//             continued = true;
//             break;
//         case 1: 
//             System.Console.WriteLine("Enter file Path");
//             string filePath =  Console.ReadLine();
//             System.Console.WriteLine("file path : {0}", filePath);
//             System.Console.WriteLine("Enter Video Path");
//             string videoDirectory = Console.ReadLine();
//             System.Console.WriteLine("Directory path : {0}", videoDirectory);

//             var Explorer = new Explorer(filePath, videoDirectory);
//             Explorer.MoveFileFromOneDirToAnother();
//             break;
//         default:
//             Console.WriteLine("Invalid number selected {0}", selector);
//             break;
//     }   
// }
        string filePath = "";
        string videoDirectory = "";

        var Explorer = new Explorer(filePath, videoDirectory);
        Explorer.MoveFileFromOneDirToAnotherAsync();

Console.ReadKey();





