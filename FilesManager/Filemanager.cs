using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Shared;


namespace Manager;

public static class FileManager
{

    public static async Task<(Dictionary<int, string>, Dictionary<int, string>)> ExtractData(SampleData data) {

        string FilePath = Path.Combine(data.PathSource, data.FilePath);
        string[] arrayOfListElementFromFile = new string[]{};

        if(data.SameSymbol is null) throw new ArgumentNullException("SameSymbol is null");

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
        string parent = "", child = "", parentFull = "",  path = "",  message = "Finished................👍", pathFull ="";
        if(data is {PathDestination: null, SameSymbol : null}) throw new ArgumentNullException();
        List<string> musicsToExport = new();
        string[] arrayOfListElementFromFile = Array.Empty<string>();

        if(! Directory.Exists(data.PathDestination)) Directory.CreateDirectory(data.PathDestination);

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
                
                switch(countMusic){
                    case 3: 
                        parent = firstIndex.Value.Replace('#', ' ').Trim();
                        pathFull = parent;
                        
                        if(data.SameSymbol.Any(symbol => parent.Contains(symbol, StringComparison.OrdinalIgnoreCase))) {
                            pathFull = Path.Combine("FR", parent);
                        }

                        foreach (var musics in from musics in musicFullPath
                                               where firstIndex.Key < musics.Key && musics.Key < lastIndex.Key
                                               select musics)
                        {
                            path = Path.Combine(data.PathDestination, pathFull);
                            try { if (!Directory.Exists(path)) Directory.CreateDirectory(path); }
                            catch (DirectoryNotFoundException) { throw; }

                            parentFull = Path.Combine(path, Path.GetFileName(musics.Value));
                            
                            sw.Start();
                            if(data.Action.ToLower() == "copy"){
                                Console.WriteLine($"copy of ---------------------- {musics.Value}");
                               
                                if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                else File.Copy(musics.Value, parentFull);
                                
                                Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                 
                            }else{
                                Console.WriteLine($"move of ---------------------- {musics.Value}");

                                if(!File.Exists(parentFull)) File.Move(musics.Value, parentFull);
                                else Console.WriteLine($"\"{parentFull}\" Already Exist");

                                Console.WriteLine("moving  to--------------{0}", parentFull);

                            }
                            
                            sw.Stop();
                            sw.Restart();
                        }

                        break;
                    case 4: 
                        child = firstIndex.Value.Replace('#', ' ').Trim();

                        foreach (KeyValuePair<int, string> musics in musicFullPath)
                        {
                            
                            if(firstIndex.Key < musics.Key && musics.Key < lastIndex.Key){
                                path = Path.Combine(data.PathDestination, pathFull, child);

                                 try{ if(!Directory.Exists(path)) Directory.CreateDirectory(path); }
                                 catch (System.Exception) {throw;}

                                parentFull = Path.Combine(path, Path.GetFileName(musics.Value));
                                                                 
                                sw.Start();
                                if(data.Action.ToLower() == "copy")
                                {
                                    Console.WriteLine($"copy of ---------------------- {musics.Value}");

                                    if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                    else File.Copy(musics.Value, parentFull);   

                                    Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));

                                }
                                else {
                                    Console.WriteLine($"move of ---------------------- {musics.Value}");

                                    if(!File.Exists(parentFull)) File.Move(musics.Value, parentFull);
                                    else  Console.WriteLine($"\"{parentFull}\" Already Exist");

                                    Console.WriteLine("moving  to--------------{0}", parentFull);

                                }
                                sw.Stop();
                                sw.Restart();
                            }
                        }
                        break;
                    case 5: 
                        var SubChild = firstIndex.Value.Replace('#', ' ').Trim();

