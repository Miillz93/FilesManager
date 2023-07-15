using System.Diagnostics;
using Shared;
using System.Linq;
using System.Runtime.InteropServices;

namespace Manager;

public static class FileManager
{

    public static async Task<(Dictionary<int, string>, Dictionary<int, string>)> ExtractData(string pathSource, string[]? videoPath, string[]? sameSymbol, params string[] CopyrightType) {

        string[] arrayOfListElementFromFile = new string[]{};

        if(sameSymbol is null) return new();
        if (!File.Exists(pathSource)) throw new FileNotFoundException();
        
        foreach (var item in videoPath.Where(item => !Directory.Exists(item))) Console.WriteLine($" \"{item}\" Don't Exist");

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
        
        var ts = new CancellationTokenSource();
        var token = ts.Token;

        await Task.Delay(100);
        
        if(File.Exists(destination)) Console.WriteLine($"\"{destination}\" Already Exist");
        else {
            Console.Write($"\nmove of ---------------------- {root}    to-------------- {destination} \n");
            // var t = new Thread(new ThreadStart(Helpers.LoadSpinner));
            var task = Task.Run(() => Helpers.LoadSpinner(token));
            
            File.Move(root, destination);
            // Thread.Sleep(100);
            Console.Write("\r Done!" );

        }
        ts.Cancel();

    }

    /// <summary>
    /// Copy Files
    /// </summary>
    /// <param name="root"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static async  Task CopyAsync(string root, string destination) {

        var ts = new CancellationTokenSource();
        var token = ts.Token;
        
