using System.Diagnostics;
using Shared;


namespace Manager;

public static class FileManager
{

    public static async Task<(Dictionary<int, string>, Dictionary<int, string>)> ExtractData(string pathSource, string[]? videoPath, string[]? sameSymbol, params string[] CopyrightType) {

        string[] arrayOfListElementFromFile = new string[]{};

        if(sameSymbol is null) throw new ArgumentNullException(null, nameof(sameSymbol));
        if (!File.Exists(pathSource)) throw new FileNotFoundException();
        
        foreach (var item in videoPath.Where(item => !Directory.Exists(item))) System.Console.WriteLine($" \"{item}\" Don't Exist");

        try { arrayOfListElementFromFile = File.ReadAllLines(pathSource) ; } 
        catch (Exception) { throw; }

        var itemForHeader = GetItemHeaderHashFromFile(arrayOfListElementFromFile);
        var musicFullPath = GetFullPath(videoPath, arrayOfListElementFromFile, CopyrightType);

        return (itemForHeader, musicFullPath);
    }



    /// <summary>
    /// Move Files
    /// </summary>
    /// <param name="root"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static async Task MoveAsync(string root, string destination) {
        var sw = new Stopwatch();
        sw.Start();
        

        if(File.Exists(destination)) Console.WriteLine($"\"{destination}\" Already Exist");
        else {
            Console.Write($"\nmove of ---------------------- {root}    to-------------- {destination} \n");
            var task = Task.Run(Helpers.LoadSpinner);
            File.Move(root, destination);
            Console.Write("\r Done!" );

        }

