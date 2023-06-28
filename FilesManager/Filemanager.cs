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

        if(data.SameSymbol is null) throw new ArgumentNullException(null, nameof(data.SameSymbol));
        if (!File.Exists(FilePath)) throw new FileNotFoundException();

        foreach (var item in data.VideoPath) {
            if(!Directory.Exists(item)) throw new DirectoryNotFoundException(item);
        }

        try { arrayOfListElementFromFile = File.ReadAllLines(FilePath) ; } 
        catch (Exception) { throw; }

        var itemForHeader = GetItemHeaderHashFromFile(arrayOfListElementFromFile);

        var musicFullPath = GetFullPath(data.VideoPath, arrayOfListElementFromFile);

        return (itemForHeader, musicFullPath);
    }

    public static async Task CopyOrMoveFileFromSourceFileAsync(SampleData data)
    {
        string parent = "", child = "", parentFull = "",  path = "",  message = "\nFinished................👍", pathFull ="";
        if(data is {PathDestination: null}) throw new ArgumentNullException();
        
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
                        
                        if(data.SameSymbol.Length != 0 ){
                            if(data.SameSymbol.Any(symbol => parent.Contains(symbol, StringComparison.OrdinalIgnoreCase))) {
                                pathFull = Path.Combine("FR", parent);
                            }
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
                               
                                if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                else {
                                    Console.WriteLine($"copy of ---------------------- {musics.Value}");
                                    File.Copy(musics.Value, parentFull);
                                    Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                }
                                 
                            }else{

                                if(!File.Exists(parentFull)) {
                                    Console.WriteLine($"move of ---------------------- {musics.Value}");
                                    File.Move(musics.Value, parentFull);
                                    Console.WriteLine("moving  to--------------{0}", parentFull);

                                    }
                                else Console.WriteLine($"\"{parentFull}\" Already Exist");
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

                                    if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                    else {
                                        Console.WriteLine($"copy of ---------------------- {musics.Value}");
                                        File.Copy(musics.Value, parentFull);
                                        Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                    }   

                                }
                                else {

                                    if(!File.Exists(parentFull)) {
                                        Console.WriteLine($"move of ---------------------- {musics.Value}");
                                        File.Move(musics.Value, parentFull);
                                        Console.WriteLine("moving  to--------------{0}", parentFull);
                                    }
                                    else  Console.WriteLine($"\"{parentFull}\" Already Exist");

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

                                    if(File.Exists(parentFull)) Console.WriteLine($"\"{parentFull}\" Already Exist");
                                    else {
                                        Console.WriteLine($"copy of ---------------------- {musics.Value}");
                                        File.Copy(musics.Value, parentFull); 
                                        Console.WriteLine("copying  to--------------{0} in {1} 's", parentFull, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                    }  
                                    
                                 }else {

                                    if(!File.Exists(parentFull)) {
                                        Console.WriteLine($"move of ---------------------- {musics.Value}");
                                        File.Move(musics.Value, parentFull);
                                        Console.WriteLine("moving  to--------------{0}", parentFull);
                                    } 
                                    else Console.WriteLine($"\"{parentFull}\" Already Exist");
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

    public static async Task DeleteDirectory(string path){

        if(path is null ) throw new ArgumentNullException(nameof(path));
        // _ = GetDirectories(path, true, true);

        await Task.Delay(500);

        if(Directory.Exists(path)){
            Directory.Delete(path, true);
            Console.WriteLine("Folder remove successfully❗");

        }
        else Console.WriteLine("The Folder u're trying to remove dont exist 🙂");

    }

    public static async Task<Dictionary<int, string>> GetFiles (Dictionary<int, string> folders){
        if(folders is null) throw new ArgumentNullException("Invalid Folders", $"{nameof(folders.Keys)} - {nameof(folders.Values)}");

        int counter = 0;
        List<string> fileLists = new();
        Dictionary<int, string> dict = new (); 

        await Task.Delay(100);

        foreach (var item in folders)
        {
            if(Directory.Exists(item.Value)) {
                
                var files =  Directory.GetFiles(item.Value);

                if(files.Length == 0) {
                    Console.WriteLine("The Folder dont contain Files");
                }
                else{

                    foreach (var file in files)
                    {
                        dict.Add(counter, file);
                        //Console.WriteLine(file);
                        counter++;
                    }
                }
            }
        }

        return dict;
    }

    public static async Task<Dictionary<int, string>> GetDirectories (string path, bool hideInfo, bool showFiles){

        Dictionary<int, string> folders = new();

        if(path is null) throw new ArgumentNullException("invalid Path", nameof(path));

        if(!Directory.Exists(path)) {
            folders.Add(0, "The Directory Dont Exist");
            return folders;
        }

        var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);

        Dictionary<int, string> data = new();
        var files = Directory.GetFiles(path, "*"); 

        if(directories.Length == 0) {

            folders.Add(0, path);
            data = await GetFiles(folders);

            if(showFiles == true)
                foreach (var item in data) Console.WriteLine(item.Value);
            
            if(hideInfo == false)
                Console.WriteLine($"\nThe directory contains {directories.Length} subFolders and {data.Count} Files \n ");

            return data;
        } 
            
        foreach (var item in directories.Select((value, index) => new {value, index}))
        {
            if(showFiles == true)
                Console.WriteLine($" {item.value}");
            folders.Add(item.index, item.value);
        }
        Console.WriteLine(": \n");

            //return folders;
        data = await GetFiles(folders);

        foreach (var item in data)
        {
            Console.WriteLine(item.Value);
        }

        if(hideInfo == false)
            Console.WriteLine($"\nThe directory contains {directories.Length} subFolders and {data.Count} Files \n ");

        return folders;
    }

    public static async Task ExportEmbeedPathToFileAsync(SampleData data){
         string indexHeader; 

        if(data is {EmbeedPath : null, EmbeedDestination: null, EmbeedFileName: null} ) 
            throw new ArgumentNullException(nameof(data));  

        var (type, dataFormat) = await FormatEmbeedPath(data);
        
        try
        {
            TimeZoneInfo parisTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            DateTime parisTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, parisTimeZone);
            
            string timer = parisTime.ToString("hh:mm").Replace(":","_");
            string file = string.Concat(parisTime.ToString("dd-MM-yyyy") + $"_{timer}", $"_{data.EmbeedFileName}");
            string fileDestination = Path.Combine(data.EmbeedDestination, file);   
            
            Console.WriteLine($"{fileDestination} \n");

            if(dataFormat.Count != 0){
                using StreamWriter sw = new (fileDestination);

                sw.WriteLine("----------------{0}-----------------\n", parisTime.ToString("dd-MM-yyyy hh:mm:ss"));

                foreach (var (header, value) in dataFormat)
                {
                    indexHeader = string.Concat("###", header);

                    if(type == "file") {
                        indexHeader = header; 

                        sw.WriteLine(value.FirstOrDefault());
                        Console.WriteLine( $"\"{value.FirstOrDefault()}\" Added successfully");
                        
                    }else {
                        sw.WriteLine($"{indexHeader} \n");

                        foreach (var item in value)
                        {
                            sw.WriteLine(item);
                            Console.WriteLine( $"\"{item}\" Added successfully");
                        }
                        sw.WriteLine("");

                    }
                    
                }
                sw.WriteLine("");
                Console.WriteLine($"\nTexts added to: \"{fileDestination}\" successfully -------------------------- 👍\n");

            } else {
                System.Console.WriteLine("The Folder is empty 😅 \n");
            }

        }
        catch (Exception) { throw; }
    }
    public static async Task<(string, Dictionary<string, List<string>>)> FormatEmbeedPath(SampleData data){
        string type ="";
        if(data is {EmbeedPath : null, EmbeedDestination: null} ) 
            throw new ArgumentNullException(nameof(data));  
        
        var filecombined = Path.Combine(data.EmbeedDestination, data.FileDestination); 
        var fileItems = await GetFilesListFromSubFolders(data); string header;

        Dictionary<string, List<string>> dict = new();
        // foreach (var file in fileItems){
        //     if(File.Exists(file)) Console.WriteLine(file);
        //     else System.Console.WriteLine("Dont exist");
        // }

        foreach (var item in fileItems)
        {
            if(File.Exists(item)) {
                var elements = new List<string>();
            
                if(data.EmbeedTypeShort == true)  elements.Add(Path.GetFileNameWithoutExtension(item));
                else elements.Add(item); 
                
                type="file";
                dict.Add(item.Replace(data.EmbeedPath+"\\",""), elements);
            }

            if(Directory.Exists(item)) {
                var fullPath = Directory.GetFiles(item);
                var elements = new List<string>();
            
                foreach (var file in fullPath)
                {
                    if(File.Exists(file) ){
                        if(data.EmbeedTypeShort == true)  elements.Add(Path.GetFileNameWithoutExtension(file));
                        else elements.Add(file); 
                    }

                }
                type="folder";
                dict.Add(item.Replace(data.EmbeedPath+"\\",""), elements);
            }            
            
        }

        return (type, dict);
        
    }

    public static async Task<List<string>> GetFilesListFromSubFolders(SampleData data)
    {
        if(data is { EmbeedDestination: null, EmbeedPath: null} ) 
            throw new ArgumentNullException(nameof(data));  
       
        DirectoryInfo[] directories;
        List<string> fileItems = new();

        try
        {
            if(!Directory.Exists(data.EmbeedPath)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(data.EmbeedDestination)) Directory.CreateDirectory(data.EmbeedDestination);
            
            var directory = new DirectoryInfo(data.EmbeedPath);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            
            if(directories.Length == 0) {
                var fileCollections = Directory.GetFiles(data.EmbeedPath);
                if(fileCollections.Length == 0) { return new List<string>{"No data Available"}; }
                
                foreach (var item in fileCollections)
                {
                    fileItems.Add(item);
                }

            }else {
                await Task.Delay(100);

                foreach (DirectoryInfo? item in directories)
                {
                    fileItems.Add(item.FullName);
                }
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