        if(!File.Exists(destination))
        {
            Console.Write($"\ncopy of ---------------------- {root}    \nto-------------- {destination} \n");
            await Task.Delay(10);
            
             var task = Task.Run(() => Helpers.LoadSpinner(token));
            File.Copy(root, destination);
            Console.Write("\r Done!");
            
        }else  Console.WriteLine($"\"{destination}\" Already Exist");
        ts.Cancel();

    }

    public static async  Task MoveAsync(List<string> elements, string destination) 
    {
        int number = 1;
        var ts = new CancellationTokenSource();
        var token = ts.Token;
        
        if(elements.Count == 0 ) 
        {   
            Console.WriteLine("LIMIT REACH 😨");
            Thread.Sleep(2500);

            return;
        } else {
            
            foreach (var item in elements)
            {
                string filename = Path.GetFileName(item);
                
                string dest = Path.Combine(destination, filename);
                Console.WriteLine($"\r \"{filename}\" Moving Correctly");
                Console.WriteLine("----------------------------------------");
                var task = Task.Run(() => Helpers.LoadSpinner(token));
                File.Move(item, dest, false);
                Console.Write("\r Done! \n");
                Console.WriteLine("----------------------------------------");
                number++;
            }
        }

        ts.Cancel();

    }

    public static async  Task CopyAsync(List<string> elements, string destination) 
    {
        int number = 1;
        var ts = new CancellationTokenSource();
        var token = ts.Token;
        
        if(elements.Count == 0 ) 
        {   
            Console.WriteLine("LIMIT REACH 😨");
            Thread.Sleep(2500);

            return;
        } else {
            
            foreach (var item in elements)
            {
                string filename = Path.GetFileName(item);
                
                string dest = Path.Combine(destination, filename);
                Console.WriteLine($"\r \"{filename}\" Is Being Created At N° {number}");
                Console.WriteLine("----------------------------------------");
                var task = Task.Run(() => Helpers.LoadSpinner(token));
                File.Copy(item, dest);
                Console.Write("\r Done! \n");
                Console.WriteLine("----------------------------------------");
                number++;
            }
        }

        ts.Cancel();

    }

    public static async Task ExecuteParallelCopyOrMoveAsync(string rootPath, string destinationPath, string action){
        
        if(action == "copy"){

            var task1 = CopyAsync(rootPath, destinationPath);
            await Task.Run(() =>task1);
        } else {

            var task1 = MoveAsync(rootPath, destinationPath);
            await Task.Run(() => task1);
        }
        
    }

    public static async Task<bool> GenerateDirectory(string path) {
        string[]? folders = new string[]{"JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC"};
        bool success = false;
        await Task.Delay(100);
        
        foreach (var folder in folders)
        {
            string folderDated = string.Concat(folder,"_",DateTime.Now.Year);
            if(!Directory.Exists(Path.Combine(path, folderDated))) {
                Directory.CreateDirectory(Path.Combine(path, folderDated));
                success = true;
            }
            // else System.Console.WriteLine($"Directory Already contains {Path.Combine(pathDestination, folderDated)}");
        }

        return success;
    }

    /// <summary>
    /// Create Destination Directories if Don't Exist
    /// </summary>
    /// <param name="pathDestination"></param>
    /// <param name="fileMultiPath"></param>
    /// <returns></returns>
    public static async Task<Dictionary<string, string>> GetRootDirectoryWithFileMatching(string pathDestination, string[] fileMultiPath){

        Dictionary<string, string> fileMatch = new();

        _ = await GenerateDirectory(pathDestination);

        foreach (var mdFile in fileMultiPath)
        {
            string filter = mdFile.Substring(mdFile.LastIndexOf("_") + 1,3);

            foreach (var folderDestination in Directory.GetDirectories(pathDestination, "*", SearchOption.TopDirectoryOnly))
            {
                if(folderDestination.Contains(filter, StringComparison.OrdinalIgnoreCase)) 
                {
                    fileMatch.Add(mdFile, folderDestination);
                }
            }
        }

        return fileMatch;
    }


    /// <summary>
    /// Manage Copy And Moving Files by Action
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task CopyOrMoveFileFromSourceFileAsync(string pathDestination, string[] fileMultiPath, string[]? videoPath, string[]? sameSymbol, string action)
    {
        string parent = "", child = "", parentFull = "",  path = "",  message = "\nFinished................👍", pathFull ="";
        var fileMatching = new Dictionary<string, string>();

        if(pathDestination is null ^ fileMultiPath is null) Console.WriteLine("PathDestination OR FileMultiPath Was Not Set Correctly");
        
        if(! Directory.Exists(pathDestination)) Directory.CreateDirectory(pathDestination);
        
        if(fileMultiPath != null)
            fileMatching = await GetRootDirectoryWithFileMatching(pathDestination, fileMultiPath);

        foreach (var (pathFile, pathSource) in fileMatching)
        {
            var (itemForHeader, musicFullPath) = await ExtractData(pathFile, videoPath, sameSymbol);

            Console.WriteLine($"\nPath is {pathFile}");
            int number = itemForHeader.Count;
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
                        
                        if(sameSymbol.Length != 0 ){
                            if(sameSymbol.Any(symbol => parent.Contains(symbol, StringComparison.OrdinalIgnoreCase))) {
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
                            
                            if(action.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "copy");
                            else await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "move");
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
                            
                            if (action.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "copy");
                            else await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "move");

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
                            
                            if (action.ToLower() == "copy") 
                                await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "copy");
                            else await ExecuteParallelCopyOrMoveAsync(musics.Value, parentFull, "move");
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
        if(folders is null) return new();

        int counter = 0;
        List<string> fileLists = new();
        var dict = new Dictionary<int, string>(); 

        await Task.Delay(10);
        
        foreach (var item in folders.Where(item => Directory.Exists(item.Value)))
        {
            var files = Directory.GetFiles(item.Value);
            if (files.Length != 0)
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

    public static async Task CreateDocument(string path){
        
        if(!File.Exists(path)) {
            await CreateDirectory(Path.GetDirectoryName(path) ?? "");
            var file = File.Create(path);
            file.Close();
        }
     }

     public static async Task CreateDocuments(string logPath){
        await CreateDirectory(logPath);
        await Task.Delay(10);
        if(!File.Exists(Path.Combine(logPath, "playlist_copyright.md"))){ 
            var file = File.Create(Path.Combine(logPath, "playlist_copyright.md"));
            file.Close();
        }
        if(!File.Exists(Path.Combine(logPath, "playlist_copycat.md"))) 
        {
            var file = File.Create(Path.Combine(logPath, "playlist_copycat.md"));
            file.Close();
        }
        if(!File.Exists(Path.Combine(logPath, "tracklist_copyright.md"))) {
            var file = File.Create(Path.Combine(logPath, "tracklist_copyright.md"));
            file.Close();
        }
        if(!File.Exists(Path.Combine(logPath, "'tracklist_copycat.md"))) 
        {
            var file = File.Create(Path.Combine(logPath, "tracklist_copycat.md"));
            file.Close();
        }
      
    }

    /// <summary>
    /// Get Directories, subDirectories and Includes Files 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="hideMessageg"></param>
    /// <param name="showFiles"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<Dictionary<int, string>> GetDirectories (string path, bool hideMessage, bool showFiles){

        Dictionary<int, string> folders = new(); List<string> directories = new();

        if(path is null ^ !Directory.Exists(path)) return folders;

        try {
            var options = new EnumerationOptions {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true
        };

        directories = Directory.EnumerateDirectories(path, "*", options).ToList();
        } catch(UnauthorizedAccessException e) {
            Console.WriteLine("Directory {0} Due To Admin Priviledge", e.Source);
        }
 

        Dictionary<int, string> data = new();
        var files = Directory.GetFiles(path, "*"); 

        if(directories.Count == 0) {

            folders.Add(0, path);
            data = await GetFiles(folders);

            if(showFiles == true)
                foreach (var item in data) Console.WriteLine(item.Value);
            
            if(hideMessage == false) 
                Console.WriteLine($"\nThe directory contains {directories.Count} subFolders and {data.Count} Files \n ");

            return data;
        }

        foreach (var item in directories.Select((value, index) => new { value, index }))
        {
            if(showFiles == true) Console.WriteLine($" {item.value}");
            folders.Add(item.index, item.value);
        }

        data = await GetFiles(folders);

        foreach (var item in data)
        {
           if(showFiles == true) Console.WriteLine(item.Value);
        }

        if(hideMessage == false)
            Console.WriteLine($"\nThe directory contains {directories.Count} subFolders and {data.Count} Files \n ");

        return folders;
    }

    public static async Task<List<string>> GetDirectories(string path)
    {
        var listing =  new List<string>();
        await CreateDirectory(path);
        var directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        if(directories.Length == 0) return new();
        foreach (var item in directories)
        {
            listing.Add(item);
        }

        return listing;
    }

    /// <summary>
    /// Export Files And Directories If Exist To A Files
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task ExportPathToDocumentAsync(string path, string destination, string fileName)
    {
         string indexHeader; 

        if(path is null ^ destination is null ^ fileName is null) return;  

        var (type, dataFormat) = await FormatPath(path, destination, true);
        
        try
        {
            TimeZoneInfo parisTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            DateTime parisTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, parisTimeZone);
            
            string timer = parisTime.ToString("hh:mm").Replace(":","_");
            string file = string.Concat(parisTime.ToString("dd-MM-yyyy") + $"_{timer}", $"_{fileName}");
            string fileDestination = Path.Combine(destination, file);   
            
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

    public static async Task ExportPathToDocumentAsync(string path, List<string> elements)
    {
        // await WriteToDocument(elements, path);
        if(elements is null ^ path is null) return;

        if(File.ReadAllLines(path).Length == 0) {
            using StreamWriter sw = new(path);
            foreach (var item in elements)
            {
                sw.WriteLine(item);

            }
        }else File.AppendAllLines(path, elements);
        
        Thread.Sleep(1000);        
        Console.WriteLine($"data added to \"{path}\" successfully 👍");

    }
    public static async Task ExportPathToDocumentAsync(string path, string element)
        {
            if(element is null ^ path is null) return;

            if(File.ReadAllLines(path ??"").Length == 0) {
                using StreamWriter sw = File.CreateText(path ??"");
                await sw.WriteLineAsync(element);
                // Console.WriteLine($"\"{element}\" added to \"{path}\" successfully 👍");


            }else {
                using StreamWriter sw = File.AppendText(path ?? "");
                await sw.WriteLineAsync(element);
                // Console.WriteLine($"\"{element}\" added to \"{path}\" successfully 👍");
            }
            
            Thread.Sleep(300);        

        }

    /// <summary>
    /// Format A List By Files If A Directory Dont Include SubDirectory Or Return List Of  SubDirectories
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<(string, Dictionary<string, List<string>>)> FormatPath(string embeedPath, string embeedDestination, bool embeedTypeShort){
        string type ="";
        if(embeedPath is null ^ embeedDestination is null ) return new();
        
        var fileItems = await GetFilesListFromSubFolders(embeedDestination, embeedPath); 

        var dict = new Dictionary<string, List<string>>();

        foreach (var item in fileItems)
        {
            if(File.Exists(item)) {
                var elements = new List<string>();
            
                if(embeedTypeShort == true)  elements.Add(Path.GetFileNameWithoutExtension(item));
                else elements.Add(item); 
                
                type="file";
                dict.Add(item.Replace(embeedPath+"\\",""), elements);
            }

            if(Directory.Exists(item)) {
                var fullPath = Directory.GetFiles(item);
                var elements = new List<string>();
                
                foreach (var file in fullPath.Where(file => File.Exists(file)))
                {
                    if (embeedTypeShort == true) elements.Add(Path.GetFileNameWithoutExtension(file));
                    else elements.Add(file);
                }

                type ="folder";
                dict.Add(item.Replace(embeedPath+"\\",""), elements);
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
    public static async Task<List<string>> GetFilesListFromSubFolders(string embeedDestination, string embeedPath)
    {
        if(embeedDestination is null ^ embeedPath is null ) return new();
       
        DirectoryInfo[] directories;
        List<string> fileItems = new();

        try
        {
            if(!Directory.Exists(embeedPath)) return new List<string>(){};
            if (!Directory.Exists(embeedDestination)) Directory.CreateDirectory(embeedDestination);
            
            var directory = new DirectoryInfo(embeedPath);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            
            if(directories.Length == 0) {
                var fileCollections = Directory.GetFiles(embeedPath);
                if(fileCollections.Length == 0) return new();
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
        if (videoDirectory is null) return new(); 

        Dictionary<int, string> musicFullPath = new();

        List<string> list = new List<string>(), musics =new List<string>();
        
        foreach (var item in videoDirectory.Where(item => !Directory.Exists(item)))Console.WriteLine($"\"{item}\" Don't Exist");
        
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

        if (arrayFilePath is null || arrayFilePath.Length == 0) return new();

        for (int i = 0; i < arrayFilePath.Length; i++)
        {
            if (arrayFilePath[i].StartsWith('#')) itemHeader.Add(i, arrayFilePath[i]);
        }

        return itemHeader;
    }

     /// <summary>
    /// Looking for a file with a specific name
    /// </summary>
    /// <param name="logPathDestination"></param>
    /// <param name="copyType"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetFilesWithSpecificInfoAsync(string logPathDestination, params string[] copyType)
    {
        var fileInfo = new List<string>();
                
        if(logPathDestination is null ^ copyType is null) return new();

        foreach (var item in copyType)
        {
            var directoryInfo = Directory.GetFiles(logPathDestination, $"*{item}*", SearchOption.AllDirectories);
            
            if(directoryInfo is null) return new();
            fileInfo.AddRange(directoryInfo);
        }

        return fileInfo;
    }

    public static async Task<string> CreateDocument(string path, string subdirectory, [Optional] string origin)
    {

        if(subdirectory is null) return string.Empty;
        
        // listing.Add(Path.Combine(path, subdirectory));
        string newPath = await FileManager.CreateDirectory(path, subdirectory ?? "", 2, origin);
        // listing.Add(newPath);
        string newPathWithDocument = Path.Combine(newPath, subdirectory + ".md" ?? "");
        ;

        if(!File.Exists(newPathWithDocument)) {
            var file  = File.Create(newPathWithDocument);
            file.Close();
        }

        return newPathWithDocument;
    }

    public static async Task CreateDirectory(string path) {
        
        if(path is null) return;                
        if(!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

     /// <summary>
    /// Create A Directory For Playlist logs
    /// </summary>
    /// <param name="logPathDestination"></param>
    /// <param name="playlistName"></param>
    /// <returns></returns>
    public static async Task<string> CreateDirectory(string path, string name, int choiceType, [Optional] string origin)
    {

        if(path is null ^ name is null) return string.Empty;



        if (choiceType == 1) {

            string list = Path.Combine(path ?? "", origin?? "" , name ?? "");
            if (!Directory.Exists(list)) Directory.CreateDirectory(list);

            return list;
        } 
        else 
        {
            string pathNumber = name+"1";
            var dict = await GetDirectories(Path.Combine(path ?? "", origin ?? "",name ?? ""));

            string newPath;
            int number = 1;
            // Create sequence of directory type
            if (dict.Count == 0)
            {
                newPath = Path.Combine(path, origin, name, pathNumber ?? "");
                await CreateDirectory(newPath);
            }else{

                number +=  dict.Count;
                newPath = Path.Combine(path, origin, name,name+$"{number}" ?? "");

                await CreateDirectory(newPath);
            }
            
            return newPath;
        }


    }

    /// <summary>
    /// Read Files Content get from pattern 
    /// </summary>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static async Task<List<string>> ReadContentWithSpecificInfos(List<string> elements){
        List<string> contents = new();

        foreach (var item in elements)
        {
            string [] arrayOfElements = await File.ReadAllLinesAsync(item);
            if(arrayOfElements != null) contents.AddRange(arrayOfElements);
        }
        
        return contents;
    }

    /// <summary>
    ///  Read Content From Different Location 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static async Task<List<string>> ReadContentWithSpecificInfos(string path, int type)
    {
        string tracking;

        if(type == 1)
           tracking =  await GetDocument(path, type);
        else
            tracking =  await GetDocument(path, type);
        var readers = await FileManager.ReadContentWithSpecificInfos(new List<string>(){tracking});
        
        return readers;
    }

    /// <summary>
    /// Get File To Compare With
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static async Task<string> GetDocument(string path, int type) 
    {
        string file;
        if(path is null ) return string.Empty;
        
        if(type == 1)
            file = Path.Combine(path ?? "", "musicplaylist.md");
        else
            file = Path.Combine(path ?? "", "musictracklist.md");
        
        await FileManager.CreateDocument(file);  

        return file;
    }

}