        sw.Stop();
        sw.Restart();
    }

    /// <summary>
    /// Copy Files
    /// </summary>
    /// <param name="root"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static async  Task CopyAsync(string root, string destination) {
        var sw = new Stopwatch();
        sw.Start();
        
        if(! File.Exists(destination))
        {

            Console.Write($"\ncopy of ---------------------- {root}    \nto-------------- {destination} \n");

            var task = Task.Run(Helpers.LoadSpinner);
            File.Copy(root, destination);
            Console.Write("\r Done!");
            
        }else  Console.WriteLine($"\"{destination}\" Already Exist");

        sw.Stop();
        sw.Restart();
    }

    public static async Task ExecuteParallelCopyOrMoveAsync(string action, string rootPath, string destinationPath){
        
        if(action == "copy"){

            var task1 = CopyAsync(rootPath, destinationPath);
            await Task.Run(() =>task1);
            
        } else {

            var task1 = MoveAsync(rootPath, destinationPath);
            await Task.Run(() => task1);

        }
        
    }

    


    /// <summary>
    /// Manage Copy And Moving Files by Action
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task CopyOrMoveFileFromSourceFileAsync(SampleData data, params string[] CopyrightType)
    {
        string parent = "", child = "", parentFull = "",  path = "",  message = "\nFinished................👍", pathFull ="";
        
        if(data is {PathDestination: null, FileMultiPath: null}) System.Console.WriteLine("PathDestination OR FileMultiPath Was Not Set Correctly");
        
        if(! Directory.Exists(data.PathDestination)) Directory.CreateDirectory(data.PathDestination);

        var fileMatching = await GetRootDirectoryWithFileMatching(data.PathDestination, data.FileMultiPath);

        
        foreach (var (pathFile, pathSource) in fileMatching)
        {
            var (itemForHeader, musicFullPath) = await ExtractData(pathFile, data.VideoPath, data.SameSymbol, CopyrightType);

            Console.WriteLine($"\nPath is {pathFile}");
            int number = itemForHeader.Count; int breaker = 0;
            int musicLastKey =  musicFullPath.Keys.Last();
            int count = 0, counting = 1;
            //------------------
            foreach (var header in itemForHeader) {
                if(count < number && counting < number) {
                    var firstIndex = itemForHeader.ElementAt(count);
                    var lastIndex = itemForHeader.ElementAt(counting);
                    
                    int countMusic = firstIndex.Value.Where(x => x == '#').Count();
                    
                    if(countMusic == 3) {
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
                            path = Path.Combine(pathSource, pathFull);
                            try { if (!Directory.Exists(path)) Directory.CreateDirectory(path); }
                            catch (DirectoryNotFoundException) { throw; }

                            parentFull = Path.Combine(path, Path.GetFileName(musics.Value));
                            
                            if(data.Action.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync("copy", musics.Value, parentFull);
                            else await ExecuteParallelCopyOrMoveAsync("move", musics.Value, parentFull);
                        }
                    }

                    if(countMusic == 4) {
                        child = firstIndex.Value.Replace('#', ' ').Trim();
                        foreach (var musics in from KeyValuePair<int, string> musics in musicFullPath
                                            where firstIndex.Key < musics.Key && musics.Key < lastIndex.Key
                                            select musics)
                        {
                            path = Path.Combine(pathSource, pathFull, child);
                            try { if (!Directory.Exists(path)) Directory.CreateDirectory(path); }
                            catch (System.Exception) { throw; }

                            parentFull = Path.Combine(path, Path.GetFileName(musics.Value));
                            
                            if (data.Action?.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync("copy", musics.Value, parentFull);
                            else await ExecuteParallelCopyOrMoveAsync("move", musics.Value, parentFull);

                        }
                    }

                    if(countMusic== 5){ 
                        var SubChild = firstIndex.Value.Replace('#', ' ').Trim();
                        foreach (var musics in musicFullPath.Where(musics => firstIndex.Key < musics.Key && musics.Key < lastIndex.Key))
                        {
                            path = Path.Combine(pathSource, pathFull, child, SubChild);
                            try { if (!Directory.Exists(path)) Directory.CreateDirectory(path); }
                            catch (Exception) { throw; }

                            parentFull = Path.Combine(path, Path.GetFileName(musics.Value));
                            
                            if (data.Action?.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync("copy", musics.Value, parentFull);
                            else await ExecuteParallelCopyOrMoveAsync("move", musics.Value, parentFull);
                        }
                    }
                } 
            count++; counting++;
            } 
        }
        //--------------------

        await Task.Delay(100);
        Console.WriteLine(message);
    }

    /// <summary>
    /// Delete Directory And SubDirectories
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// Get Files List From A Directory
    /// </summary>
    /// <param name="folders"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<Dictionary<int, string>> GetFiles (Dictionary<int, string> folders){
        if(folders is null) throw new ArgumentNullException("Invalid Folders", $"{nameof(folders.Keys)} - {nameof(folders.Values)}");

        int counter = 0;
        List<string> fileLists = new();
        Dictionary<int, string> dict = new (); 

        await Task.Delay(100);
        
        foreach (var item in folders.Where(item => Directory.Exists(item.Value)))
        {
            var files = Directory.GetFiles(item.Value);
            if (files.Length == 0)
            {
                Console.WriteLine("The Folder dont contain Files");
            }
            else
            {
                foreach (var file in files)
                {
                    dict.Add(counter, file);
                    counter++;
                }
            }
        }

        return dict;
    }

    /// <summary>
    /// Get Directories, subDirectories and Includes Files 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="hideMessageg"></param>
    /// <param name="showFiles"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<Dictionary<int, string>> GetDirectories (string path, bool hideMessageg, bool showFiles){

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
            
            if(hideMessageg == false)
                Console.WriteLine($"\nThe directory contains {directories.Length} subFolders and {data.Count} Files \n ");

            return data;
        }

        foreach (var item in directories.Select((value, index) => new { value, index }))
        {
            if(showFiles == true)
                Console.WriteLine($" {item.value}");
            folders.Add(item.index, item.value);
        }
        Console.WriteLine(": \n");

        data = await GetFiles(folders);

        foreach (var item in data)
        {
            Console.WriteLine(item.Value);
        }

        if(hideMessageg == false)
            Console.WriteLine($"\nThe directory contains {directories.Length} subFolders and {data.Count} Files \n ");

        return folders;
    }

    /// <summary>
    /// Export Files And Directories If Exist To A Files
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task ExportPathToFileAsync(SampleData data){
         string indexHeader; 

        if(data is {EmbeedPath : null, EmbeedDestination: null, EmbeedFileName: null} ) 
            throw new ArgumentNullException(nameof(data));  

        var (type, dataFormat) = await FormatPath(data);
        
        try
        {
            TimeZoneInfo parisTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            DateTime parisTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, parisTimeZone);
            
            string timer = parisTime.ToString("hh:mm").Replace(":","_");
            string file = string.Concat(parisTime.ToString("dd-MM-yyyy") + $"_{timer}", $"_{data.EmbeedFileName}");
            string fileDestination = Path.Combine(data.EmbeedDestination, file);   
            
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
                Console.WriteLine("The Folder is empty OR Dont Exist 😅 \n");
            }

        }
        catch (Exception) { throw; }
    }


    /// <summary>
    /// Format A List By Files If A Directory Dont Include SubDirectory Or Return List Of  SubDirectories
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<(string, Dictionary<string, List<string>>)> FormatPath(SampleData data){
        string type ="";
        if(data is {EmbeedPath : null, EmbeedDestination: null} ) 
            throw new ArgumentNullException(nameof(data));  
        
        var fileItems = await GetFilesListFromSubFolders(data); 

        Dictionary<string, List<string>> dict = new();

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
                
                foreach (var file in fullPath.Where(file => File.Exists(file)))
                {
                    if (data.EmbeedTypeShort == true) elements.Add(Path.GetFileNameWithoutExtension(file));
                    else elements.Add(file);
                }

                type ="folder";
                dict.Add(item.Replace(data.EmbeedPath+"\\",""), elements);
            }            
        }

        return (type, dict);
    }

    /// <summary>
    /// Get Files List From a Directory If Exist Or Retun A list Of Directory
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<List<string>> GetFilesListFromSubFolders(SampleData data)
    {
        if(data is { EmbeedDestination: null, EmbeedPath: null} ) 
            throw new ArgumentNullException(nameof(data));  
       
        DirectoryInfo[] directories;
        List<string> fileItems = new();

        try
        {
            if(!Directory.Exists(data.EmbeedPath)) return new List<string>(){};
            if (!Directory.Exists(data.EmbeedDestination)) Directory.CreateDirectory(data.EmbeedDestination);
            
            var directory = new DirectoryInfo(data.EmbeedPath);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            
            if(directories.Length == 0) {
                var fileCollections = Directory.GetFiles(data.EmbeedPath);
                if(fileCollections.Length == 0) { return new List<string>{"No data Available"}; }
                fileItems.AddRange(fileCollections);
            }
            else {
                await Task.Delay(100);
                fileItems.AddRange(from DirectoryInfo? item in directories
                                   select item.FullName);
            }

        }
        catch (UnauthorizedAccessException) {throw;}
        catch (DirectoryNotFoundException) { throw; }
        
        return fileItems;
    }

    /// <summary>
    /// Get A List Of Item In A Specific Folder When Items Match Each Other
    /// Compare A MD File Which contain Elements with A Directory Which contain Videos OR Audios
    /// </summary>
    /// <param name="videoDirectory"></param>
    /// <param name="arrayOfFullPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Dictionary<int, string> GetFullPath(string[] videoDirectory, string[] arrayOfListElementFromFile, params string[] CopyrightType)
    {
        if (videoDirectory is null) throw new ArgumentNullException(); //new Tuple<string, Dictionary<int, string>>("", new Dictionary<int, string>);
        
        Dictionary<int, string> musicFullPath = new();

        List<string> list = new List<string>(), musics =new List<string>();
        
        foreach (var item in videoDirectory.Where(item => !Directory.Exists(item))) throw new DirectoryNotFoundException(item);
        
        foreach (var data in from item in videoDirectory.ToList()
                             let data = Directory.GetFiles(item)
                             where data.Length > 0
                             select data)
        {
            musics.AddRange(data);
        }

        for (int h = 0; h < arrayOfListElementFromFile.Length; h++)
        {
            for (int i = 0; i < musics.Count; i++)
            {
                string extension = Path.GetFileNameWithoutExtension(musics[i]);
                
                if (arrayOfListElementFromFile[h].Contains(extension, StringComparison.OrdinalIgnoreCase))
                {
                   musicFullPath.TryAdd(h, Path.GetFullPath(musics[i]));
                }
            }
        }

        return musicFullPath;
    }

    /// <summary>
    /// Get Only Headers And Index From Input File
    /// </summary>
    /// <param name="arrayFilePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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