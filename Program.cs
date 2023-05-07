// See https://aka.ms/new-console-template for more information
System.Console.WriteLine("Start Program.................................");


bool continued = true;
int selector = 0        ;

while (continued)
{

    Console.WriteLine("Select one of the following");
    Console.WriteLine("1 - Moves files");
    Console.WriteLine("0 - Exit from Console App");
    string strSelector = Console.ReadLine();

    try{
        selector = int.Parse(strSelector);
    }catch(FormatException){
        Console.WriteLine("Bad format");
    }

    switch(selector){
        case 0:
            continued = true;
            break;
        case 1: 
            System.Console.WriteLine("case 1 begin");

            break;
        default:
            Console.WriteLine("Invalid number selected {0}", selector);
            break;
    }   
}


Console.WriteLine("End  Program.................................");
Console.ReadKey();