                        foreach (KeyValuePair<int, string> musics in musicFullPath)
                        {
                            if(firstIndex.Key < musics.Key && musics.Key < lastIndex.Key){
                                path = Path.Combine(data.PathDestination, pathFull, child, SubChild);

                                try{ if(!Directory.Exists(path)) Directory.CreateDirectory(path); }
                                catch (Exception) {throw;}
                        
                                parentFull = Path.Combine(path, Path.GetFileName(musics.Value));

                                sw.Start();
                                 if(data.Action.ToLower() == "copy") {
                                    Console.WriteLine($"copy of ---------------------- {musics.Value}");

                                    if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                    else File.Copy(musics.Value, parentFull);   
                                    
                                    Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                    
                                 }else {
                                    Console.WriteLine($"move of ---------------------- {musics.Value}");

                                    if(!File.Exists(parentFull)) File.Move(musics.Value, parentFull);
                                    else Console.WriteLine($"\"{parentFull}\" Already Exist");

                                    Console.WriteLine("moving  to--------------{0}", parentFull);

                                 }

                                sw.Stop();
                                sw.Restart();
                            }
                                
                        }
                        break;
                    default: break;
                } 


            }
            count++; counting++;
            
        }

        await Task.Delay(100);
        Console.WriteLine(message);
    }

    public static async Task ExportEmbeedPathToFileAsync(SampleData data){
         string indexHeader = ""; 

        if(data is {EmbeedPath : null, EmbeedDestination: null, EmbeedFileName: null} ) 
            throw new ArgumentNullException(nameof(data));  

        var dataFormat = await FormatEmbeedPath(data);
        
        try
        {
            string FileDestination = Path.Combine(data.EmbeedDestination, data.EmbeedFileName);
            
            using StreamWriter sw = new (FileDestination);
            sw.WriteLine("");
            sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
            
            foreach (var (header, value) in dataFormat)
            {
                indexHeader = string.Concat("###", header);
                
                sw.WriteLine(indexHeader);
                sw.WriteLine("");
                
                foreach (var item in value)
                {
                    Console.WriteLine( $"\"{item}\" Added successfully");
                    // Thread.Sleep(5);
                    sw.WriteLine(item);
                }
                
                sw.WriteLine("");
            }

            Console.WriteLine($"\nTexts added to: {FileDestination} successfully -------------------------- 👍");
        }
        catch (Exception) { throw; }
    }
    public static async Task<Dictionary<string, List<string>>> FormatEmbeedPath(SampleData data){
                
        if(data is {EmbeedPath : null, EmbeedDestination: null} ) 
            throw new ArgumentNullException(nameof(data));  
        
        var filecombined = Path.Combine(data.EmbeedDestination, data.FileDestination);
        var fileItems = await GetFilesListFromSubFolders(data); string header;

        Dictionary<string, List<string>> dict = new();

         foreach (var item in fileItems)
         {
            var fullPath = Directory.GetFiles(item);
            var elements = new List<string>();
            string message =""; 
            foreach (var file in fullPath)
            {
                // header = item
                if(data.EmbeedTypeShort == true)  elements.Add(Path.GetFileName(file));
                else elements.Add(file); ;
            }
            
            dict.Add(item.Replace(data.EmbeedPath+"\\",""), elements);
             
         }

        return dict;
        
    }

    public static async Task<List<string>> GetFilesListFromSubFolders(SampleData data)
    {
        if(data is { EmbeedDestination: null, EmbeedPath: null} ) 
            throw new ArgumentNullException(nameof(data));  
       
        DirectoryInfo[] directories;
        List<string> fileItems = new();

        try
        {
            var directory = new DirectoryInfo(data.EmbeedPath);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            //root= Directory.GetDirectories(mainPath,"*", searchOption: SearchOption.AllDirectories);
            if (!Directory.Exists(data.EmbeedDestination)) Directory.CreateDirectory(data.EmbeedDestination);

            await Task.Delay(100);

            foreach (DirectoryInfo? item in directories)
            {
                fileItems.Add(item.FullName);
            }
        }
        catch (UnauthorizedAccessException) {throw;}
        catch (DirectoryNotFoundException) { throw; }
        

        return fileItems;
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