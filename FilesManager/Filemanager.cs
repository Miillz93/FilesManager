using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shared;


namespace Manager;

public static class FileManager
{

    public static async Task<(Dictionary<int, string>, Dictionary<int, string>)> ExtractData(SampleData data) {

        string FilePath = Path.Combine(data.PathSource, data.FilePath);
        string[] arrayOfListElementFromFile = new string[]{};

        if (!File.Exists(FilePath)) throw new FileNotFoundException();


        foreach (var item in data.VideoPath) {
            if(!Directory.Exists(item)) throw new DirectoryNotFoundException(item);

        }
        var itemForHeader = new Dictionary<int, string>();

        try { arrayOfListElementFromFile = File.ReadAllLines(FilePath) ; } 
        catch (Exception) { throw; }



        itemForHeader = GetItemHeaderHashFromFile(arrayOfListElementFromFile);

        var musicFullPath = GetFullPath(data.VideoPath, arrayOfListElementFromFile);

        return (itemForHeader, musicFullPath);
    }

    public static async Task CopyOrMoveFileFromSourceFileAsync(SampleData data)
    {
        string parent = "", child = "", parentFull = ""; string message = "Finished................";
        List<string> musicsToExport = new();
        string[] arrayOfListElementFromFile = Array.Empty<string>();

        var sw = new Stopwatch();

        var item = await ExtractData(data);
        var itemForHeader = item.Item1;
        var musicFullPath = item.Item2;

        int number = itemForHeader.Count;
        int musicCounter = musicFullPath.Count;
        int count = 0, counting = 1;

        foreach (var header in itemForHeader) {
            if(count < number && counting < number){
                var firstIndex = itemForHeader.ElementAt(count);
                var lastIndex = itemForHeader.ElementAt(counting);
                
                int countMusic = firstIndex.Value.Where(x => x == '#').Count();

                if(countMusic == 3) {
                    parent = firstIndex.Value.Replace('#', ' ').Trim();
                    
                    //Console.WriteLine($"{firstIndex.Key} - {firstIndex.Value}");
                    parentFull = Path.Combine(data.PathDestination, parent);

                    try{ if(Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                    catch (Exception) {throw;}

                    foreach (var musics in musicFullPath)
                    {
                        parentFull = Path.Combine(data.PathDestination, parent, Path.GetFileName(musics.Value));

                        if(firstIndex.Key < musics.Key && musics.Key < lastIndex.Key){
                            //Console.WriteLine(musics.Value);
                            Console.WriteLine(parentFull);

                        }
                            
                    }

                }else{
                    child = firstIndex.Value.Replace('#', ' ').Trim();
                    parentFull = Path.Combine(parent, child);

                    try{ if(Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                    catch (System.Exception) {throw;}
                    
                    Console.WriteLine($"{firstIndex.Key} - {firstIndex.Value}");

                    foreach (KeyValuePair<int, string> musics in musicFullPath)
                    {
                        parentFull = Path.Combine(data.PathDestination, parent, child, Path.GetFileName(musics.Value));

                        if(firstIndex.Key < musics.Key && musics.Key < lastIndex.Key){
                            //Console.WriteLine(musics.Value);
                            Console.WriteLine(parentFull);

                        }
                            
                    }
                    
                }

            }
            count++; counting++;
            
        }

        //System.Console.WriteLine($" header {number}   --   {musicCounter}");

        throw new Exception();
        //this.LogsDataFromOriginAfterMove(musicFullPath, ItemHeader,  FilePath);

        // foreach (var item in musicsToExport)
        // {
        //     Console.WriteLine("item {0}", item);
        // }
        //var message = await LogsDataFromOriginAfterCopy(musicsToExport, data);
        await Task.Delay(100);
        Console.WriteLine(message);
    }

    //Get List item Matching from logs
    public static async Task<string> LogsDataFromOriginAfterCopy(List<string> musicsToExport,  SampleData data)
    {
        string message;

        if (musicsToExport == null || data.LogFileDestination == null || data.LogPathDestination == null) 
            throw new ArgumentNullException("data is null");

        try { if (!Directory.Exists(data.LogPathDestination)) Directory.CreateDirectory(data.LogPathDestination); }
        catch (DirectoryNotFoundException e) { Console.WriteLine(e.ToString()); }

        string currentLogDirectory = Path.GetFullPath(data.LogPathDestination);
        var logFile = Path.Combine(currentLogDirectory, data.LogFileDestination);

        // logFile = Path.ChangeExtension(logFile, DateTime.Now.ToString("dd-MM-yyyy")+".txt");

        try
        {
    
            if (File.Exists(logFile))
            {
                var logFileListToCheckIfExisted = File.ReadAllLines(logFile).ToList();

                if(logFileListToCheckIfExisted is not null && !logFileListToCheckIfExisted.Any(data => musicsToExport.Contains(data, StringComparer.OrdinalIgnoreCase))){
                    using StreamWriter sw = File.AppendText(logFile);
                    sw.WriteLine("");
                    sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                    foreach (var item in musicsToExport)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine("");
                }
                
            }
            else
                File.WriteAllLines(logFile, musicsToExport);

            await Task.Delay(100);
            message = "---------------------------- Texts added to Log File successfully";
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (Exception) { throw; }

        return message;
    }

    public static async Task ExportEmbeedFileFromFolderAsync(SampleData data){
                
        if(data is null || data.PathSource is null || data.FileDestination is null || data.PathDestination is null) throw new ArgumentNullException(nameof(data));  
        
        var (filecombined, fileItems) = await GetFilesWithInFoldersAndSubFolders(data); 

        try
        {
            if (File.Exists(filecombined))
            {
                using StreamWriter sw = File.AppendText(filecombined);
                sw.WriteLine("");
                sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                foreach (var item in fileItems)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("");
            }
            else{
                using StreamWriter sw = new(filecombined);
                sw.WriteLine("");
                sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                foreach (var item in fileItems)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("");
            }
            await Task.Delay(1000);
            System.Console.WriteLine("---------------------Texts added to Log File successfully");
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (Exception) { throw; }
    }

    public static async Task<(string, Dictionary<string, List<string>>)> GetFilesWithInFoldersAndSubFolders(SampleData data)
    {
        if(data is null || data.PathSource is null || data.FileDestination is null || data.PathDestination is null) throw new ArgumentNullException(nameof(data));  

        string filecombined ="";
        DirectoryInfo[] directories; var items = new List<string>();
        Dictionary<string, List<string>> fileItems = new();

        try
        {
            var directory = new DirectoryInfo(data.PathSource);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            //root= Directory.GetDirectories(mainPath,"*", searchOption: SearchOption.AllDirectories);
            if (!Directory.Exists(data.PathDestination)) Directory.CreateDirectory(data.PathDestination);

            filecombined = Path.Combine(data.PathDestination, data.FileDestination);

            foreach (var item in directories)
            {
                //System.Console.WriteLine(item.FullName);
                fileItems.TryAdd(item.FullName, items);
                items = new();
            }
        }
        catch (UnauthorizedAccessException e) {System.Console.WriteLine(e.ToString());  }
        catch (DirectoryNotFoundException e) { System.Console.WriteLine(e.ToString()); }
        
        await Task.Delay(100);
        return (filecombined, fileItems);
    }

    /// <summary>
    /// Get a list of item in a specific folder when items match each other
    /// </summary>
    /// <param name="videoDirectory"></param>
    /// <param name="arrayOfFullPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Dictionary<int, string> GetFullPath(string[] videoDirectory, string[] arrayOfListElementFromFile)
    {
        if (videoDirectory is null) throw new ArgumentNullException();
        
        Dictionary<int, string> musicFullPath = new(), musicFull = new();
        HashSet<string> clips = new();  SortedDictionary<int, string> dict = new();

        List<string> list = new List<string>(), musics =new List<string>();
        
        foreach (var item in videoDirectory.Where(item => !Directory.Exists(item)))
        {
            throw new DirectoryNotFoundException(item);
        }

        //directories = directory.GetDirectories("*", SearchOption.AllDirectories);
        foreach (var item in videoDirectory.ToList())
        {
            var data = Directory.GetFiles(item);

            if(data.Length > 0) musics.AddRange(data); 

        }

        for (int h = 0; h < arrayOfListElementFromFile.Length; h++)
        {
            for (int i = 0; i < musics.Count; i++)
            {
                string extension = Path.GetFileNameWithoutExtension(musics[i]);
                 if (arrayOfListElementFromFile[h].Contains(extension, StringComparison.OrdinalIgnoreCase))
                {
                   var check= musicFullPath.TryAdd(h, Path.GetFullPath(musics[i]));
                   
                }
            }
   
        }

        return musicFullPath;
    }

    /*Get only headers from input file*/
    private static Dictionary<int, string> GetItemHeaderHashFromFile(string[] arrayFilePath)
    {
        Dictionary<int, string> itemHeader = new();

        if (arrayFilePath is null || arrayFilePath.Length == 0) throw new ArgumentNullException("Array is null");

        for (int i = 0; i < arrayFilePath.Length; i++)
        {
            if (arrayFilePath[i].StartsWith('#')) itemHeader.Add(i, arrayFilePath[i]);
        }

        return itemHeader;
    }


}